using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Update()
    {
        KeyExpineButton();
        GameQuitButton();

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

    private void GameQuitButton()
    {
        gameQuitButton.onClick.AddListener( () => gameQuit.SetActive(true));
        gameQuitButton.onClick.AddListener(() => uiManager.exitButton.gameObject.SetActive(false));
    }
}
