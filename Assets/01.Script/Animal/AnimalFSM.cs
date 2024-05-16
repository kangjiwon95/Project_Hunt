using System.Collections;
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

    [Header("Blood")]
    public GameObject blood;

    [Header("TargetPos")]
    public Transform[] patrolTarget;
    public Transform escapeTarget;

    [Header("Bool")]
    private bool isSprint = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
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
        isSprint = false;
        StartCoroutine(ChangeCoroutine(AnimalState.Chase, 10f));
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
        animator.SetBool("isChase", true);
        PlayerSerch();
    }
    #endregion

    #region 도망 상태
    public virtual void Escape()
    {
        agent.speed = 5;

        // 배열에서 랜덤한 인덱스를 선택
        int randomIndex = Random.Range(0, patrolTarget.Length);

        // 선택된 인덱스의 위치를 도망 목적지로 설정
        Transform randomEscapeTarget = patrolTarget[randomIndex];
        agent.SetDestination(randomEscapeTarget.position);
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, 20F);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Player")
            {
                ChangeState(AnimalState.Escape);
            }
        }
    }
    #endregion

    #region 소리 감지

    #endregion

    #region 데미지 받다
    public virtual void TakeDamage(float x)
    {
    }
    #endregion

    #region 피 흘리다
    public virtual void Blood()
    {
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
