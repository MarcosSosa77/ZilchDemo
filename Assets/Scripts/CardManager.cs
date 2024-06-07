using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    public List<DeckList> cardList;

    public List<string> deck;
    public List<string> randomLetters;

    const string baseURL = "https://api.dictionaryapi.dev/api/v2/entries/en/";

    private void Awake()
    {
        instance = this;
        deck = new();
        for (int i = 0; i < cardList.Count; i++)
        {
            for (int j = 0; j < cardList[i].repeatTimes; j++)
                deck.Add(cardList[i].letter);
        }
    }

    void Start()
    {
        deck.Shuffle();
        StartCoroutine(CardAddOnTable());
    }

    public void Zilch()
    {
        Player.Instance.totalCollectedCards += randomLetters.Count;
        UIManager.Instance.playerCards.text = Player.Instance.totalCollectedCards.ToString();
        StartCoroutine(Player.Instance.NextRound(0.2f));
        randomLetters = new();
        UIManager.Instance.cards = new();
        foreach (var item in UIManager.Instance.tableCards)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < UIManager.Instance.tableCards.Count; i++)
        {
            UIManager.Instance.cards.Add(UIManager.Instance.tableCards[i]);
        }
        StartCoroutine(CardAddOnTable());
    }

    public IEnumerator CardAddOnTable()
    {
        for (int i = 0; i < UIManager.Instance.cards.Count; i++)
        {
            int random = Random.Range(0, deck.Count);
            UIManager.Instance.cards[i].cardLetter.text = deck[random];
            randomLetters.Add(deck[random]);
            UIManager.Instance.cards[i].gameObject.SetActive(true);
            UIManager.Instance.cards[i].cardBtn.interactable = true;
            deck.RemoveAt(random);
            yield return new WaitForSeconds(0.5f);
        }
        UIManager.Instance.cards.Clear();
    }


    public IEnumerator WordChecker(System.Action success)
    {
        using UnityWebRequest request = UnityWebRequest.Get(baseURL + Player.Instance.word);
        //request.SetRequestHeader("Content-Type", "application/json");
        //request.SetRequestHeader("Accept", "application/json");
        yield return request.SendWebRequest();

        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log(request.downloadHandler.text);

            if (request.downloadHandler.text.Contains("words"))
                Debug.Log("true");
            success.Invoke();

        }
        else
        {
            Debug.LogError(request.downloadHandler.text);
        }
    }

}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}

[System.Serializable]
public struct DeckList
{
    public string letter;
    public int repeatTimes;
}