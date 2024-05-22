using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : AnimalFSM
{
    [SerializeField]
    protected float currentHP;
    protected float maxHP;

    private float currentHungry;
    private float maxHungry;

    private List<GameObject> bloodMarks = new List<GameObject>();

    [Header("Bool")]
    private bool isBlooding = false;

    private void Start()
    {
        // �ٸ� ������ ü�¸� ��ȯ
        maxHP = 400f;
        currentHP = maxHP;

        // ���� ������ �����
        maxHungry = 100f;
        currentHungry = maxHungry;

        currentState = AnimalState.Idle;
    }

    public override void Awake()
    {
        base.Awake();

        agent.enabled = false;
        agent.transform.position = GameManager.instance.bearPos.position;
        agent.enabled = true;
    }

    public override void Update()
    {
        base.Update();

        if (isBlooding)
        {
            currentHP -= Time.deltaTime * 0.5f;

            StartCoroutine(Blooding());
            if (currentHP <= 0)
            {
                currentHP = 0;
                Die();
                isBlooding = false; // ������ ���� �ߴ�
            }
        }
    }

    #region FSM ����
    public override void Idle()
    {
        // �޽� ���� ����
        animator.SetBool("isRest", true);
        agent.isStopped = true;
        UseHungryValue();
    }

    public override void Chase()
    {
        //  ��� ���� ����
        animator.SetBool("isChase", true);
        Serch();
    }

    public override void Run()
    {
        animator.SetBool("isSprint",true);
        Attack();
    }

    public override void Eat()
    {
        animator.SetTrigger("isEat");
    }

    public override void Die()
    {
        animator.SetTrigger("isDie");
        gameObject.tag = "Dead";
        currentHP = 0;
        agent.isStopped = false;
    }

    #endregion

    #region ���� ���� ���� ����
    private void Attack()
    {
        Collider[] two = Physics.OverlapSphere(transform.position,12F);
        foreach (Collider collider in two)
        {
            Moss moss = collider.GetComponent<Moss>();

            if (collider.tag == "Player")
            {
                animator.SetTrigger("isAttack");
            }
            else if (collider.name == "Moss")
            {
                animator.SetTrigger("isAttack");
                moss.TakeDamage(100f);
            }
            else if(collider.tag == "Dead")
            {
                animator.SetTrigger("isEat");
                Destroy(collider.gameObject);
                GameManager.instance.SpawnAnimal(GameManager.instance.moss, GameManager.instance.mossPos);
            }
        }
    }
    #endregion

    #region ������ ����
    public override void TakeDamage(float x)
    {
        currentHP -= x;
        isBlooding = true;
        Blood();
        
        if (currentHP <= 0)
        {
            currentHP = 0;
            isBlooding = false;
            Die();
        }
    }
    #endregion

    #region �����
    private void UseHungryValue()
    {
        currentHungry -= 5f * Time.deltaTime;

        if(currentHungry <= 0)
        {
            ChangeState(AnimalState.Chase);
        }
    }

    #endregion

    #region �ֺ� ��ɰ� ����
    public void Serch()
    {
        Collider[] two = Physics.OverlapSphere(transform.position, 100F);
        foreach (Collider collider in two)
        {
            if (collider.tag == "Player")
            {
                ChangeState(AnimalState.Run);
                agent.SetDestination(GameManager.instance.playerPos.position);
            }
            else if (collider.name == "Moss")
            {
                ChangeState(AnimalState.Run);
                agent.SetDestination(GameManager.instance.mossPos.position);
            }
        }
    }
    #endregion

    #region ���ڱ� ����
    public override void Blood()
    {
        GameObject newBlood = LeanPool.Spawn(blood, transform);
        bloodMarks.Add(newBlood);
    }

    IEnumerator Blooding()
    {
        while (isBlooding)
        {
            yield return new WaitForSeconds(30f);
            Blood();
        }

    }
    #endregion


    private void OnDestroy()
    {
        foreach (var mark in bloodMarks)
        {
            Destroy(mark);
        }
    }
}
