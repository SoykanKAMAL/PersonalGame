using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Deck : Singleton<Deck>
{
    public GameObject cardPrefab;
    public RectTransform cardsParent;

    [Range(1, 6)]
    public int rowCount = 4;
    [Range(1, 6)]
    public int columnCount = 4;

    private List<CardDataSO> cardDataList = new List<CardDataSO>();

    private List<Card> cards = new List<Card>();
    
    private GridLayoutGroup gridLayoutGroup => cardsParent.GetComponent<GridLayoutGroup>();

    private void OnEnable()
    {
        GameManager.onGameStart += CreateDeck;
        GameManager.onGameLoaded += CreateDeck;
    }

    private void OnDisable()
    {
        GameManager.onGameStart -= CreateDeck;
        GameManager.onGameLoaded -= CreateDeck;
    }

    public void FlipCardsWithIds(List<uint> cardIds)
    {
        foreach (Card card in cards)
        {
            if (cardIds.Contains(card.cardData.id))
            {
                card.SetMatched(true);
                card.FlipBack();
            }
        }
    }

    public void CreateDeck(uint seed)
    {
        cardDataList = Resources.LoadAll<CardDataSO>("Cards").ToList();

        foreach(Card card in cards)
        {
            Destroy(card.gameObject);
        }

        cards.Clear();

        // Check if odd number of cards
        if ((rowCount * columnCount) % 2 != 0)
        {
            Debug.LogWarning("Odd number of cards. Adding one more card to make it even.");
            columnCount++;
        }

        ReScaleDeck();

        // Shuffle the card data list based on the seed
        Random.InitState((int)seed);

        List<CardDataSO> selectedCardDataList = new List<CardDataSO>();

        for (int i = 0; i < (rowCount * columnCount) / 2; i++)
        {
            CardDataSO cardData = cardDataList[Random.Range(0, cardDataList.Count)];
            selectedCardDataList.Add(cardData);
            selectedCardDataList.Add(cardData);

            // Remove the card data from the list to avoid duplicates
            cardDataList.Remove(cardData);
        }

        // Shuffle the selected card data list
        selectedCardDataList = selectedCardDataList.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                Card card = Instantiate(cardPrefab, cardsParent).GetComponent<Card>();
                card.Initialize(selectedCardDataList[i * columnCount + j]);
                cards.Add(card);
            }
        }

        FlipCardsWithIds(GameManager.Instance.matchedCardIds);
    }

    public void Reset()
    {
        foreach (Card card in cards)
        {
            card.Initialize(card.cardData);
        }
    }

    private void ReScaleDeck()
    {
        var cellSize = Mathf.Min(cardsParent.rect.width / columnCount, cardsParent.rect.height / rowCount);
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayoutGroup.constraintCount = rowCount;
    }

    private void OnRectTransformDimensionsChange()
    {
        ReScaleDeck();
    }
}
