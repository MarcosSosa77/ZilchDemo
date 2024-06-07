using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int totalCollectedCards;

    public int selectedLetters;

    public string word;

    public float time;

    private void Awake()
    {
        Instance = this;
    }

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
        if (time < UIManager.Instance.cpu.time)
        {
            totalCollectedCards += selectedLetters;
            UIManager.Instance.playerCards.text = totalCollectedCards.ToString();

            UIManager.Instance.message.text = UIManager.Instance.correctWord;
            UIManager.Instance.message.gameObject.SetActive(true);
        }
        else
        {
            char[] letters = UIManager.Instance.cpu.cpuWord.ToCharArray();
            if(UIManager.Instance.cpu.isCPUTrue)
            {
                UIManager.Instance.cpuTotalCards += letters.Length;
                UIManager.Instance.cpuCards.text = UIManager.Instance.cpuTotalCards.ToString();

                UIManager.Instance.message.text = UIManager.Instance.wrongWord;
                UIManager.Instance.message.gameObject.SetActive(true);
            }
            else
            {
                totalCollectedCards += selectedLetters;
                UIManager.Instance.playerCards.text = totalCollectedCards.ToString();
                UIManager.Instance.message.text = UIManager.Instance.correctWord;
                UIManager.Instance.message.gameObject.SetActive(true);
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

        UIManager.Instance.timer = 30;
        UIManager.Instance.isTimerRunning = false;

        StartCoroutine(NextRound(3));
    }

    public void NextRound()
    {
        StartCoroutine(NextRound(3));
    }

    public IEnumerator NextRound(float sec)
    {
        yield return new WaitForSeconds(sec);
        StartCoroutine(CardManager.instance.CardAddOnTable());
        UIManager.Instance.submitButton.interactable = true;
        selectedLetters = 0;
        for (int i = 0; i < UIManager.Instance.playerWord.Count; i++)
        {
            UIManager.Instance.playerWord[i].text = string.Empty;
        }

        UIManager.Instance.message.gameObject.SetActive(false);
    }
}
