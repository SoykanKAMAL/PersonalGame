using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardDataSO cardData;
    public Sprite backSideSprite;
    public Image imageComponent;
    public Button buttonComponent;
    public Animator animatorComponent;

    public static Action<Card> onCardFlipped;

    private bool isFlipped = false;
    private bool isMatched = false;

    private void Start()
    {
        UpdateCardView();
        buttonComponent.onClick.AddListener(OnCardClicked);
    }

    public void Initialize(CardDataSO data)
    {
        cardData = data;
        UpdateCardView();
    }

    public void Flip()
    {
        isFlipped = !isFlipped;
        UpdateCardView();
    }

    public void UpdateCardView()
    {
        imageComponent.sprite = isFlipped || isMatched ? cardData.image : backSideSprite;
    }

    private void OnCardClicked()
    {
        if (!isFlipped && !isMatched)
        {
            Flip();
            animatorComponent.SetTrigger("Flip");
            onCardFlipped?.Invoke(this);
        }
    }

    public bool CheckMatch(Card otherCard)
    {
        return cardData.id == otherCard.cardData.id;
    }

    private void SetMatched(bool matched)
    {
        isMatched = matched;
        UpdateCardView();
    }
}