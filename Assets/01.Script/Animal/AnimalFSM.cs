using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AnimalState
{
    Idle,
    Patrol,
    Chase,
    Run,
    Eat,
}

public class AnimalFSM : MonoBehaviour
{
    protected AnimalState currentState;
    protected Animator animator;
    protected NavMeshAgent agent;

    [Header("Blood")]
    public GameObject blood;

    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public virtual void Update()
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
            case AnimalState.Run:
                Run();
                break;
            case AnimalState.Eat:
                Eat();
                break;
        }
    }

    #region 휴식 상태
    public virtual void Idle()
    {
    }
    #endregion

    #region 순찰 상태
    public virtual void Patrol()
    {
        // 탐색 로직 구현
    }
    #endregion

    #region 경계 상태
    public virtual void Chase()
    {
    }
    #endregion

    #region 도망 상태
    public virtual void Run()
    {
    }
    #endregion

    #region 식사 상태
    public virtual void Eat()
    {
        // 식사 로직 구현
    }
    #endregion

    #region 죽음 상태
    public virtual void Die()
    {
    }
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
}
