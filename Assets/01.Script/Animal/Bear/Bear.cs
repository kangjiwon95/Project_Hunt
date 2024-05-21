using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : AnimalFSM
{
    [SerializeField]
    protected float currentHP;
    protected float maxHP;
    protected float currentfatigue;
    protected float maxfatigue;

    private List<GameObject> bloodMarks = new List<GameObject>();

    [Header("Bool")]
    private bool isBlooding = false;

    private void Start()
    {
        // 다른 동물들 체력만 변환
        maxHP = 400f;
        currentHP = maxHP;
        // 동물의 피로도
        maxfatigue = 100f;
        currentfatigue = maxfatigue;

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
        RecoveryFatigue();
    }

    public override void Patrol()
    {
        Fatigue(3f);
    }

    public override void Chase()
    {
        //  경계 로직 구현
        Fatigue(2);
    }

    public override void Escape()
    {
        agent.speed = 10;
        Fatigue(5);
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
        agent.enabled = false;
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

    #region 피로도 구현
    private void Fatigue(float x)
    {
        currentfatigue -= x * Time.deltaTime;

        if (currentfatigue <= 0)
        {
            currentfatigue = 0;
            animator.SetBool("isIdle", true);
            ChangeState(AnimalState.Idle);
        }
    }

    private void RecoveryFatigue()
    {
        currentfatigue += 5 * Time.deltaTime;

        if (currentfatigue >= maxfatigue)
        {
            currentfatigue = maxfatigue;
            ChangeState(AnimalState.Chase);
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
