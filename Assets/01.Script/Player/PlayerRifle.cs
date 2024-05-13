using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRifle : MonoBehaviour
{
    [Space(20)]
    [Header("Component")]
    Animator animator;
    PlayerSound playerSound;
    PlayerController playerController;

    [Header("RifleObject")]
    // ������ ����� ���� �ѱ⸦ ������� �� �ѱ� ����
    public GameObject rifleObj;

    [Space(20)]
    [Header("Item State")]
    private bool isWeapons = false; // ���� ����ִ»���

    [Space(20)]
    [Header("SightOn")]
    public Camera sightMoveCam;
    public Camera mainCam;
    public Transform sightCamPos;
    public Transform mainCamPos;
    public GameObject leanSight;
    public GameObject scope;
    public GameObject cross;

    [Header("Rifle State")]
    public bool isSight = false; // ���� ����Ʈ ���� üũ

    [Space(20)]
    [Header("Zoom")]
    public Image Breath;
    [SerializeField]
    private float currentBreath = 0f;
    private float MaxBreath = 10;
    [SerializeField]
    private float zoomSpeed = 20;

    [Space(20)]
    [Header("Bullet")]
    public Text bulletText;
    public Text maxBulletText;
    public Text magazineText;
    private float maxBullet;
    private float currentBullet;
    private float magazine;
    private bool isFire = false;

    [Space(20)]
    [Header("ShootGameObjet")]
    public GameObject sleeve;
    public Transform sleevePos;
    public GameObject bullet;
    public Transform bulletPos;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerSound = GetComponent<PlayerSound>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        rifleObj.SetActive(false);

        MaxBreath = 10f;
        currentBreath = MaxBreath;

        maxBullet = 6f;
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
            SightOn();
        }
        #endregion

        #region �� ����
        if (isSight)
        {
            Zoom();
            Shoot();
            playerController.SightMouseMove();
            BreathHold();
        }
        else
        {
            playerController.MouseMove();
            BreathRecovery();
        }
        #endregion
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
            LeanPool.Spawn(sleeve, sleevePos);
            // �Ѿ� ���� ��ġ�� ȸ�� ����
            Vector3 bulletSpawnPosition = sightMoveCam.transform.position + sightMoveCam.transform.forward * 0.5f; // ī�޶� ������ 0.5 ����
            Quaternion bulletSpawnRotation = Quaternion.LookRotation(sightMoveCam.transform.forward);

            // �Ѿ� ����
            Instantiate(bullet, bulletSpawnPosition, bulletSpawnRotation);
            currentBullet--;
            CheckBullet();
            playerSound.Sound(10);
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
    private void SightOn()
    {
        if (Input.GetKey(KeyCode.Mouse1) && currentBreath > 5) // �þ� ��ȯ ���� �ƴϰ� ���콺 ��Ŭ���� ������ ��
        {
            isSight = true;
            leanSight.SetActive(false);
            scope.SetActive(true);
            cross.SetActive(true);
            animator.SetBool("isSight", true);
            mainCam.transform.position = sightCamPos.position;
        }
        // �þ� ��ȯ ���̰� ���콺 ��Ŭ���� �����Ǿ��� ��
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            SightOff();
        }
    }

    private void SightOff()
    {
        isSight = false;
        animator.SetBool("isSight", false);
        leanSight.SetActive(true);
        scope.SetActive(false);
        cross.SetActive(false);
        mainCam.transform.position = mainCamPos.position;
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
    }

    // �� ����
    private void BreathHold()
    {
        currentBreath -= Time.deltaTime;

        if (currentBreath <= 0)
        {
            currentBreath = 0f;
            SightOff();
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
        if (currentBreath >= 8)
        {
            Breath.color = Color.green;
        }

        if (currentBreath <= 5)
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

    private void OnDrawGizmos()
    {
        // ī�޶��� ��ġ���� �����ؼ� ī�޶��� �������� ���̸� �׸��ϴ�.
        // �� ���̴� �����Ϳ����� ���̸� ���� ���� �߿��� ������ �ʽ��ϴ�.
        Gizmos.color = Color.red;  // ������ ������ ���������� ����
        Vector3 direction = sightMoveCam.transform.forward * 100;  // �������� 5 ������ ���̷� ����
        Gizmos.DrawRay(sightMoveCam.transform.position, direction);
    }
}
