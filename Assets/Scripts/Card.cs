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

    public static Action<Card> onCardClicked;
    public static Action<Card> onCardMatched;
    public static Action onCardHovered;
    public static Action onCardFlipped;

    private bool _isFlipped = false;
    private bool _isMatched = false;

    public void OnCardHovered()
    {
        onCardHovered?.Invoke();
    }

    private void OnEnable()
    {
        UpdateCardView();
        buttonComponent.onClick.AddListener(OnCardClicked);
    }

    private void OnDisable()
    {
        buttonComponent.onClick.RemoveListener(OnCardClicked);
    }

    public void Initialize(CardDataSO data)
    {
        cardData = data;
        UpdateCardView();
    }

    private void Flip()
    {
        _isFlipped = !_isFlipped;
        animatorComponent.SetTrigger("Flip");
        onCardFlipped?.Invoke();
    }

    public void FlipBack()
    {
        _isFlipped = !_isFlipped;
        animatorComponent.SetTrigger("FlipBack");
        onCardFlipped?.Invoke();
    }

    private void UpdateCardView()
    {
        imageComponent.sprite = _isFlipped || _isMatched ? cardData.image : backSideSprite;
    }

    private void OnCardClicked()
    {
        if (!_isFlipped && !_isMatched)
        {
            Flip();
        }
    }

    private void InvokeClickedEvent()
    {
        onCardClicked?.Invoke(this);
    }

    public bool CheckMatch(Card otherCard)
    {
        return cardData.id == otherCard.cardData.id;
    }

    public void SetMatched(bool matched)
    {
        _isMatched = matched;
        if(matched) 
        {
            animatorComponent.SetTrigger("Match");
            buttonComponent.interactable = false;
            onCardMatched?.Invoke(this);
        }
        UpdateCardView();
    }
}
