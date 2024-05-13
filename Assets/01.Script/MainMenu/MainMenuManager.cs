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

    #region MainScene에서 BackImage 랜덤 변경
    private void RandomBackImage()
    {
        backGround.color = Color.white;
        // 이미지 배열이 비어있지 않은지 확인
        if (backimages.Length > 0)
        {
            // 랜덤으로 인덱스 선택
            int randomIndex = Random.Range(0, backimages.Length);
            // 선택한 이미지를 backGround의 sprite로 설정
            backGround.sprite = backimages[randomIndex];
        }
        else
        {
            Debug.LogError("backimages 배열이 비어있습니다.");
        }
    }
    #endregion

    //람다식을 통해 함수 이벤트 호출
    // Button.OnClick.AddListener(() => );

    #region New Game Button
    private void NewGameButton()
    {
        //ToDo: 추가 작업
        // 데이터 저장 불러오기 제작
        // 슬롯에 데이터 저장
        // 비동기로 씬 로딩 구현
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

    #region 게임종료 버튼
    private void GameQuitPanel()
    {
        gamequitButton.onClick.AddListener(() => gameQuitPanel.SetActive(true));

        yesButton.onClick.AddListener(GameQuit);

        noButton.onClick.AddListener(() => gameQuitPanel.SetActive(false));
    }

    private void GameQuit()
    {
        #if UNITY_EDITOR
        // 에디터 상에서는 에디터를 종료합니다.
        UnityEditor.EditorApplication.isPlaying = false;

        // 빌드된 게임에서는 애플리케이션을 종료합니다.
        Application.Quit();
        #endif
    }
    #endregion
}
