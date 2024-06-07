using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public List<Card> tableCards;

    public List<TMP_Text> playerWord;

    public Button submitButton;
    public TMP_Text playerCards;
    public TMP_Text cpuCards;

    public GameObject minLetterWarning;

    [Tooltip("At runtime getting cards that need to change")]
    public List<Card> cards;

    private void Awake()
    {
        Instance = this;
        playerCards.text = "0";
        cpuCards.text = "0";
        for (int i = 0; i < playerWord.Count; i++)
        {
            playerWord[i].text = string.Empty;
        }
        cards = new();
        for (int i = 0;i < tableCards.Count; i++)
        {
            cards.Add(tableCards[i]);
        }
    }

    public IEnumerator MinLetterWaning()
    {
        minLetterWarning.SetActive(true);
        yield return new WaitForSeconds(3);
        minLetterWarning.SetActive(false);
    }

}
