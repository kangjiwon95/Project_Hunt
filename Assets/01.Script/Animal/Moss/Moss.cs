using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Moss : AnimalFSM
{
    private void Start()
    {
        currentHP = maxHP;
        maxHP = 300f;
    }
}
