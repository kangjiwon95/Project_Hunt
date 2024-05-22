using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }


    [Header("Animal")]
    public GameObject moss;
    public GameObject bear;
    public Transform mossPos;
    public Transform bearPos;

    [Header("Player")]
    public Transform playerPos;

    [Header("PatrolPos")]
    public Transform[] patrolPos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        SpawnAnimal(moss,mossPos);
        SpawnAnimal(bear,bearPos);
    }


    private void Update()
    {
    }

    public void SpawnAnimal(GameObject animal, Transform pos)
    {
        LeanPool.Spawn(animal, pos);
    }

    public void NewGame(int slot)
    {
        // 예제에서는 플레이어의 위치를 저장하는 것으로 가정합니다.
        Vector3 playerStartPosition = new Vector3(0, 0, 0); // 초기 위치
        DataManager.instance.SaveGame(slot, playerStartPosition);

        // 새로운 게임 씬 로드
        SceneManager.LoadScene("GameScene");
    }
}
