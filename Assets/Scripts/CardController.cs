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

    Card firstSelectedCard;
    Card secondSelectedCard;
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


    // Start is called before the first frame update
    private void Start()
    {
        SetupLevel(2, 2); // Example: 2x2 grid = 4 cards = 2 pairs
    }

    private void SetupLevel(int row, int column)
    {
        int totalCards = row * column;
        int pairCount = totalCards / 2;

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = column;
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

        cardShuffleList = cardShuffleList.OrderBy(_ => UnityEngine.Random.value).ToList();
    }

    private void SpawnCards()
    {
        for (int i = 0; i < cardShuffleList.Count; i++)
        {
            GameObject cardGO = Instantiate(cardPrefab, grid.transform);
            Card card = cardGO.GetComponent<Card>();
            card.InitializeCard(cardShuffleList[i], i); // Send index + sprite
        }
    }

    public void SelectCard(Card selectedCard)
    {
        if (isChecking || selectedCard == firstSelectedCard || selectedCard == secondSelectedCard) return;
        if (firstSelectedCard == null)
        {
            firstSelectedCard = selectedCard;
            selectedCard.FlipCardSprite();
        }
        else
        {
            secondSelectedCard = selectedCard;
            selectedCard.FlipCardSprite();
            StartCoroutine(nameof(MatchCardsCheck));
        }
    }

    IEnumerator MatchCardsCheck()
    {
        isChecking = true;
        yield return new WaitForSeconds(checkDelay);
        if (firstSelectedCard.CardFrontSprite == secondSelectedCard.CardFrontSprite)
        {
            firstSelectedCard.SetMatched();
            secondSelectedCard.SetMatched();
        }
        else
        {
            firstSelectedCard.FlipCardSprite(false);
            secondSelectedCard.FlipCardSprite(false);
        }
        isChecking = false;
        firstSelectedCard = null;
        secondSelectedCard = null;
    }
}
