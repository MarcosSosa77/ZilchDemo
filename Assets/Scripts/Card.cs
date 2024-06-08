using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Button cardBtn;
    public TMP_Text cardLetter;

    public void CardSelect()
    {
        if (UIManager.Instance.submitButton.interactable && UIManager.Instance.isTimerRunning)
        {
            UIManager.Instance.playerWord[CardManager.instance.player.selectedLetters].text = cardLetter.text;
            CardManager.instance.player.selectedLetters++;
            UIManager.Instance.cards.Add(this);
            cardBtn.interactable = false;
            GetComponent<Outline>().enabled = true;
        }
    }

    private void OnDisable()
    {
        GetComponent<Outline>().enabled = false;
    }
}