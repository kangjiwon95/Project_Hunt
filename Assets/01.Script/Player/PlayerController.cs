using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    CharacterController characterController;
    Animator animator;
    PlayerSound playerSound;
    PlayerRifle playerRifle;

    [Space(20)]
    [Header("Speed")]
    [SerializeField]
    float speed = 0f;
    [SerializeField]
    float baseSpeed = 5f;
    [SerializeField]
    float runingSpeed = 20f;

    [Space(20)]
    [Header("MouseMove")]
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

    [Header("UI Interaction")]
    float currentFatigue;
    float maxFatigue;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerSound = GetComponent<PlayerSound>();
        playerRifle = GetComponent<PlayerRifle>();
    }

    private void Start()
    {
        // ���콺 Ŀ���� ȭ�� �ȿ� ����
        Cursor.lockState = CursorLockMode.Locked;
        // ���콺 Ŀ�� ����
        Cursor.visible = false;

        // UI ��ȣ�ۿ� �ʱ� �� ����
        maxFatigue = 1000f;
        currentFatigue = maxFatigue;
    }

    private void Update()
    {
        Sprint();
        Move();
        ApplyGravity();
        Fatigue(2);
    }

    #region �޸���
    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && playerRifle.currentBreath > 0 && currentFatigue > 0)
        {
            isRuning = true;
            playerSound.Sound(6);
            Fatigue(5);
            playerRifle.BreathHold(10);
            playerRifle.ColorBreathHold();
        }
        else
        {
            isRuning = false;
            playerSound.Sound(2);
            playerRifle.BreathRecovery(5);
            playerRifle.ColorBreathHold();
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

        Vector3 move = moveDir.normalized * speed * Time.deltaTime;

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
    public void MouseMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        // ī�޶��� ���� ȸ��
        yRotation -= mouseY;
        // ���� ȸ�� ���� ���� (-5f, 3f)
        yRotation = Mathf.Clamp(yRotation, -5f, 3f);
        mainCam.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        // �÷��̾��� �¿� ȸ��
        transform.rotation *= Quaternion.Euler(0f, mouseX, 0f);
        arrow.transform.rotation *= Quaternion.Euler(0f, 0f, -mouseX);
    }

    public void SightMouseMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ī�޶��� ���� ȸ��
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -5f, 3f); // ���� ȸ�� ���� ���� (-40 , 20)
        sightMoveCam.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        
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

    public void FatigueRecovery()
    {
        currentFatigue = maxFatigue;
        fatigueGauge.fillAmount = currentFatigue / maxFatigue;
    }
    #endregion

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Dead")
        {
            print(other.tag);
            if (Input.GetKeyDown(KeyCode.F))
            {
                Destroy(other.gameObject);
                AnimalSpawn(other);
            }
        }
    }

    private void AnimalSpawn(Collider collider)
    {
        if(collider.name == "Moss")
        {
            GameManager.instance.SpawnAnimal(GameManager.instance.moss, GameManager.instance.mossPos);
        }
        else if(collider.name == "Bear")
        {
            GameManager.instance.SpawnAnimal(GameManager.instance.bear, GameManager.instance.bearPos);
        }
    }
}
