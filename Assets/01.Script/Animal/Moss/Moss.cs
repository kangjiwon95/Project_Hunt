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

    private List<GameObject> bloodMarks = new List<GameObject>();

    [Header("Bool")]
    private bool isBlooding = false;

    private void Start()
    {
        // 다른 동물들 체력만 변환
        maxHP = 400f;
        currentHP = maxHP;
    }

    private void Update()
    {
        if (isBlooding)
        {
            currentHP -= Time.deltaTime * 0.5f;

            if (currentHP <= 0)
            {
                currentHP = 0;
                ChangeState(AnimalState.Die);
                isBlooding = false; // 죽으면 출혈 중단
            }
        }
    }

    public override void TakeDamage(float x)
    {
        currentHP -= x;
        isBlooding = true;
        StartCoroutine(Blooding());
        ChangeState(AnimalState.Escape);
        if (currentHP <= 0)
        {
            currentHP = 0;
            isBlooding = false;
            ChangeState(AnimalState.Die);
        }
    }

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

    private void OnDestroy()
    {
        foreach (var mark in bloodMarks)
        {
            LeanPool.Despawn(mark);
        }
    }
}
