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

    public Player player;
    public CPU cPU;

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
        StartCoroutine(CardAddOnTable(3));
    }

    public void Zilch()
    {
        if(!UIManager.Instance.isTimerRunning)
            return;

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
        player.NextRound(3);
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
        player.totalCollectedCards += randomLetters.Count;
        UIManager.Instance.playerCards.text = player.totalCollectedCards.ToString();
    }

    public void ResetCards()
    {
        for (int i = 0; i < randomLetters.Count; i++)
        {
            deck.Add(randomLetters[i]);
        }

        randomLetters.Clear();
    }

    public IEnumerator CardAddOnTable(float sec)
    {
        Debug.Log("CardAddOnTable");

        if(deck.Count < 8)
        {
            if(player.totalCollectedCards > cPU.totalCollectedCards)
            {
                UIManager.Instance.winnerText.text = UIManager.Instance.winnerPlayer;
            }
            else
            {
                UIManager.Instance.winnerText.text = UIManager.Instance.losePlayer;
            }
            UIManager.Instance.resultScreen.SetActive(true);
        }
        else
        {
            UIManager.Instance.nextRound.SetActive(true);
            UIManager.Instance.nextRoundTimer.text = sec.ToString();
            while(sec > 0)
            {
                yield return new WaitForSeconds(1);
                sec--;
                UIManager.Instance.nextRoundTimer.text = sec.ToString();
            }
            UIManager.Instance.nextRound.SetActive(false);

            deck.Shuffle();
            UIManager.Instance.cards = new();
            for (int i = 0;i < UIManager.Instance.tableCards.Count; i++)
            {
                UIManager.Instance.cards.Add(UIManager.Instance.tableCards[i]);
            }

            for(int i = 0; i < UIManager.Instance.tableCards.Count; i++)
                UIManager.Instance.tableCards[i].gameObject.SetActive(false);

            for (int i = 0; i < UIManager.Instance.cards.Count; i++)
            {
                int random = Random.Range(0, deck.Count);
                UIManager.Instance.cards[i].cardLetter.text = deck[random];
                randomLetters.Add(deck[random]);
                UIManager.Instance.cards[i].cardBtn.interactable = true;
                UIManager.Instance.cards[i].gameObject.SetActive(true);
                deck.RemoveAt(random);
                UIManager.Instance.dealerCards.text = deck.Count.ToString();
                yield return new WaitForSeconds(0.25f);
            }
            UIManager.Instance.cards.Clear();
            UIManager.Instance.isTimerRunning = true;
        }
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
            UIManager.Instance.message.text = UIManager.Instance.wrongWord;
            UIManager.Instance.message.gameObject.SetActive(true);
        }
        else
        {
            if (wordChecker.dictionary.Contains(words.ToLower()))
                success.Invoke();
            else
            {
                Debug.Log("no possible word");
                UIManager.Instance.message.text = UIManager.Instance.wrongWord;
                UIManager.Instance.message.gameObject.SetActive(true);
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