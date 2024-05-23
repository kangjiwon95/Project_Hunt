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

    #region �г� �ݱ�
    private void OffUI()
    {
        bool isActive = false;

        foreach (GameObject panel in uiPanel)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                // ���콺 Ŀ���� ȭ�� �ȿ� ����
                Cursor.lockState = CursorLockMode.Locked;
                // ���콺 Ŀ�� ����
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

    #region ķ��UI Ȱ��
    public void Camp()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            campUI.SetActive(true);
            // ���콺 Ŀ���� ȭ�� �ȿ� ���� ��ü
            Cursor.lockState = CursorLockMode.None;
            // ���콺 Ŀ�� ����
            Cursor.visible = true;
        }
    }
    #endregion

    #region EscUI
    private void EscUi()
    {
        escUI.SetActive(true);
        // ���콺 Ŀ���� ȭ�� �ȿ� ���� ��ü
        Cursor.lockState = CursorLockMode.None;
        // ���콺 Ŀ�� ����
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
