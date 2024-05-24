using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private uint _seed;
    public float delayBeforeFlipBack = 1f;
    public List<Card> selectedCards = new List<Card>();
    public List<uint> matchedCardIds = new List<uint>();

    public static Action<uint> onGameStart;
    public static Action onGameEnd;
    public static Action onCardsMatched;
    public static Action onCardsMismatched;
    public static Action onGameSaved;
    public static Action<uint> onGameLoaded;

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

    [ContextMenu("Save Game Test")]
    private void SaveGameTest()
    {
        onGameSaved?.Invoke();
    }

    [ContextMenu("Load Game Test")]
    private void LoadGameTest()
    {
        onGameLoaded?.Invoke(_seed);
    }

    public void SaveGame()
    {
        PlayerPrefs.SetString("_seed", _seed.ToString());
        // Convert matchedCardIds to string to save in PlayerPrefs
        var matchedCardIdsString = "";
        for (int i = 0; i < matchedCardIds.Count; i++)
        {
            matchedCardIdsString += matchedCardIds[i].ToString();
            if (i < matchedCardIds.Count - 1)
            {
                matchedCardIdsString += ",";
            }
        }
        PlayerPrefs.SetString("MatchedCardIds", matchedCardIdsString);

        onGameSaved?.Invoke();
    }

    public void LoadGame()
    {
        _seed = uint.Parse(PlayerPrefs.GetString("_seed"));
        // Convert matchedCardIds from string to List<uint>
        var matchedCardIdsString = PlayerPrefs.GetString("MatchedCardIds");
        var matchedCardIdsArray = matchedCardIdsString.Split(',');
        matchedCardIds.Clear();
        for(int i = 0; i < matchedCardIdsArray.Length; i++)
        {
            matchedCardIds.Add(uint.Parse(matchedCardIdsArray[i]));
        }
        
        onGameLoaded?.Invoke(_seed);
    }

    public void QuitGame()
    {
        Application.Quit();
    }  

    public void StartGame()
    {
        // Randomize _seed to shuffle cards
        _seed = (uint)UnityEngine.Random.Range(0, int.MaxValue);
        matchedCardIds.Clear();
        onGameStart?.Invoke(_seed);
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
