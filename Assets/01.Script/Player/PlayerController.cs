using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    CharacterController characterController;
    Animator animator;
    PlayerSound playerSound;

    [Space(20)]
    [Header("Speed")]
    [SerializeField]
    float speed = 0f;
    [SerializeField]
    float baseSpeed = 5f;
    [SerializeField]
    float runingSpeed = 10f;

    [Space(20)]
    [Header("MouseMove")]
    public Camera mainCam;
    public Camera sightMoveCam;
    [SerializeField]
    private float yRotation = 0f; // 머리의 Y 축 회전값
    [SerializeField]
    private float mouseSensitivity = 100; // 머리 회전의 속도

    [Header("Bool")]
    private bool isRuning = false;

    [Header("Compas")]
    public Image arrow;

    [Header("UI")]
    public Sprite[] fatigueImage;
    public Image fatigue;
    public Image fatigueGauge;

    [Header("UI Interaction")]
    float currentFatigue;
    float maxFatigue;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerSound = GetComponent<PlayerSound>();
    }

    private void Start()
    {
        // 마우스 커서를 화면 안에 고정
        Cursor.lockState = CursorLockMode.Locked;

        // UI 상호작용 초기 값 설정
        maxFatigue = 1000f;
        currentFatigue = maxFatigue;
    }

    private void Update()
    {
        Sprint();
        Move();
        ApplyGravity();
    }

    #region 달리기
    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentFatigue > 0)
        {
            isRuning = true;
            playerSound.Sound(6);
        }
        else
        {
            isRuning = false;
            playerSound.Sound(2);
        }
        animator.SetBool("isSprint", isRuning);
    }
    #endregion

    #region 플레이어의 움직임 조작
    private void Move()
    {
        Camera.main.transform.forward = transform.forward;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        var moveDir = Camera.main.transform.forward * v + Camera.main.transform.right * h;

        if (isRuning)
        {
            speed = runingSpeed;
        }
        else
        {
            speed = baseSpeed;
        }

        Vector3 move = moveDir.normalized * speed * Time.deltaTime;

        characterController.Move(move);
    }
    #endregion

    #region 중력 적용
    private void ApplyGravity()
    {
        if (!characterController.isGrounded) // 캐릭터가 땅에 닿지 않은 경우
        {
            // 중력을 아래쪽으로 적용
            Vector3 gravity = Vector3.down * 9.81f * Time.deltaTime;
            characterController.Move(gravity);
        }
    }
    #endregion

    #region 마우스 움직임
    public void MouseMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        // 카메라의 상하 회전
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -5f, 3f); // 상하 회전 각도 제한 (-40 , 20)
        mainCam.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        // 플레이어의 좌우 회전
        transform.rotation *= Quaternion.Euler(0f, mouseX, 0f);
        arrow.transform.rotation *= Quaternion.Euler(0f, 0f, -mouseX);
    }

    public void SightMouseMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 카메라의 상하 회전
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -5f, 3f); // 상하 회전 각도 제한 (-40 , 20)
        sightMoveCam.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        
        // 플레이어의 좌우 회전
        transform.rotation *= Quaternion.Euler(0f, mouseX, 0f);
        arrow.transform.rotation *= Quaternion.Euler(0f, 0f, -mouseX);
    }
    #endregion

    #region UI 상호작용
    public void Fatigue(float X)
    {
        currentFatigue -= X * Time.deltaTime;
        fatigueGauge.fillAmount = currentFatigue / maxFatigue;
        if (currentFatigue <= 0f)
        {
            currentFatigue = 0f;
        }
    }
    #endregion
}
