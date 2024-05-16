using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : AnimalFSM
{
    [SerializeField]
    protected float currentHP;
    protected float maxHP;

    [Header("Bool")]
    private bool isBlooding = false;

    private void Start()
    {
        maxHP = 500f;
        currentHP = maxHP;
    }

    private void Update()
    {
        if(isBlooding)
        {
            currentHP -= Time.deltaTime * 0.5f;

            if (currentHP <= 0)
            {
                currentHP = 0;
                ChangeState(AnimalState.Die);
                isBlooding = false; // Á×À¸¸é ÃâÇ÷ Áß´Ü
            }
        }
        
    }

    public override void TakeDamage(float x)
    {
        currentHP -= x;
        Blood();
        isBlooding = true;

        if (currentHP <= 0)
        {
            currentHP = 0;
            isBlooding = false;
            ChangeState(AnimalState.Die);
        }
    }

    public override void Blood()
    {
        LeanPool.Spawn(blood, transform);
    }

    IEnumerator Blooding()
    {
        yield return new WaitForSeconds(30f);

    }
}
