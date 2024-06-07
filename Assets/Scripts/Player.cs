using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int totalCollectedCards;

    public int selectedLetters;

    public string word;

    private void Awake()
    {
        Instance = this;
    }

    public void Submit()
    {
        word = string.Empty;
        if (selectedLetters >= 4)
        {
            for (int i = 0; i < selectedLetters; i++)
            {
                word += UIManager.Instance.playerWord[i].text;
            }
            Debug.Log(word);
            UIManager.Instance.submitButton.interactable = false;
            StartCoroutine(CardManager.instance.WordChecker(TrueWord));
        }
        else
        {
            StartCoroutine(UIManager.Instance.MinLetterWaning());
        }
    }

    public void TrueWord()
    {
        totalCollectedCards += selectedLetters;
        UIManager.Instance.playerCards.text = totalCollectedCards.ToString();
        System.Collections.Generic.List<string> stringList = new();
        for(int i =0; i < selectedLetters; i++)
        {
            stringList.Add(UIManager.Instance.playerWord[i].text);
        }
        foreach (string item in stringList)
        {
            CardManager.instance.randomLetters.Remove(item);
        }

        foreach (var card in UIManager.Instance.cards)
        {
            card.transform.SetAsLastSibling();
            card.gameObject.SetActive(false);
        }
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
    }
}
