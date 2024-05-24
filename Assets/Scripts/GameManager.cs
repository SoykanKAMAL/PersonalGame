using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public uint seed;
    public float delayBeforeFlipBack = 1f;
    public List<Card> selectedCards = new List<Card>();
    public List<uint> matchedCardIds = new List<uint>();

    public static Action<uint> onGameStart;
    public static Action onGameEnd;
    public static Action onCardsMatched;
    public static Action onCardsMismatched;

    private void Start()
    {
        StartGame();
    }

    private void OnEnable()
    {
        Card.onCardClicked += OnCardSelected;
        onCardsMatched += CheckGameEnd;
    }

    private void OnDisable()
    {
        Card.onCardClicked -= OnCardSelected;
        onCardsMatched -= CheckGameEnd;
    }

    private void StartGame()
    {
        onGameStart?.Invoke(seed);
    }

    private void CheckGameEnd()
    {
        if (matchedCardIds.Count >= Deck.Instance.rowCount * Deck.Instance.columnCount * .5f)
        {
            onGameEnd?.Invoke();
        }
    }

    private void OnCardSelected(Card card)
    {
        selectedCards.Add(card);

        if (selectedCards.Count == 2)
        {
            CheckMatch();
        }
    }

    private void CheckMatch()
    {
        var card1 = selectedCards[0];
        var card2 = selectedCards[1];

        if (card1.CheckMatch(card2))
        {
            card1.SetMatched(true);
            card2.SetMatched(true);

            matchedCardIds.Add(card1.cardData.id);

            onCardsMatched?.Invoke();
        }
        else
        {
            StartCoroutine(FlipBack(card1, card2));

            onCardsMismatched?.Invoke();
        }

        selectedCards.Clear();
    }

    private IEnumerator FlipBack(Card card1, Card card2)
    {
        yield return new WaitForSeconds(delayBeforeFlipBack);

        card1.FlipBack();
        card2.FlipBack();
    }
}
