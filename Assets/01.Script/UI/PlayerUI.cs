using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("playerUI")]
    public PlayerUiManager uiManager;
    public GameManager gameManager;

    [Header("Key Expine")]
    public Button keyExpineButton;
    public GameObject keyExpine;

    [Header("Game Quit")]
    public Button gameQuitButton;
    public GameObject gameQuit;
    public Button yesButton;
    public Button noButton;

    [Header("Inventory")]
    public GameObject inventory;
    public Text goldText;
    [SerializeField]
    public float currentGold;
    protected float maxGold;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        maxGold = 99999;
        currentGold = 10000f;
        goldText.text = currentGold.ToString();
    }

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
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            InventoryOpen();
        }
    }

    private void KeyExpineButton()
    {
        keyExpineButton.onClick.AddListener( () => keyExpine.SetActive(true));
        keyExpineButton.onClick.AddListener(() => uiManager.exitButton.gameObject.SetActive(false));
    }

    #region ���� ���� ��ư
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

    #region �κ��丮
    private void InventoryOpen()
    {
        inventory.SetActive(true);
        goldText.text = currentGold.ToString();
        // ���콺 Ŀ���� ȭ�� �ȿ� ���� ��ü
        Cursor.lockState = CursorLockMode.None;
        // ���콺 Ŀ�� ����
        Cursor.visible = true;
    }

    #endregion
}
