using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GameData
{
    public int slot;
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;
    // 필요한 다른 데이터 필드 추가
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private string dataPath;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        dataPath = Application.persistentDataPath + "/savedata";
    }

    public void SaveGame(int slot, Vector3 playerPosition)
    {
        GameData data = new GameData
        {
            slot = slot,
            playerPositionX = playerPosition.x,
            playerPositionY = playerPosition.y,
            playerPositionZ = playerPosition.z
            // 필요한 다른 데이터 설정
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(dataPath + slot + ".json", json);
    }

    public GameData LoadGame(int slot)
    {
        string filePath = dataPath + slot + ".json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            Debug.LogError("Save file not found in " + filePath);
            return null;
        }
    }

}
