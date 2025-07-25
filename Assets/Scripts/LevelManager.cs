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
        LoadLevel(CurrentLevel);
    }

    void LoadLevel(CardLevelData level)
    {
        CardController.Instance.SetupLevel(level);
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
}
