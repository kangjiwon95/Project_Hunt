using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampUI : MonoBehaviour
{
    [Header("Rest")]
    public PlayerController playerController;
    public GameObject restUI;
    public Button rest;
    public Sprite[] backImage;
    public Image restImage;
    public Image restGauge;

    [Header("Shop")]
    public Button shop;
    public GameObject shopPanel;
    public Button buyButton;

    [Header("Player")]
    public PlayerRifle playerRifle;
    public PlayerUI playerUI;

    private void Start()
    {
        BuyButton();
        RestButton();
        ShopButton();
    }

    #region  휴식 버튼
    private void RestButton()
    {
        rest.onClick.AddListener(() => Camp());
    }

    private void Camp()
    {
        restUI.SetActive(true);
        playerController.enabled = false;
        StartCoroutine(RestCamp());
    }

    private IEnumerator RestCamp()
    {
        RandomBackImage();
        restGauge.fillAmount = 0f;
        float timer = 0f;
        while (timer < 10f)
        {
            timer += Time.deltaTime;
            restGauge.fillAmount = timer / 10f;
            yield return null;
        }
        restUI.SetActive(false);
        playerController.enabled = true;
        playerController.FatigueRecovery();
        // 마우스 커서를 화면 안에 고정
        Cursor.lockState = CursorLockMode.Locked;
        // 마우스 커서 숨김
        Cursor.visible = false;
    }

    private void RandomBackImage()
    {
        restImage.color = Color.white;
        // 이미지 배열이 비어있지 않은지 확인
        if (backImage.Length > 0)
        {
            // 랜덤으로 인덱스 선택
            int randomIndex = Random.Range(0, backImage.Length);
            // 선택한 이미지를 backGround의 sprite로 설정
            restImage.sprite = backImage[randomIndex];
        }
    }
    #endregion

    #region 상점 이용
    private void ShopButton()
    {
        shop.onClick.AddListener(() => Shop());
    }

    private void Shop()
    {
        shopPanel.SetActive(true);
    }

    private void BuyMagazine()
    {
        if (playerUI.currentGold > 499f)
        {
            // 탄창 구매
            playerRifle.magazine = playerRifle.magazine + 1;
            playerRifle.magazineText.text = playerRifle.magazine.ToString();
            playerUI.currentGold -= 500f;
        }
        else
        {
            print("구매 불가");
        }
    }

    private void BuyButton()
    {
        buyButton.onClick.AddListener(() => BuyMagazine());
    }
    #endregion
}
