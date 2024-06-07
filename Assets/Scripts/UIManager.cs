using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public CPU cpu;
    public List<Card> tableCards;

    public List<TMP_Text> playerWord;

    public Button submitButton;
    public TMP_Text playerCards;
    public TMP_Text cpuCards;
    public TMP_Text dealerCards;

    public GameObject minLetterWarning;

    [Tooltip("At runtime getting cards that need to change")]
    public List<Card> cards;

    public int cpuTotalCards;

    public TMP_Text remainingTime;

    public float timer;
    public bool isTimerRunning;

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
        cpuTotalCards = 0;
    }

    private void Start()
    {
        dealerCards.text = CardManager.instance.deck.Count.ToString();
    }

    private void Update()
    {
        if(isTimerRunning)
        {
            timer -= Time.deltaTime;
            int min = (int)(timer / 60);
            int sec = (int)(timer % 60);
            remainingTime.text = string.Format("{0:00}:{1:00}", min, sec);

            if(timer <= 0)
            {
                isTimerRunning = false;
            }
        }
    }

    public IEnumerator MinLetterWaning()
    {
        minLetterWarning.SetActive(true);
        yield return new WaitForSeconds(3);
        minLetterWarning.SetActive(false);
    }
}