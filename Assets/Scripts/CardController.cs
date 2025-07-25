using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public static CardController Instance { get; private set; }

    [SerializeField] GameObject cardPrefab;
    [SerializeField] GridLayoutGroup grid;
    [SerializeField] SpriteAtlas cardAtlas;

    [SerializeField] List<Sprite> allCardSpritesList = new();
    [SerializeField] List<Sprite> cardShuffleList = new();
    List<Card> cardLists = new();
    List<Card> flippedCardsLists = new();

    bool isChecking;
    private float checkDelay = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetupLevel(CardLevelData level)
    {
        int totalCards = level.rows * level.columns;
        int pairCount = totalCards / 2;

        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = level.rows;
        OnLoadCardSprites();
        GenerateCardPairs(pairCount);
        SpawnCards();
    }


    private void OnLoadCardSprites()
    {
        allCardSpritesList.Clear();

        Sprite[] spriteArray = new Sprite[cardAtlas.spriteCount];
        cardAtlas.GetSprites(spriteArray);
        allCardSpritesList.AddRange(spriteArray);
    }

    private void GenerateCardPairs(int pairCount)
    {
        List<int> availableIndices = Enumerable.Range(0, allCardSpritesList.Count).ToList();
        availableIndices = availableIndices.OrderBy(_ => Random.value).ToList();

        List<int> selectedIds = availableIndices.Take(pairCount).ToList();

        cardShuffleList.Clear();
        foreach (int id in selectedIds)
        {
            Sprite sprite = allCardSpritesList[id];
            cardShuffleList.Add(sprite);
            cardShuffleList.Add(sprite);
        }

        cardShuffleList = cardShuffleList.OrderBy(_ => Random.value).ToList();
    }

    private void SpawnCards()
    {
        for (int i = 0; i < cardShuffleList.Count; i++)
        {
            GameObject cardGO = Instantiate(cardPrefab, grid.transform);
            Card card = cardGO.GetComponent<Card>();
            card.InitializeCard(cardShuffleList[i], i);
            cardLists.Add(card);
        }
        StartCoroutine(nameof(FlipCardsToBackOnStart));
    }

    public void SelectCard(Card selectedCard)
    {
        selectedCard.FlipCardSprite();
        if (!flippedCardsLists.Contains(selectedCard))
        {
            flippedCardsLists.Add(selectedCard);
        }

        if (!isChecking && flippedCardsLists.Count >= 2)
        {
            StartCoroutine(nameof(MatchCardsCheck));
        }
    }

    IEnumerator MatchCardsCheck()
    {
        isChecking = true;
        UIManager.Instance.IncrementTurnScore();
        yield return new WaitForSeconds(checkDelay);

        while (flippedCardsLists.Count >= 2)
        {
            Card firstSelectedCard = flippedCardsLists[0];
            Card secondSelectedCard = flippedCardsLists[1];
            if (firstSelectedCard.CardFrontSprite == secondSelectedCard.CardFrontSprite)
            {
                firstSelectedCard.SetMatched();
                secondSelectedCard.SetMatched();
                UIManager.Instance.IncrementMatchScore();
                AudioManager.Instance.PlayWinLoseSound(true);
                StartCoroutine(nameof(CheckLevelOver));               
            }
            else
            {
                firstSelectedCard.FlipCardSprite(false);
                secondSelectedCard.FlipCardSprite(false);
                AudioManager.Instance.PlayWinLoseSound(false);
            }
            flippedCardsLists.Remove(firstSelectedCard);
            flippedCardsLists.Remove(secondSelectedCard);
        }

        isChecking = false;
    }

    public void ClearOutCards()
    {
        foreach (Card card in cardLists)
        {
            Destroy(card.gameObject);
        }
        cardLists.Clear();
    }

    IEnumerator FlipCardsToBackOnStart()
    {
        yield return new WaitForSeconds(1f);
        foreach (Card card in cardLists)
        {
            card.FlipCardSprite(false);
        }
    }

    IEnumerator CheckLevelOver()
    {
        foreach (Card card in cardLists)
        {
            if (!card.IsMatched) yield break;
        }
        yield return new WaitForSeconds(1f);
        UIManager.Instance.EnableNextLevelButton(true);
        //StartCoroutine(nameof(ChangeLevel));
    }
}
