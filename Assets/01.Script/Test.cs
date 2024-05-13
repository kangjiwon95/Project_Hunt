using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            print("¸Â¾Ò´Ù");
        }
    }
}
