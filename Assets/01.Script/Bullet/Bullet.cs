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
            //Animal animal = collider.GetComponentInParent<Animal>();

            if (collider.tag == "Head")
            {
                print("�Ӹ�");
                Destroy(gameObject);
                //animal.HeadDie();
            }
            else if (collider.tag == "Heart")
            {
                print("����");
                Destroy(gameObject);
            }
            else if(collider.tag == "Rock")
            {
                print("�¾Ҵ�");
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
