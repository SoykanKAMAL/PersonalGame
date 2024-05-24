using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public GameObject cardPrefab;
    public RectTransform cardsParent;

    [Range(1, 10)]
    public int rows = 4;
    [Range(1, 10)]
    public int columns = 4;

    public List<CardDataSO> cardDataList = new List<CardDataSO>();

    private List<Card> cards = new List<Card>();

    public void CreateDeck(uint seed)
    {
        foreach(Card card in cards)
        {
            Destroy(card.gameObject);
        }

        cards.Clear();

        Random.InitState((int)seed);

        List<CardDataSO> selectedCardDataList = new List<CardDataSO>();

        for (int i = 0; i < (rows * columns) / 2; i++)
        {
            CardDataSO cardData = cardDataList[Random.Range(0, cardDataList.Count)];
            selectedCardDataList.Add(cardData);
            selectedCardDataList.Add(cardData);

            // Remove the card data from the list to avoid duplicates
            cardDataList.Remove(cardData);
        }

        // Shuffle the selected card data list
        selectedCardDataList = selectedCardDataList.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Card card = Instantiate(cardPrefab, cardsParent).GetComponent<Card>();
                card.Initialize(selectedCardDataList[i * columns + j]);
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
}
