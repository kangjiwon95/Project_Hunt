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

    [Header("EscUI")]
    public GameObject escUI;



    private void Update()
    {
        RestButton();
        ShopButton();
    }

    #region  �޽� ��ư
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
        // ���콺 Ŀ���� ȭ�� �ȿ� ����
        Cursor.lockState = CursorLockMode.Locked;
        // ���콺 Ŀ�� ����
        Cursor.visible = false;
    }

    private void RandomBackImage()
    {
        restImage.color = Color.white;
        // �̹��� �迭�� ������� ������ Ȯ��
        if (backImage.Length > 0)
        {
            // �������� �ε��� ����
            int randomIndex = Random.Range(0, backImage.Length);
            // ������ �̹����� backGround�� sprite�� ����
            restImage.sprite = backImage[randomIndex];
        }
    }
    #endregion

    #region ���� �̿�
    private void ShopButton()
    {
        shop.onClick.AddListener(() => Shop());
    }

    private void Shop()
    {

    }
    #endregion
}
