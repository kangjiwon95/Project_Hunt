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
    private GameObject rockHitParticlePrefab; // ��ƼŬ �ý��� ������ ����

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // ���� ����ĳ��Ʈ�� �����Ͽ� �浹�� �����մϴ�.
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
        // �Ѿ˿� ���� ���Ͽ� ������ �����ư� ��
        rb.AddForce(transform.forward * force, ForceMode.Impulse);

        StartCoroutine(DestroyBullet());
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
