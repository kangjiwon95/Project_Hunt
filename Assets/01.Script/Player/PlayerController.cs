using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    CharacterController characterController;
    Animator animator;
    PlayerRifle playerRifle;

    [Space(20)]
    [Header("Speed")]
    [SerializeField]
    float speed = 0f;
    [SerializeField]
    float baseSpeed = 5f;
    [SerializeField]
    float runingSpeed = 10f;

    [Space(20)]
    [Header("Zoom , MouseMove")]
    public Camera mainCam;
    public Camera sightMoveCam;
    [SerializeField]
    private float yRotation = 0f; // �Ӹ��� Y �� ȸ����
    [SerializeField]
    private float mouseSensitivity = 100; // �Ӹ� ȸ���� �ӵ�

    [Header("Bool")]
    private bool isRuning = false;

    [Header("Compas")]
    public Image arrow;

    [Header("UI")]
    public Sprite[] fatigueImage;
    public Image fatigue;
    public Image fatigueGauge;
    public Image soundGauge;

    [Header("UI Interaction")]
    float currentFatigue;
    float maxFatigue;
    float currentSound;
    float maxSound;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerRifle = GetComponent<PlayerRifle>();
    }

    private void Start()
    {
        // ���콺 Ŀ���� ȭ�� �ȿ� ����
        Cursor.lockState = CursorLockMode.Locked;

        // UI ��ȣ�ۿ� �ʱ� �� ����
        maxFatigue = 1000f;
        currentFatigue = maxFatigue;

        currentSound = 0f;
        maxSound = 10f;
    }

    private void Update()
    {
        Sprint();
        Move();
        ApplyGravity();
        MouseMove();

        if(playerRifle.isSight)
        {
            sightMoveCam.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        }
        else
        {
            mainCam.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        }

        if (isRuning)
        {
            Sound(6);
            Fatigue(6);
        }
        else
        {
            Sound(2);
            Fatigue(1);
        }
    }

    #region �޸���
    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentFatigue > 0)
        {
            isRuning = true;
        }
        else
        {
            isRuning = false;
        }
        animator.SetBool("isSprint", isRuning);
    }
    #endregion

    #region �÷��̾��� ������ ����
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

        Vector3 move = moveDir * speed * Time.deltaTime;

        characterController.Move(move);
    }
    #endregion

    #region �߷� ����
    private void ApplyGravity()
    {
        if (!characterController.isGrounded) // ĳ���Ͱ� ���� ���� ���� ���
        {
            // �߷��� �Ʒ������� ����
            Vector3 gravity = Vector3.down * 9.81f * Time.deltaTime;
            characterController.Move(gravity);
        }
    }
    #endregion

    #region ���콺 ������
    private void MouseMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        // ī�޶��� ���� ȸ��
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -5f, 3f); // ���� ȸ�� ���� ���� (-40 , 20)
        // �÷��̾��� �¿� ȸ��
        transform.rotation *= Quaternion.Euler(0f, mouseX, 0f);
        arrow.transform.rotation *= Quaternion.Euler(0f, 0f, -mouseX);
    }
    #endregion

    #region UI ��ȣ�ۿ�
    public void Fatigue(float X)
    {
        currentFatigue -= X * Time.deltaTime;
        fatigueGauge.fillAmount = currentFatigue / maxFatigue;
        if (currentFatigue <= 0f)
        {
            currentFatigue = 0f;
        }
    }

    public void Sound(float X)
    {
        currentSound = X;
        soundGauge.fillAmount = currentSound / maxSound;
        if (currentSound >= 10f)
        {
            currentSound = 10f;
        }
    }

    #endregion
}
