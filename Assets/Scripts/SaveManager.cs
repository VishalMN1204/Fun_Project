using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    string savePath = string.Empty;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "cardMatch.json");
    }
    public void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"Game saved to {savePath}");
    }

    public SaveData LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found.");
            return null;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        return data;
    }

    void OnApplicationQuit()
    {
        SaveData data = CardController.Instance.CreateSaveData();
        SaveGame(data);
    }

}

[System.Serializable]
public class SaveData
{
    public int levelIndex;
    public int rows;
    public int columns;
    public List<int> matchedCardIndices = new();
    public List<int> spriteIndices; // Indexes from the atlas for each card
    public List<bool> matchedCards; // True if the card at index is matched
    public int matchScore;
    public int turnScore;
}
