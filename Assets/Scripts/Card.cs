using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int CardId { get; private set; }
    public Sprite CardFrontSprite { get; private set; }
    [SerializeField] Button cardBtn;
    [SerializeField] Image cardImage;
    [SerializeField] Sprite cardBackSprite;
    [SerializeField] bool isFlipped;
    bool isMatched = false;
    public bool IsMatched { get { return isMatched; } }
    private void OnEnable()
    {
        cardBtn.onClick.AddListener(() => SelectCardToFlip());
    }

    private void OnDisable()
    {
        cardBtn.onClick.RemoveAllListeners();
    }

    public void InitializeCard(Sprite card, int _id)
    {
        CardFrontSprite = card;
        cardImage.sprite = card;
        CardId = _id;
    }

    void SelectCardToFlip()
    {
        // if the card is not flipped then make it flipped
        if (isFlipped) return;
        CardController.Instance.SelectCard(this);
    }

    public void FlipCardSprite(bool isFront = true)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (isFront) AudioManager.Instance.PlayCardFlipSound();
        LeanTween.scale(gameObject, new Vector3(0f, rectTransform.localScale.y, rectTransform.localScale.z), 0.1f).setOnComplete(() =>
        {
            cardImage.sprite = isFront ? CardFrontSprite : cardBackSprite;
            LeanTween.scale(gameObject, new Vector3(1f, rectTransform.localScale.y, rectTransform.localScale.z), 0.1f).setOnComplete(() =>
            {
                isFlipped = isFront;
            });
        });
    }

    public void SetMatched()
    {
        isMatched = true;
        cardBtn.interactable = false;
    }
}
