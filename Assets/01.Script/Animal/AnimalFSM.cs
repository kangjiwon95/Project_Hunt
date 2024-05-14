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
    #region 휴식 상태
    void Idle()
    {
        // 휴식 로직 구현
        agent.isStopped = true;
        animator.SetTrigger("isRest");
    }
    #endregion

    #region 순찰 상태
    void Patrol()
    {
        // 탐색 로직 구현
    }
    #endregion

    #region 경계 상태
    void Chase()
    {
        //  경계 로직 구현

    }
    #endregion

    #region 도망 상태
    public virtual void Escape()
    {
        agent.speed = 5;
        isSprint = true;
        agent.SetDestination(escapeTarget.transform.position);
    }
    #endregion

    #region 식사 상태
    void Eat()
    {
        // 식사 로직 구현
    }
    #endregion

    #region 죽음 상태
    void Die()
    {
        animator.SetTrigger("isDie");
    }
    #endregion

    #region 플레이어 감지
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

    #region 소리 감지

    #endregion

    #region 데미지 받다
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

    #region 피 흘리다
    private void Blood()
    {
        LeanPool.Spawn(blood , transform);
    }
    #endregion

    #region 상태 전환 메서드
    // 즉시 전환
    public void ChangeState(AnimalState newState)
    {
        currentState = newState;
    }

    // 시간 차 전환

    public IEnumerator ChangeCoroutine(AnimalState newState, float x)
    {
        yield return new WaitForSeconds(x);

        currentState = newState;
    }
    #endregion

    #region 즉사 부위 사망
    public virtual void HeadDie() { }

    public virtual void HeartDie() { }
    #endregion
}
