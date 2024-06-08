using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU : MonoBehaviour
{
    [Header("How much percentage it should win from 100%")]
    public int percentage;
    public float time;
    public string cpuWord;
    public int totalCollectedCards;
    public bool isCPUTrue;

    public void CPUPlay()
    {
        int random = Random.Range(0, 100);
        // if(random < percentage)
        // {
        //     CPUWinRound();
        // }
        // else
        // {
        //     CPULoseRound();
        // }
        CPUWinRound();
    }

    void CPUWinRound()
    {
        string word = string.Empty;
        for(int i = 0; i < CardManager.instance.randomLetters.Count; i++)
        {
            word += CardManager.instance.randomLetters[i];
        }
        time = Random.Range(0, 30);
        if(CardManager.instance.wordExist.validWords.Count > 0)
        {
            isCPUTrue = true;
            cpuWord = CardManager.instance.wordExist.validWords[Random.Range(0, CardManager.instance.wordExist.validWords.Count - 1)];
        } 

        StartCoroutine(WinRound(time));  
    }

    IEnumerator WinRound(float sec)
    {
        yield return new WaitForSeconds(sec);

        
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
        cpuWord = word;
        isCPUTrue = false;
        //CardManager.instance.WordCheck(CPUWordFind, cpuWord);
    }

    void CPUWordFind()
    {
        isCPUTrue = true;
        time = UIManager.Instance.timer;
    }
}