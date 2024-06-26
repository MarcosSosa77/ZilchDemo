using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    public WordExist wordExist;
    public List<DeckList> cardList;

    public List<string> deck;
    public List<string> randomLetters;

    public int roundTimer, nextRoundTimer;
    public Player player;
    public CPU cPU;

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

        if(wordExist.validWords.Count == 0)
        {
            UIManager.Instance.isTimerRunning = false;

            player.totalCollectedCards += 8;
            UIManager.Instance.playerCards.text = player.totalCollectedCards.ToString();

            UIManager.Instance.message.text = UIManager.Instance.zilchRound;
            UIManager.Instance.message.gameObject.SetActive(true);

            Debug.Log(UIManager.Instance.zilchRound);

            randomLetters.Clear();

            player.NextRound(nextRoundTimer);
        }
        else if(cPU.isRoundLose)
        {
            UIManager.Instance.isTimerRunning = false;

            UIManager.Instance.message.text = UIManager.Instance.wrongWord;
            UIManager.Instance.message.gameObject.SetActive(true);

            Debug.Log(UIManager.Instance.wrongWord);

            ResetCards();

            player.NextRound(nextRoundTimer);
        }
        else if(!player.isRoundLose)
        {
            player.isRoundLose = true;

            UIManager.Instance.message.text = UIManager.Instance.zilchWrong;
            UIManager.Instance.message.gameObject.SetActive(true);

            Debug.Log(UIManager.Instance.zilchWrong);
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
        if(deck.Count < 8)
        {
            if(player.totalCollectedCards > cPU.totalCollectedCards)
            {
                UIManager.Instance.winnerText.text = UIManager.Instance.winnerPlayer;
                SoundManager.instance.PlaySound(SoundType.WIN_MATCH, 1f);
            }
            else
            {
                UIManager.Instance.winnerText.text = UIManager.Instance.losePlayer;
                SoundManager.instance.PlaySound(SoundType.LOSE_MATCH, 1f);
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
            UIManager.Instance.message.gameObject.SetActive(false);

            deck.Shuffle();
            SoundManager.instance.PlaySound(SoundType.SHUFFLE_AND_DELIVER, 1f);

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
                UIManager.Instance.cards[i].cardBtn.interactable = true;
                UIManager.Instance.cards[i].gameObject.SetActive(true);
                randomLetters.Add(deck[random]);
                deck.RemoveAt(random);
                UIManager.Instance.dealerCards.text = deck.Count.ToString();
                yield return new WaitForSeconds(0.2f);
            }
            wordExist.CheckWord();
            cPU.CPUPlay();
            UIManager.Instance.cards.Clear();
            UIManager.Instance.isTimerRunning = true;
            player.isRoundLose = false;
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