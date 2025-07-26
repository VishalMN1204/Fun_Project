using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] LevelDataBase levelData;
    int currentLevelIndex;
    public CardLevelData CurrentLevel => levelData.cardLevels[currentLevelIndex];

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        SaveData data = SaveManager.Instance.LoadGame();
        if (data != null) CardController.Instance.LoadFromSaveData(data);
        else LoadLevel(CurrentLevel);
    }

    void LoadLevel(CardLevelData level,bool isLoad = false)
    {
        CardController.Instance.SetupLevel(level, isLoad);
    }

    public void IncrementLevel()
    {
        if (currentLevelIndex >= levelData.cardLevels.Count - 1)
        {
            UIManager.Instance.ShowEndGamePanel();
            return;
        }
        currentLevelIndex++;
        Debug.Log(currentLevelIndex.ToString());
        LoadLevel(CurrentLevel);
    }

    public void GetLevelOnLoad(int levelIndex)
    {
        foreach (CardLevelData cardLevel in levelData.cardLevels)
        {
            if (cardLevel.level == levelIndex) currentLevelIndex = levelIndex;
        }

        LoadLevel(CurrentLevel, isLoad: true);
    }
}
