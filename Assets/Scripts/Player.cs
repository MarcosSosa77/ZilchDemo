using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int totalCollectedCards;

    public int selectedLetters;

    public string word;

    public float time;

    public void Submit()
    {
        if(!UIManager.Instance.isTimerRunning)
            return;

        word = string.Empty;
        if (selectedLetters >= 4)
        {
            for (int i = 0; i < selectedLetters; i++)
            {
                word += UIManager.Instance.playerWord[i].text;
            }
            UIManager.Instance.submitButton.interactable = false;
            CardManager.instance.WordCheck(TrueWord, word);
            time = UIManager.Instance.timer;
        }
        else
        {
            StartCoroutine(UIManager.Instance.MinLetterWaning());
        }
    }

    public void TrueWord()
    {
        if (time < CardManager.instance.cPU.time)
        {
            totalCollectedCards += selectedLetters;
            UIManager.Instance.playerCards.text = totalCollectedCards.ToString();

            UIManager.Instance.message.text = UIManager.Instance.correctWord;
            UIManager.Instance.message.gameObject.SetActive(true);
            Debug.Log(UIManager.Instance.correctWord);
        }
        else
        {
            char[] letters = CardManager.instance.cPU.cpuWord.ToCharArray();
            if(CardManager.instance.cPU.isCPUTrue)
            {
                UIManager.Instance.cpuTotalCards += letters.Length;
                UIManager.Instance.cpuCards.text = UIManager.Instance.cpuTotalCards.ToString();

                UIManager.Instance.message.text = UIManager.Instance.wrongWord;
                UIManager.Instance.message.gameObject.SetActive(true);
                Debug.Log(UIManager.Instance.wrongWord);
            }
            else
            {
                totalCollectedCards += selectedLetters;
                UIManager.Instance.playerCards.text = totalCollectedCards.ToString();
                UIManager.Instance.message.text = UIManager.Instance.correctWord;
                UIManager.Instance.message.gameObject.SetActive(true);
                Debug.Log(UIManager.Instance.correctWord);
            }
        }

        System.Collections.Generic.List<string> stringList = new();
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

        NextRound(3);
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
