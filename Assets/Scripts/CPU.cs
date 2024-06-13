using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CPU : MonoBehaviour
{
    [Header("How much percentage it should win from 100%")]
    public int percentage;
    public float time;
    public string cpuWord;
    public int totalCollectedCards;
    public bool isCPUTrue;
    public bool isRoundLose;

    public void CPUPlay()
    {
        cpuWord = string.Empty;
        time = 0;
        isCPUTrue = false;
        isRoundLose = false;
        UIManager.Instance.cpuWordTxt.text = string.Empty;
        int random = Random.Range(0, 100);
        if(random < percentage)
        {
            CPUWinRound();
        }
        else
        {
            CPULoseRound();
        }
    }

    public void StopCPU()
    {
        StopAllCoroutines();
    }

    void CPUWinRound()
    {
        string word = string.Empty;
        for(int i = 0; i < CardManager.instance.randomLetters.Count; i++)
        {
            word += CardManager.instance.randomLetters[i];
        }
        time = Random.Range(5, CardManager.instance.roundTimer);
        if(CardManager.instance.wordExist.validWords.Count > 0)
        {
            isCPUTrue = true;
            cpuWord = CardManager.instance.wordExist.validWords[Random.Range(0, CardManager.instance.wordExist.validWords.Count - 1)];
        } 

        Debug.Log("CPU will win round in " + time + "s with " + cpuWord + " word!");
        //SoundManager.instance.PlaySound(SoundType.LOSE_MATCH);

        StartCoroutine(WinRound(time));  
    }

    IEnumerator WinRound(float sec)
    {
        float totalWaitTimer = CardManager.instance.roundTimer - sec;
        
        while(UIManager.Instance.timer > totalWaitTimer)
        {
            yield return null;
        }

        UIManager.Instance.cpuWordTxt.text = cpuWord;
        UIManager.Instance.isTimerRunning = false;

        yield return new WaitForSeconds(1);

        string[] lettersArray = cpuWord.ToCharArray().Select(c => c.ToString()).ToArray();

        foreach (string item in lettersArray)
        {
            CardManager.instance.randomLetters.Remove(item.ToUpper());
        }

        UIManager.Instance.cpuTotalCards += cpuWord.Length;
        UIManager.Instance.cpuCards.text = UIManager.Instance.cpuTotalCards.ToString();

        UIManager.Instance.message.text = UIManager.Instance.cpuCorrectWord;
        UIManager.Instance.message.gameObject.SetActive(true);
        Debug.Log(UIManager.Instance.cpuCorrectWord);
        SoundManager.instance.PlaySound(SoundType.LOSE_ROUND, 1f);

        UIManager.Instance.timer = CardManager.instance.roundTimer;

        CardManager.instance.ResetCards();
        CardManager.instance.player.NextRound(CardManager.instance.nextRoundTimer);
    }

    void CPULoseRound()
    {
        List<string> words = new();
        for (int i = 0; i < CardManager.instance.randomLetters.Count; i++)
        {
            words.Add(CardManager.instance.randomLetters[i]);
        }
        words.Shuffle();
        string word = string.Empty;
        for (int i = 0; i < CardManager.instance.randomLetters.Count; i++)
        {
            word += CardManager.instance.randomLetters[i];
        }
        cpuWord = word[..Random.Range(4, 8)];

        isCPUTrue = false;

        time = Random.Range(0, CardManager.instance.roundTimer);

        Debug.Log("CPU will lose round in " + time + "s with " + cpuWord + " word!");

        StartCoroutine(LoseRound(time));
    }

    IEnumerator LoseRound(float sec)
    {
        float totalWaitTimer = CardManager.instance.roundTimer - sec;
        
        while(UIManager.Instance.timer > totalWaitTimer)
        {
            yield return null;
        }

        UIManager.Instance.cpuWordTxt.text = cpuWord;

        if(CardManager.instance.player.isRoundLose)
        {
            UIManager.Instance.isTimerRunning = false;
            UIManager.Instance.message.text = UIManager.Instance.cpuWrongWordPlayerWrongWord;
            UIManager.Instance.message.gameObject.SetActive(true);
            Debug.Log(UIManager.Instance.cpuWrongWordPlayerWrongWord);
            SoundManager.instance.PlaySound(SoundType.WIN_ROUND, 1f);
        }
        else
        {
            UIManager.Instance.message.text = UIManager.Instance.cpuWrongWord;
            UIManager.Instance.message.gameObject.SetActive(true);
            Debug.Log(UIManager.Instance.cpuWrongWord);
        }

        isRoundLose = true;

        if(CardManager.instance.player.isRoundLose)
        {
            UIManager.Instance.timer = CardManager.instance.roundTimer;

            CardManager.instance.ResetCards();
            CardManager.instance.player.NextRound(CardManager.instance.nextRoundTimer);
        }
    }
}