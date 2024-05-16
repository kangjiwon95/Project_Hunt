using Lean.Pool;
using System.Collections;
using UnityEngine;

public class Sleeve : MonoBehaviour
{
    [SerializeField]
    float force; // 탄피가 발사될 힘

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
        // 탄피에 힘을 가해 우측위로 발사합니다.
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
