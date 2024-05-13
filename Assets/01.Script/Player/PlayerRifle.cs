using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRifle : MonoBehaviour
{
    [Space(20)]
    [Header("Animator")]
    Animator animator;

    [Header("RifleObject")]
    // ������ ����� ���� �ѱ⸦ ������� �� �ѱ� ����
    public GameObject rifleObj;

    [Space(20)]
    [Header("Item State")]
    private bool isWeapons = false; // ���� ����ִ»���

    [Space(20)]
    [Header("Sight")]
    public Camera sightMoveCam;
    public Camera mainCam;
    public Transform sightCamPos;
    public Transform mainCamPos;
    public GameObject leanSight;
    public GameObject scope;
    public GameObject cross;

    [Header("Rifle State")]
    public bool isSight = false; // ���� ����Ʈ ���� üũ
    private bool isHold = false;  // ���� ������ ���� üũ
    private bool isTogglingSight = false; // �þ� ��ȯ �� ���θ� ��Ÿ���� ����

    [Space(20)]
    [Header("Zoom")]
    public Image Breath;
    [SerializeField]
    private float currentBreath = 0f;
    private float MaxBreath = 20;
    [SerializeField]
    private float zoomSpeed = 20;
    private float yRotation = 0f; // �Ӹ��� Y �� ȸ����

    [Space(20)]
    [Header("Bullet")]
    public Text bulletText;
    public Text maxBulletText;
    public Text magazineText;
    private float maxBullet;
    private float currentBullet;
    private float magazine;
    private bool isFire = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        rifleObj.SetActive(false);

        MaxBreath = 5f;
        currentBreath = MaxBreath;

        maxBullet = 12f;
        currentBullet = maxBullet;
        magazine = 99f;
        bulletText.text = currentBullet.ToString();
        maxBulletText.text = maxBullet.ToString();
        magazineText.text = magazine.ToString();
    }

    private void Update()
    {
        GetWeapon();
        IdleChanged();

        #region �ѱ⸦ ���� �� ���డ��
        if (isWeapons)
        {
            Reload();
            Observe();
            IdleChanged();
            Sight();
        }
        #endregion

        if(isSight)
        {
            Zoom();
            Hold();
            Shoot();
        }

        ColorBreathHold();
    }

    #region  �ѱ� ������, ���� �� �ѱ�,ź�� Ȯ��

    // �ѱ� ������
    private void GetWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isWeapons)
        {
            isWeapons = true;
            animator.SetBool("isGetWeapon", isWeapons);
            rifleObj.SetActive(true);
            leanSight.SetActive(true);
        }
    }


    //�ѱ� ����
    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Reload");
            currentBullet = maxBullet;
            magazine--;
            if(magazine <= 0)
            {
                magazine = 0;
                magazineText.color = Color.red;
            }
            CheckBullet();
        }
    }

    // �ѱ� �� ź�� Ȯ��
    private void Observe()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetTrigger("ObserveWeapon");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("ObserveCartridge");
            CheckBullet();
        }
    }
    #endregion

    #region �Ѿ� �߻�
    private void Shoot()
    {
        if (currentBullet > 0 && !isFire && Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("isShoot");
            StartCoroutine(WaitForNextShot());
            //LeanPool.Spawn(sleeve, sleevePos);
            currentBullet--;
            CheckBullet();

        }
        else if (currentBullet <= 0)
        {
            //TODO : �ִϸ��̼� �߰� �Ǵ� �Ҹ� �߰�
            print("�Ѿ˾���");
        }
    }

    private IEnumerator WaitForNextShot()
    {
        isFire = true;
        yield return new WaitForSeconds(1.5f);
        isFire = false;
    }

    private void CheckBullet()
    {
        bulletText.text = currentBullet.ToString();
        maxBulletText.text = maxBullet.ToString();
        magazineText.text = magazine.ToString();
    }
    #endregion

    #region Sight ����
    private void Sight()
    {
        if (!isTogglingSight && Input.GetKeyDown(KeyCode.Mouse1)) // �þ� ��ȯ ���� �ƴϰ� ���콺 ��Ŭ���� ������ ��
        {
            isTogglingSight = true; // �þ� ��ȯ ������ ��Ÿ���� ������ true�� ����
            isSight = !isSight; // �þ� ���¸� ���

            if (isSight) // �þ߰� ���� ���
            {
                leanSight.SetActive(false);
                scope.SetActive(true);
                cross.SetActive(true);
                animator.SetBool("isSight", true);
                mainCam.transform.position = sightCamPos.position;
            }
            else // �þ߰� ���� ���
            {
                animator.SetBool("isSight", false);
                leanSight.SetActive(true);
                scope.SetActive(false);
                cross.SetActive(false);
                mainCam.transform.position = mainCamPos.position;
            }
        }
        else if (isTogglingSight && Input.GetKeyUp(KeyCode.Mouse1)) // �þ� ��ȯ ���̰� ���콺 ��Ŭ���� �����Ǿ��� ��
        {
            isTogglingSight = false; // �þ� ��ȯ �Ϸ�
        }
    }


    // zoom �Է�
    private void Zoom()
    {
        // �� �Է°���
        float zoom = Input.GetAxis("Mouse ScrollWheel");

        // �� �ӵ��� ���� �þ� ����
        sightMoveCam.fieldOfView -= zoom * zoomSpeed;

        // �þ߸� �ּҰ��� �ִ밪 ����
        sightMoveCam.fieldOfView = Mathf.Clamp(sightMoveCam.fieldOfView, 5f, 20f);

        // ������ ���� ȿ���� ���� ī�޶� �ణ ȸ��
        if (!isHold)
        {
            // ������ ������ ���� ��ȭ�� �ùķ��̼�
            float breathingAngleX = Mathf.Sin(Time.time * 4f) * 2.1f; // �¿���� ������
            float breathingAngleY = Mathf.Sin(Time.time * 4f) * 0.3f; // ���Ϸ��� ������

            sightMoveCam.transform.localRotation = Quaternion.Euler(yRotation + breathingAngleY, breathingAngleX * 0.5f, 0f);
        }
        else
        {
            BreathHold();
        }
    }

    // �� ���� �ߵ�
    private void Hold()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentBreath > 0)
        {
            isHold = true;
        }
        else
        {
            isHold = false;
            BreathRecovery();
        }
    }

    // �� ����
    private void BreathHold()
    {
        currentBreath -= Time.deltaTime;

        if (currentBreath <= 0)
        {
            currentBreath = 0f;
        }
    }

    // ���� ȸ��
    private void BreathRecovery()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            currentBreath += Time.deltaTime;
        }
        if (currentBreath >= 5)
        {
            currentBreath = MaxBreath;
        }
    }

    // ���� ���� �� sight ���¿��� �� �̹��� �� ����
    private void ColorBreathHold() 
    {
        if (currentBreath >= 5)
        {
            Breath.color = Color.green;
        }

        if (currentBreath <= 2.5)
        {
            Breath.color = Color.yellow;
        }

        if (currentBreath <= 0)
        {
            Breath.color = Color.red;
        }
    }
    #endregion

    #region Idle ���·� ��ȯ
    // �ʱ� �ִϸ��̼� ��ȯ
    private void IdleChanged()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isWeapons = false;
            animator.SetBool("isGetWeapon", isWeapons);
            leanSight.SetActive(false);
            StartCoroutine(HideRifle(rifleObj));
        }
    }
    // �ѱ� ������Ʈ �����
    private IEnumerator HideRifle(GameObject targetObject)
    {
        yield return new WaitForSeconds(0.5f);

        targetObject.SetActive(false);
    }
    #endregion
}
