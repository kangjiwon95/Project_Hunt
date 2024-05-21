using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : AnimalFSM
{
    [SerializeField]
    protected float currentHP;
    protected float maxHP;

    private List<GameObject> bloodMarks = new List<GameObject>();

    [Header("Bool")]
    private bool isBlooding = false;

    [Header("Die")]
    public GameObject dead;

    private void Start()
    {
        // �ٸ� ������ ü�¸� ��ȯ
        maxHP = 400f;
        currentHP = maxHP;

        currentState = AnimalState.Idle;
    }

    public override void Awake()
    {
        base.Awake();
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

        if (!dead.activeSelf)
        {
            Destroy(gameObject);
        }

    }

    #region FSM ����
    public override void Idle()
    {
        // �޽� ���� ����
        agent.isStopped = true;
    }

    public override void Patrol()
    {
        animator.SetBool("isPatrol", true);
    }

    public override void Chase()
    {
        //  ��� ���� ����
        agent.isStopped = false;
        animator.SetBool("isChase", true);
    }

    public override void Escape()
    {
        agent.speed = 5;
        animator.SetBool("isEscape", true);
    }

    public override void Eat()
    {
        animator.SetTrigger("isEat");
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
