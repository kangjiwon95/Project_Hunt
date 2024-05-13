using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    private float force;

    [SerializeField]
    private GameObject rockHitParticlePrefab; // 파티클 시스템 프리팹 참조

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 원형 레이캐스트를 실행하여 충돌을 검출합니다.
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5F);
        foreach (Collider collider in colliders)
        {
            //Animal animal = collider.GetComponentInParent<Animal>();

            if (collider.tag == "Head")
            {
                print("머리");
                Destroy(gameObject);
                //animal.HeadDie();
            }
            else if (collider.tag == "Heart")
            {
                print("심장");
                Destroy(gameObject);
            }
            else if(collider.tag == "Rock")
            {
                print("맞았다");
                if (rockHitParticlePrefab != null)
                {
                    Instantiate(rockHitParticlePrefab, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }

    private void OnEnable()
    {
        rb.useGravity = false;
        // 총알에 힘을 가하여 앞으로 날가아게 함
        rb.AddForce(transform.forward * force, ForceMode.Impulse);

        StartCoroutine(DestroyBullet());
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
