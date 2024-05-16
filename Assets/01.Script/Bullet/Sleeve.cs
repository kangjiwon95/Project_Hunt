using Lean.Pool;
using System.Collections;
using UnityEngine;

public class Sleeve : MonoBehaviour
{
    [SerializeField]
    float force; // ź�ǰ� �߻�� ��

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Shoot();
    }

    private void Shoot()
    {
        // ź�ǿ� ���� ���� �������� �߻��մϴ�.
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        rb.AddForce(Vector3.left * force, ForceMode.Impulse);
        StartCoroutine(DestroySleeve());
    }

    private IEnumerator DestroySleeve()
    {
        yield return new WaitForSeconds(5f);

        LeanPool.Despawn(gameObject);
    }
}
