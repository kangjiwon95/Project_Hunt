using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // ���� ����ĳ��Ʈ�� �����Ͽ� �浹�� �����մϴ�.
        Collider[] colliders = Physics.OverlapSphere(transform.position, 20F);
        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Player")
            {
                animator.SetBool("isIteraction", true);
                PlayerCamp playerCamp = collider.GetComponent<PlayerCamp>();
                playerCamp.Camp();
            }
            else
            {
                animator.SetBool("isIteraction", false);
            }
        }
    }


}
