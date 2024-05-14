using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AnimalState
{
    Idle,
    Patrol,
    Chase,
    Escape,
    Eat,
    Die
}

public class AnimalFSM : MonoBehaviour
{
    private AnimalState currentState;
    protected Animator animator;
    NavMeshAgent agent;

    [SerializeField]
    protected float currentHP;
    protected float maxHP;

    [Header("Blood")]
    public GameObject blood;


    [Header("Bool")]
    private bool isSprint = false;

    [Header("TargetPos")]
    public Transform[] patrolTarget;
    public Transform escapeTarget;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentState = AnimalState.Idle;
    }

    void Update()
    {
        switch (currentState)
        {
            case AnimalState.Idle:
                Idle();
                break;
            case AnimalState.Patrol:
                Patrol();
                break;
            case AnimalState.Chase:
                Chase();
                break;
            case AnimalState.Escape:
                Escape();
                break;
            case AnimalState.Eat:
                Eat();
                break;
            case AnimalState.Die:
                Die();
                break;
        }
    }
    #region �޽� ����
    void Idle()
    {
        // �޽� ���� ����
        agent.isStopped = true;
        animator.SetTrigger("isRest");
    }
    #endregion

    #region ���� ����
    void Patrol()
    {
        // Ž�� ���� ����
    }
    #endregion

    #region ��� ����
    void Chase()
    {
        //  ��� ���� ����

    }
    #endregion

    #region ���� ����
    public virtual void Escape()
    {
        agent.speed = 5;
        isSprint = true;
        agent.SetDestination(escapeTarget.transform.position);
    }
    #endregion

    #region �Ļ� ����
    void Eat()
    {
        // �Ļ� ���� ����
    }
    #endregion

    #region ���� ����
    void Die()
    {
        animator.SetTrigger("isDie");
    }
    #endregion

    #region �÷��̾� ����
    protected void PlayerSerch()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5F);
        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Player")
            {

            }
        }
    }
    #endregion

    #region �Ҹ� ����

    #endregion

    #region ������ �޴�
    public void TakeDamage(float x)
    {
        currentHP -= x;
        Blood();
        currentHP -= Time.deltaTime;
        if(currentHP <= 0)
        {
            currentHP = 0;
            ChangeState(AnimalState.Die);
        }
    }
    #endregion

    #region �� �긮��
    private void Blood()
    {
        LeanPool.Spawn(blood , transform);
    }
    #endregion

    #region ���� ��ȯ �޼���
    // ��� ��ȯ
    public void ChangeState(AnimalState newState)
    {
        currentState = newState;
    }

    // �ð� �� ��ȯ

    public IEnumerator ChangeCoroutine(AnimalState newState, float x)
    {
        yield return new WaitForSeconds(x);

        currentState = newState;
    }
    #endregion

    #region ��� ���� ���
    public virtual void HeadDie() { }

    public virtual void HeartDie() { }
    #endregion
}
