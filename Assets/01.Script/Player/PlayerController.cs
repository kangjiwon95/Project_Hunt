using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    CharacterController characterController;
    Animator animator;

    [Space(20)]
    [Header("Speed")]
    [SerializeField]
    float speed = 0f;
    [SerializeField]
    float baseSpeed = 5f;
    [SerializeField]
    float fatigueSpeed = 2f;
    [SerializeField]
    float runingSpeed = 10f;

    [Header("Bool")]
    private bool isRuning = false;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // 마우스 커서를 화면 안에 고정
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Speed();
        Move(speed);
        ApplyGravity();
    }

    private void Speed()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = runingSpeed;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = baseSpeed;
        }
    }

    // 플레이어의 움직임 조작
    private void Move(float Speed)
    {
        Camera.main.transform.forward = transform.forward;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        var moveDir = Camera.main.transform.forward * v + Camera.main.transform.right * h;

        Vector3 move = moveDir * Speed * Time.deltaTime;

        characterController.Move(move);
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded) // 캐릭터가 땅에 닿지 않은 경우
        {
            // 중력을 아래쪽으로 적용
            Vector3 gravity = Vector3.down * 9.81f * Time.deltaTime;
            characterController.Move(gravity);
        }
    }
}
