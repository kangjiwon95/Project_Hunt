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

    #region �޽� ����
    public virtual void Idle()
    {
    }
    #endregion

    #region ���� ����
    public virtual void Patrol()
    {
        // Ž�� ���� ����
    }
    #endregion

    #region ��� ����
    public virtual void Chase()
    {
    }
    #endregion

    #region ���� ����
    public virtual void Run()
    {
    }
    #endregion

    #region �Ļ� ����
    public virtual void Eat()
    {
        // �Ļ� ���� ����
    }
    #endregion

    #region ���� ����
    public virtual void Die()
    {
    }
    #endregion


    #region ������ �޴�
    public virtual void TakeDamage(float x)
    {
    }
    #endregion

    #region �� �긮��
    public virtual void Blood()
    {
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
}
