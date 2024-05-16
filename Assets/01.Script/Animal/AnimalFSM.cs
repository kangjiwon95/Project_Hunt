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
    #region �޽� ����
    void Idle()
    {
        // �޽� ���� ����
        agent.isStopped = true;
        isSprint = false;
        StartCoroutine(ChangeCoroutine(AnimalState.Chase, 10f));
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
        animator.SetBool("isChase", true);
        PlayerSerch();
    }
    #endregion

    #region ���� ����
    public virtual void Escape()
    {
        agent.speed = 5;

        // �迭���� ������ �ε����� ����
        int randomIndex = Random.Range(0, patrolTarget.Length);

        // ���õ� �ε����� ��ġ�� ���� �������� ����
        Transform randomEscapeTarget = patrolTarget[randomIndex];
        agent.SetDestination(randomEscapeTarget.position);
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

    #region �Ҹ� ����

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

    #region ��� ���� ���
    public virtual void HeadDie() { }

    public virtual void HeartDie() { }
    #endregion
}
