using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    public WordChecker wordChecker;

    public List<DeckList> cardList;

    public List<string> deck;
    public List<string> randomLetters;

    private bool isValid;

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
        string word = string.Empty;
        for (int i = 0; i < randomLetters.Count; i++)
        {
            word += randomLetters[i];
        }
        wordChecker.IsWordPossible(word);
        if(wordChecker.words.Count == 0)
        {
            WordCheckingFalse();
        }
        else
            WordCheckingTrue();
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
    }

    public void WordCheckingTrue()
    {
        UIManager.Instance.cpuTotalCards += randomLetters.Count;
        UIManager.Instance.cpuCards.text = UIManager.Instance.cpuTotalCards.ToString();
    }

    public void WordCheckingFalse()
    {
        Player.Instance.totalCollectedCards += randomLetters.Count;
        UIManager.Instance.playerCards.text = Player.Instance.totalCollectedCards.ToString();
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
            UIManager.Instance.dealerCards.text = deck.Count.ToString();
            yield return new WaitForSeconds(0.5f);
        }
        UIManager.Instance.cards.Clear();
        UIManager.Instance.isTimerRunning = true;
    }

    public void WordCheck(System.Action success, string words)
    {
        isValid = false;
        if (words.Contains('?'))
        {
            List<string> expandedWords = wordChecker.ExpandWordWithWildcards(words);
            foreach (string word in expandedWords)
            {
                isValid = wordChecker.dictionary.Contains(word.ToLower());
                Debug.Log($"Is '{word}' valid? {isValid}");
                if (isValid)
                {
                    success.Invoke();
                    return;
                }
            }
            Debug.Log("no possible word");
        }
        else
        {
            if (wordChecker.dictionary.Contains(words.ToLower()))
                success.Invoke();
            else
            {
                Debug.Log("no possible word");
            }
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