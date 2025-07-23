using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int CardId { get; private set; }
    [SerializeField] Button cardBtn;
    [SerializeField] Image cardImage;
    [SerializeField] Sprite cardBackSprite;
    Sprite cardFrontSprite;
    [SerializeField] bool isFlipped;

    private void OnEnable()
    {
        cardBtn.onClick.AddListener(() => FlipCard());
    }

    private void OnDisable()
    {
        cardBtn.onClick.RemoveAllListeners();
    }

    public void InitializeCard(Sprite card, int _id)
    {
        cardFrontSprite = card;
        cardImage.sprite = cardBackSprite;
        CardId = _id;
    }

    void FlipCard()
    {
        if (!isFlipped) FlipCardSprite(cardFrontSprite, isFlipped);
        else FlipCardSprite(cardBackSprite, isFlipped);
    }

    void FlipCardSprite(Sprite card1, bool _isFlipped)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        LeanTween.scale(gameObject, new Vector3(0f, rectTransform.localScale.y, rectTransform.localScale.z), 0.1f).setOnComplete(() =>
        {
            cardImage.sprite = card1;
            LeanTween.scale(gameObject, new Vector3(1f, rectTransform.localScale.y, rectTransform.localScale.z), 0.1f).setOnComplete(() =>
            {
                isFlipped = !_isFlipped;
            });
        });
    }
}
