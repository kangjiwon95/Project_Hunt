using UnityEngine;
using UnityEngine.UI;

public class PlayerUiManager : MonoBehaviour
{
    [Header("UI Object")]
    public GameObject escUI;
    public GameObject campUI;
    public GameObject restUI;
    public GameObject shopUI;
    public GameObject inventoryUI;
    public PlayerUI playerUI;
    public GameObject[] uiPanel;

    [Header("Button")]
    public Button exitButton;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (restUI.activeSelf)
            {
                return;
            }
            OffUI();
        }
        ExitButton();
    }

    #region 패널 닫기
    private void OffUI()
    {
        bool isActive = false;

        foreach (GameObject panel in uiPanel)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                // 마우스 커서를 화면 안에 고정
                Cursor.lockState = CursorLockMode.Locked;
                // 마우스 커서 숨김
                Cursor.visible = false;
                return;
            }
        }

        if(!isActive)
        {
            EscUi();
        }
    }

    #endregion

    #region 캠프UI 활성
    public void Camp()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            campUI.SetActive(true);
            // 마우스 커서를 화면 안에 고정 해체
            Cursor.lockState = CursorLockMode.None;
            // 마우스 커서 보임
            Cursor.visible = true;
        }
    }
    #endregion

    #region EscUI
    private void EscUi()
    {
        escUI.SetActive(true);
        // 마우스 커서를 화면 안에 고정 해체
        Cursor.lockState = CursorLockMode.None;
        // 마우스 커서 보임
        Cursor.visible = true;
    }

    private void ExitButton()
    {
        exitButton.onClick.AddListener(() => escUI.SetActive(false));
        exitButton.onClick.AddListener(() => Cursor.lockState = CursorLockMode.Locked);
        exitButton.onClick.AddListener(() => Cursor.visible = false);
    }
    #endregion
}
