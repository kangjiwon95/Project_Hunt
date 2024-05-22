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
        // 다른 동물들 체력만 변환
        maxHP = 400f;
        currentHP = maxHP;

        // 육식 동물의 배고픔
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
                isBlooding = false; // 죽으면 출혈 중단
            }
        }
    }

    #region FSM 구현
    public override void Idle()
    {
        // 휴식 로직 구현
        animator.SetBool("isRest", true);
        agent.isStopped = true;
        UseHungryValue();
    }

    public override void Chase()
    {
        //  경계 로직 구현
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

    #region 육식 동물 공격 구현
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

    #region 데미지 구현
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

    #region 배고픔
    private void UseHungryValue()
    {
        currentHungry -= 5f * Time.deltaTime;

        if(currentHungry <= 0)
        {
            ChangeState(AnimalState.Chase);
        }
    }

    #endregion

    #region 주변 사냥감 감지
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

    #region 핏자국 구현
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
