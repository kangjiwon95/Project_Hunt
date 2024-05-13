using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("BakcImage")]
    public Sprite[] backimages;
    public Image backGround;

    [Space(20)]
    [Header("Button")]
    public Button continueButton;
    public Button newGameButton;
    public Button optionButton;
    public Button gamequitButton;
    public Button yesButton;
    public Button noButton;

    [Space(20)]
    [Header("Panel")]
    public GameObject optionPanel;
    public GameObject gameQuitPanel;


    private void Start()
    {
        RandomBackImage();
    }

    private void Update()
    {
        NewGameButton();
        OptionButton();
        GameQuitPanel();
    }

    #region MainScene���� BackImage ���� ����
    private void RandomBackImage()
    {
        backGround.color = Color.white;
        // �̹��� �迭�� ������� ������ Ȯ��
        if (backimages.Length > 0)
        {
            // �������� �ε��� ����
            int randomIndex = Random.Range(0, backimages.Length);
            // ������ �̹����� backGround�� sprite�� ����
            backGround.sprite = backimages[randomIndex];
        }
        else
        {
            Debug.LogError("backimages �迭�� ����ֽ��ϴ�.");
        }
    }
    #endregion

    //���ٽ��� ���� �Լ� �̺�Ʈ ȣ��
    // Button.OnClick.AddListener(() => );

    #region New Game Button
    private void NewGameButton()
    {
        //ToDo: �߰� �۾�
        // ������ ���� �ҷ����� ����
        // ���Կ� ������ ����
        // �񵿱�� �� �ε� ����
        newGameButton.onClick.AddListener(() => SceneManager.LoadScene(1));
    }

    #endregion

    #region OptionButton
    private void OptionButton()
    {
        optionButton.onClick.AddListener(() => optionPanel.SetActive(true));

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            optionPanel.SetActive(false);
        }
    }
    #endregion

    #region �������� ��ư
    private void GameQuitPanel()
    {
        gamequitButton.onClick.AddListener(() => gameQuitPanel.SetActive(true));

        yesButton.onClick.AddListener(GameQuit);

        noButton.onClick.AddListener(() => gameQuitPanel.SetActive(false));
    }

    private void GameQuit()
    {
        #if UNITY_EDITOR
        // ������ �󿡼��� �����͸� �����մϴ�.
        UnityEditor.EditorApplication.isPlaying = false;

        // ����� ���ӿ����� ���ø����̼��� �����մϴ�.
        Application.Quit();
        #endif
    }
    #endregion
}
