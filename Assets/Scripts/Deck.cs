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

    [Range(1, 10)]
    public int rowCount = 4;
    [Range(1, 10)]
    public int columnCount = 4;

    private List<CardDataSO> cardDataList = new List<CardDataSO>();

    private List<Card> cards = new List<Card>();
    
    private GridLayoutGroup gridLayoutGroup => cardsParent.GetComponent<GridLayoutGroup>();

    private void Awake()
    {
        cardDataList = Resources.LoadAll<CardDataSO>("Cards").ToList();
    }

    private void OnEnable()
    {
        GameManager.onGameStart += CreateDeck;
    }

    private void OnDisable()
    {
        GameManager.onGameStart -= CreateDeck;
    }

    public void CreateDeck(uint seed)
    {
        foreach(Card card in cards)
        {
            Destroy(card.gameObject);
        }

        cards.Clear();

        ReScaleDeck();

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
