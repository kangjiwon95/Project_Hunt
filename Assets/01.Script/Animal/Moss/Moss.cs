using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Moss : AnimalFSM
{
    [SerializeField]
    protected float currentHP;
    protected float maxHP;
    protected float currentfatigue;
    protected float maxfatigue;

    private List<GameObject> bloodMarks = new List<GameObject>();

    [Header("Bool")]
    private bool isBlooding = false;
    private bool isMovingToPatrolPoint = false;

    private void Start()
    {
        // ������ ü��
        maxHP = 400f;
        currentHP = maxHP;
        // ������ �Ƿε�
        maxfatigue = 100f;
        currentfatigue = maxfatigue;


        currentState = AnimalState.Idle;
    }

    public override void Awake()
    {
        base.Awake();

        agent.enabled = false;
        agent.transform.position = GameManager.instance.mossPos.position;
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
        //�޽� ���� ����
        RecoveryFatigue();
    }

    public override void Patrol()
    {
        agent.speed = 5f;
        if (!isMovingToPatrolPoint)
        {
            MoveRandomPoint();
            isMovingToPatrolPoint = true;
        }

        Vector3 direction = agent.steeringTarget - transform.position;
        Vector3 localDirection = transform.InverseTransformDirection(direction);

        animator.SetFloat("xDir", localDirection.x);
        animator.SetFloat("yDir", localDirection.z);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isMovingToPatrolPoint = false;
            Fatigue(maxfatigue);
        }
    }

    public override void Chase()
    {
        //  ��� ���� ����
        animator.SetBool("isChase",true);
        Serch();
    }

    public override void Run()
    {
        agent.speed = 10;
        animator.SetBool("isSprint", true);
        if (!isMovingToPatrolPoint)
        {
            MoveRandomPoint();
            isMovingToPatrolPoint = true;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isMovingToPatrolPoint = false;
            animator.SetBool("isSprint", false);
            Fatigue(maxfatigue);
        }
    }

    public override void Eat()
    {
        animator.SetTrigger("isEat");
    }

    public override void Die()
    {
        agent.isStopped = true;
        animator.SetTrigger("isDie");
        gameObject.tag = "Dead";
        currentHP = 0;
    }
    #endregion

    #region �Ƿε� ����
    private void Fatigue(float x)
    {
        currentfatigue -= x * Time.deltaTime;

        if(currentfatigue <= 0)
        {
            currentfatigue = 0;
            agent.isStopped = true;
            ChangeState(AnimalState.Idle);
        }
    }

    private void RecoveryFatigue()
    {
        currentfatigue += 5 * Time.deltaTime;

        if(currentfatigue >= maxfatigue)
        {
            currentfatigue = maxfatigue;
            agent.isStopped = false;
            ChangeState(AnimalState.Chase);
        }
    }
    #endregion

    #region ������ ����
    public override void TakeDamage(float x)
    {
        currentHP -= x;
        isBlooding = true;
        ChangeState(AnimalState.Run);
        Blood();
        if (currentHP <= 0)
        {
            currentHP = 0;
            isBlooding = false;
            Die();
        }
    }

    #endregion

    #region �ֺ� ����
    public void Serch()
    {
        Collider[] two = Physics.OverlapSphere(transform.position, 20F);
        foreach (Collider collider in two)
        {
            if (collider.tag == "Player")
            {
                ChangeState(AnimalState.Run);
            }
            else if (collider.tag != "Player")
            {
                StartCoroutine(ChangeCoroutine(AnimalState.Patrol,10f));
            }
        }
    }
    #endregion

    #region ������ ����Ʈ �̵�(Patrol)
    private void MoveRandomPoint()
    {
        Transform[] patrolPoints = GameManager.instance.patrolPos;
        if (patrolPoints.Length == 0) return;

        int randomIndex = Random.Range(0, patrolPoints.Length);
        Vector3 randomPatrolPoint = patrolPoints[randomIndex].position;
        agent.SetDestination(randomPatrolPoint);
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

    // ������ Destroy ���ڱ� ����
    private void OnDestroy()
    {
        foreach (var mark in bloodMarks)
        {
            Destroy(mark);
        }
    }
    #endregion
}
