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

    // 캠프 진입
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

    private void EscUi()
    {
        if (!escUI.activeSelf)
        {
            escUI.SetActive(true);
            // 마우스 커서를 화면 안에 고정 해체
            Cursor.lockState = CursorLockMode.None;
            // 마우스 커서 보임
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
