using UnityEngine;
using UnityEngine.UI;

public class PlayerUiManager : MonoBehaviour
{
    [Header("UI Object")]
    public GameObject escUI;
    public GameObject campUI;

    [Header("Button")]
    public Button exitButton;


    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            EscUi();
        }
        ExitButton();
    }

    // ķ�� ����
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

    private void EscUi()
    {
        if (!escUI.activeSelf)
        {
            escUI.SetActive(true);
            // ���콺 Ŀ���� ȭ�� �ȿ� ���� ��ü
            Cursor.lockState = CursorLockMode.None;
            // ���콺 Ŀ�� ����
            Cursor.visible = true;
        }
    }

    private void ExitButton()
    {
        exitButton.onClick.AddListener(() => escUI.SetActive(false));
        exitButton.onClick.AddListener(() => Cursor.lockState = CursorLockMode.Locked);
        exitButton.onClick.AddListener(() => Cursor.visible = false);
    }
}
