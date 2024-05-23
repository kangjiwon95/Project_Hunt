using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using System;

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
    public Button exitButton;

    [Space(20)]
    [Header("Panel")]
    public GameObject optionPanel;
    public GameObject gameQuitPanel;

    [Space(20)]
    [Header("NewGame, LoadGame")]
    public Button slot;
    public Text slotText;
    public GameObject newGamePanel;




    private void Start()
    {
        RandomBackImage();
        exitButton.onClick.AddListener(() => newGamePanel.SetActive(false));
        newGameButton.onClick.AddListener(() => newGamePanel.SetActive(true));
        slot.onClick.AddListener(() => NewGameButton(1));
        slotText.text = DateTime.Now.ToString("HHmmss");
    }

    private void Update()
    {
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
            int randomIndex = UnityEngine.Random.Range(0, backimages.Length);
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

    private void NewGameButton(int slot)
    {
        //ToDo: �߰� �۾�
        // ���������� �÷��̾��� ��ġ�� �����ϴ� ������ �����մϴ�.
        Vector3 playerStartPosition = new Vector3(0, 0, 130); // �ʱ� ��ġ
        DataManager.instance.SaveGame(slot, playerStartPosition);
        SceneManager.LoadScene("LoadingScene");
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
