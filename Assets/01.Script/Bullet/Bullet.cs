using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            AnimalFSM animal = collider.GetComponentInParent<AnimalFSM>();
            print(collider.tag);
            if (collider.tag == "Head")
            {
                Destroy(gameObject);
                animal.ChangeState(AnimalState.Die);
            }
            else if (collider.tag == "Heart")
            {
                Destroy(gameObject);
                animal.ChangeState(AnimalState.Die);
            }
            else if (collider.tag == "Animal")
            { 
                Destroy(gameObject);
                animal.TakeDamage(150);
            }
            else if (collider.tag == "Rock")
            {
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
