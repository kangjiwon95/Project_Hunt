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
    // 아이템 사용을 위해 총기를 들고있을 때 총기 숨김
    public GameObject rifleObj;

    [Space(20)]
    [Header("Item State")]
    private bool isWeapons = false; // 무기 들고있는상태

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
    public bool isSight = false; // 현재 사이트 상태 체크
    private bool isHold = false;  // 현재 숨참기 상태 체크
    private bool isTogglingSight = false; // 시야 전환 중 여부를 나타내는 변수

    [Space(20)]
    [Header("Zoom")]
    public Image Breath;
    [SerializeField]
    private float currentBreath = 0f;
    private float MaxBreath = 20;
    [SerializeField]
    private float zoomSpeed = 20;
    private float yRotation = 0f; // 머리의 Y 축 회전값

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

        #region 총기를 꺼낸 후 실행가능
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

    #region  총기 꺼내기, 장전 및 총기,탄약 확인

    // 총기 꺼내기
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


    //총기 장전
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
            //LeanPool.Spawn(sleeve, sleevePos);
            currentBullet--;
            CheckBullet();

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
    private void Sight()
    {
        if (!isTogglingSight && Input.GetKeyDown(KeyCode.Mouse1)) // 시야 전환 중이 아니고 마우스 우클릭이 눌렸을 때
        {
            isTogglingSight = true; // 시야 전환 중임을 나타내는 변수를 true로 설정
            isSight = !isSight; // 시야 상태를 토글

            if (isSight) // 시야가 열린 경우
            {
                leanSight.SetActive(false);
                scope.SetActive(true);
                cross.SetActive(true);
                animator.SetBool("isSight", true);
                mainCam.transform.position = sightCamPos.position;
            }
            else // 시야가 닫힌 경우
            {
                animator.SetBool("isSight", false);
                leanSight.SetActive(true);
                scope.SetActive(false);
                cross.SetActive(false);
                mainCam.transform.position = mainCamPos.position;
            }
        }
        else if (isTogglingSight && Input.GetKeyUp(KeyCode.Mouse1)) // 시야 전환 중이고 마우스 우클릭이 해제되었을 때
        {
            isTogglingSight = false; // 시야 전환 완료
        }
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

        // 숨쉬는 듯한 효과를 위해 카메라를 약간 회전
        if (!isHold)
        {
            // 숨쉬는 동안의 각도 변화를 시뮬레이션
            float breathingAngleX = Mathf.Sin(Time.time * 4f) * 2.1f; // 좌우로의 움직임
            float breathingAngleY = Mathf.Sin(Time.time * 4f) * 0.3f; // 상하로의 움직임

            sightMoveCam.transform.localRotation = Quaternion.Euler(yRotation + breathingAngleY, breathingAngleX * 0.5f, 0f);
        }
        else
        {
            BreathHold();
        }
    }

    // 숨 참기 발동
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

    // 숨 참기
    private void BreathHold()
    {
        currentBreath -= Time.deltaTime;

        if (currentBreath <= 0)
        {
            currentBreath = 0f;
        }
    }

    // 숨을 회복
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

    // 숨을 참을 때 sight 상태에서 폐 이미지 색 변경
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
