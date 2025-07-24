using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnsTxt;
    [SerializeField] TextMeshProUGUI matchTxt;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Image loadingImage;
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
        ;
    }
}
