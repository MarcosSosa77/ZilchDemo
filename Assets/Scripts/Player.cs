using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int totalCollectedCards;
    public int selectedLetters;
    public string word;
    public float time;
    public bool isRoundLose;

    public void Submit()
    {
        if(!UIManager.Instance.isTimerRunning || isRoundLose) 
            return;

        word = string.Empty;
        if (selectedLetters >= 4)
        {
            for (int i = 0; i < selectedLetters; i++)
            {
                word += UIManager.Instance.playerWord[i].text;
            }
            UIManager.Instance.submitButton.interactable = false;

            time = UIManager.Instance.timer;
            
            if(CardManager.instance.wordExist.validWords.Contains(word.ToLower()))
            {
                TrueWord();
            }
            else
            {
                UIManager.Instance.message.text = UIManager.Instance.wrongWord;
                UIManager.Instance.message.gameObject.SetActive(true);

                Debug.Log(UIManager.Instance.wrongWord);

                if(CardManager.instance.cPU.isRoundLose)
                {
                    UIManager.Instance.isTimerRunning = false;

                    CardManager.instance.ResetCards();

                    foreach (var card in UIManager.Instance.cards)
                    {
                        card.transform.SetAsLastSibling();
                        card.gameObject.SetActive(false);
                    }

                    UIManager.Instance.timer = CardManager.instance.roundTimer;;

                    NextRound(CardManager.instance.nextRoundTimer);
                }
                else
                {
                    isRoundLose = true;
                }
            }
        }
        else
        {
            StartCoroutine(UIManager.Instance.MinLetterWaning());
        }
    }

    public void TrueWord()
    {
        CardManager.instance.cPU.StopCPU();

        totalCollectedCards += selectedLetters;
        UIManager.Instance.playerCards.text = totalCollectedCards.ToString();

        UIManager.Instance.message.text = UIManager.Instance.correctWord;
        UIManager.Instance.message.gameObject.SetActive(true);
        Debug.Log(UIManager.Instance.correctWord);

        List<string> stringList = new();
        for (int i = 0; i < selectedLetters; i++)
        {
            stringList.Add(UIManager.Instance.playerWord[i].text);
        }
        foreach (string item in stringList)
        {
            CardManager.instance.randomLetters.Remove(item);
        }

        CardManager.instance.ResetCards();

        foreach (var card in UIManager.Instance.cards)
        {
            card.transform.SetAsLastSibling();
            card.gameObject.SetActive(false);
        }

        UIManager.Instance.timer = CardManager.instance.roundTimer;;
        UIManager.Instance.isTimerRunning = false;

        NextRound(CardManager.instance.nextRoundTimer);
    }

    public void NextRound(float sec)
    {
        StartCoroutine(CardManager.instance.CardAddOnTable(sec));
        UIManager.Instance.submitButton.interactable = true;
        selectedLetters = 0;
        for (int i = 0; i < UIManager.Instance.playerWord.Count; i++)
        {
            UIManager.Instance.playerWord[i].text = string.Empty;
        }
    }
}