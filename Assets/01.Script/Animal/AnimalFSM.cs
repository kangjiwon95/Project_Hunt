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
}

public class AnimalFSM : MonoBehaviour
{
    protected AnimalState currentState;
    protected Animator animator;
    protected NavMeshAgent agent;

    [Header("Blood & DIe")]
    public GameObject blood;
    public GameObject spain;

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
            case AnimalState.Escape:
                Escape();
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
    public virtual void Escape()
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
    public void Die()
    {
        print("����");
        animator.SetTrigger("isDie");
        spain.tag = "Dead";
    }
    #endregion

    #region �÷��̾� ����
    public void PlayerSerch()
    {
        Collider[] two = Physics.OverlapSphere(transform.position, 20F);
        foreach (Collider collider in two)
        {
            if(collider.tag == "Player")
            {
                
            }
            else if(collider.tag != "Player")
            {
                
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
}
