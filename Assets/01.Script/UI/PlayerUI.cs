using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("playerUI")]
    public PlayerUiManager uiManager;

    [Header("Key Expine")]
    public Button keyExpineButton;
    public GameObject keyExpine;

    [Header("Game Quit")]
    public Button gameQuitButton;
    public GameObject gameQuit;
    public Button yesButton;
    public Button noButton;

    private void Update()
    {
        KeyExpineButton();
        GameQuitButton();
        YesButton();
        NoButton();

        if (Input.GetButtonDown("Cancel"))
        {
            if(keyExpine.activeSelf)
            {
                keyExpine.SetActive(false);
                uiManager.exitButton.gameObject.SetActive(true);
            }
            else if(gameQuit.activeSelf)
            {
                gameQuit.SetActive(false);
                uiManager.exitButton.gameObject.SetActive(true);
            }
            else if(uiManager.campUI.activeSelf)
            {
                uiManager.campUI.SetActive(false);
            }
        }
    }

    private void KeyExpineButton()
    {
        keyExpineButton.onClick.AddListener( () => keyExpine.SetActive(true));
        keyExpineButton.onClick.AddListener(() => uiManager.exitButton.gameObject.SetActive(false));
    }

    #region 게임 종료 버튼
    private void GameQuitButton()
    {
        gameQuitButton.onClick.AddListener( () => gameQuit.SetActive(true));
        gameQuitButton.onClick.AddListener(() => uiManager.exitButton.gameObject.SetActive(false));
    }

    private void YesButton()
    {
        yesButton.onClick.AddListener(() => DataManager.instance.SaveGame(1,GameManager.instance.playerPos.position));
        yesButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
    }

    private void NoButton()
    {
        noButton.onClick.AddListener(() => gameQuit.SetActive(false));
    }

    #endregion
}
