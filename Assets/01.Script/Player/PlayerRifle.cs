using Lean.Pool;
using System;
using System.Collections;
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
    // 아이템 사용을 위해 총기를 들고있을 때 총기 숨김
    public GameObject rifleObj;

    [Space(20)]
    [Header("Item State")]
    private bool isWeapons = false; // 무기 들고있는상태

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
    public bool isSight = false; // 현재 사이트 상태 체크

    [Space(20)]
    [Header("Zoom")]
    public Image Breath;
    [SerializeField]
    public float currentBreath = 0f;
    protected float MaxBreath = 100;
    [SerializeField]
    private float zoomSpeed = 20;

    [Space(20)]
    [Header("Bullet")]
    public Text bulletText;
    public Text maxBulletText;
    public Text magazineText;
    private float maxBullet;
    private float currentBullet;
    public float magazine;
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

        MaxBreath = 100f;
        currentBreath = MaxBreath;

        maxBullet = 6f;
        currentBullet = maxBullet;
        magazine = 1f;
        bulletText.text = currentBullet.ToString();
        maxBulletText.text = maxBullet.ToString();
        magazineText.text = magazine.ToString();
    }

    private void Update()
    {
        GetWeapon();
        IdleChanged();

        #region 총기를 꺼낸 후 실행가능
        if (isWeapons)
        {
            Reload();
            Observe();
            IdleChanged();
            SightOn();
        }
        #endregion

        #region 줌 상태
        if (isSight)
        {
            Zoom();
            Shoot();
            playerController.SightMouseMove();
            BreathHold(1);
        }
        else
        {
            playerController.MouseMove();
            BreathRecovery(3);
        }
        #endregion
        ColorBreathHold();
    }

    #region  총기 꺼내기, 장전 및 총기,탄약 확인

    // 총기 꺼내기
    private void GetWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isWeapons && !Input.GetKey(KeyCode.LeftShift))
        {
            isWeapons = true;
            animator.SetBool("isGetWeapon", isWeapons);
            rifleObj.SetActive(true);
            leanSight.SetActive(true);
        }
    }


    //총기 장전
    private void Reload()
    {
        if (currentBullet < 1 && magazine > 0 && Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Reload");
            currentBullet = maxBullet;
            magazine--;
            if (magazine <= 0)
            {
                magazine = 0;
                magazineText.color = Color.red;
            }
            CheckBullet();
        }
    }

    // 총기 및 탄약 확인
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

    #region 총알 발사
    private void Shoot()
    {
        if (currentBullet > 0 && !isFire && Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("isShoot");
            StartCoroutine(WaitForNextShot());
            LeanPool.Spawn(sleeve, sleevePos);
            // 총알 생성 위치와 회전 조정
            Vector3 bulletSpawnPosition = sightMoveCam.transform.position + sightMoveCam.transform.forward * 0.5f; // 카메라 앞으로 0.5 미터
            Quaternion bulletSpawnRotation = Quaternion.LookRotation(sightMoveCam.transform.forward);

            // 총알 생성
            Instantiate(bullet, bulletSpawnPosition, bulletSpawnRotation);
            currentBullet--;
            CheckBullet();
            playerSound.Sound(10);
        }
        else if (currentBullet <= 0)
        {
            //TODO : 애니메이션 추가 또는 소리 추가
            print("총알없음");
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

    #region Sight 상태
    private void SightOn()
    {
        if (Input.GetKey(KeyCode.Mouse1) && currentBreath > 5) // 시야 전환 중이 아니고 마우스 우클릭이 눌렸을 때
        {
            isSight = true;
            leanSight.SetActive(false);
            scope.SetActive(true);
            cross.SetActive(true);
            animator.SetBool("isSight", true);
            mainCam.transform.position = sightCamPos.position;
        }
        // 시야 전환 중이고 마우스 우클릭이 해제되었을 때
        else if (Input.GetKeyUp(KeyCode.Mouse1))
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


    // zoom 입력
    private void Zoom()
    {
        // 휠 입력감지
        float zoom = Input.GetAxis("Mouse ScrollWheel");

        // 줌 속도에 따라 시야 조절
        sightMoveCam.fieldOfView -= zoom * zoomSpeed;

        // 시야를 최소값과 최대값 설정
        sightMoveCam.fieldOfView = Mathf.Clamp(sightMoveCam.fieldOfView, 5f, 20f);
    }

    // 숨 참기
    public void BreathHold(float x)
    {
        currentBreath -= x * Time.deltaTime;

        if (currentBreath <= 0)
        {
            currentBreath = 0f;
            SightOff();
        }
    }

    // 숨을 회복
    public void BreathRecovery(float x)
    {
        currentBreath += x + Time.deltaTime;

        if (currentBreath >= MaxBreath)
        {
            currentBreath = MaxBreath;
        }
    }

    // 숨을 참을 때 sight 상태에서 폐 이미지 색 변경
    private void ColorBreathHold()
    {
        if (currentBreath >= 80)
        {
            Breath.color = Color.green;
        }

        if (currentBreath <= 50)
        {
            Breath.color = Color.yellow;
        }

        if (currentBreath <= 10)
        {
            Breath.color = Color.red;
        }
    }
    #endregion

    #region Idle 상태로 변환
    // 초기 애니메이션 전환
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
    // 총기 오브젝트 숨기기
    private IEnumerator HideRifle(GameObject targetObject)
    {
        yield return new WaitForSeconds(0.5f);

        targetObject.SetActive(false);
    }
    #endregion
}
