using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnsTxt;
    [SerializeField] TextMeshProUGUI matchTxt;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Image loadingImage;
    [SerializeField] Button nextLevelBtn;
    [SerializeField] GameObject endGamePanel;
    [SerializeField] Button playAgainBtn;
    [SerializeField] Button exitGameBtn;
    float turnScore;
    float matchScore;
    float loadDuration = 3f;
    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        nextLevelBtn.onClick.AddListener(() => ProceedToNextLevel());
        playAgainBtn.onClick.AddListener(() => PlayAgain());
        exitGameBtn.onClick.AddListener(() => ExitGame());
    }

    private void OnDisable()
    {
        nextLevelBtn.onClick.RemoveAllListeners();
        playAgainBtn.onClick.RemoveAllListeners();
        exitGameBtn.onClick.RemoveAllListeners();
    }

    void Start()
    {
        ShowLoadingPanel();
    }

    public void IncrementTurnScore()
    {
        turnScore++;
        turnsTxt.text = $"Turns: {turnScore}";
    }

    public void IncrementMatchScore()
    {
        matchScore++;
        matchTxt.text = $"Match: {matchScore}";
    }

    public void ShowLoadingPanel()
    {
        StartCoroutine(nameof(LoadingCoroutine));
    }

    IEnumerator LoadingCoroutine()
    {
        loadingPanel.SetActive(true);
        float timer = 0f;
        float progress = 0f;
        ClearOutScores();
        while (timer < loadDuration)
        {
            timer += Time.deltaTime;
            progress = Mathf.Clamp01(timer / loadDuration); // 0 to 1
            loadingImage.fillAmount = progress;
            yield return null;
        }
        loadingPanel.SetActive(false);
        loadingImage.fillAmount = 0f;
    }

    public void ClearOutScores()
    {
        turnScore = 0f;
        matchScore = 0f;
        turnsTxt.text = $"Turns: {turnScore}";
        matchTxt.text = $"Match: {matchScore}";       
    }

    public void EnableNextLevelButton(bool interactable)
    {
        nextLevelBtn.interactable = interactable;
    }

    void ProceedToNextLevel()
    {
        StartCoroutine(nameof(ProceedToNextLevelCoroutine));
    }

    IEnumerator ProceedToNextLevelCoroutine()
    {
        EnableNextLevelButton(false);
        ShowLoadingPanel();
        CardController.Instance.ClearOutCards();
        yield return new WaitForSeconds(1f);
        LevelManager.Instance.IncrementLevel();
    }

    public void ShowEndGamePanel()
    {
        endGamePanel.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
