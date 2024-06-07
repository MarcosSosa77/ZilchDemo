using System.Collections.Generic;
using UnityEngine;

public class CPU : MonoBehaviour
{
    [Tooltip("how much percentage it sholud do before player")]
    public int percentage;

    public float time;
    public string cpuWord;

    public bool isCPUTrue;

    public void CPUPlay()
    {
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

    void CPUWinRound()
    {
        string word = string.Empty;
        for(int i =0; i< CardManager.instance.randomLetters.Count; i++)
        {
            word += CardManager.instance.randomLetters[i];
        }
        CardManager.instance.wordChecker.IsWordPossible(word);
        time = UIManager.Instance.timer;
        cpuWord = CardManager.instance.wordChecker.words[0];
        isCPUTrue = true;
    }

    void CPULoseRound()
    {
        List<string> words = new();
        for (int i = 0; i < CardManager.instance.randomLetters.Count; i++)
        {
            words[i] = CardManager.instance.randomLetters[i];
        }
        words.Shuffle();
        string word = string.Empty;
        for (int i = 0; i < CardManager.instance.randomLetters.Count; i++)
        {
            word += CardManager.instance.randomLetters[i];
        }
        cpuWord = word;
        isCPUTrue = false;
        CardManager.instance.WordCheck(CPUWordFind, cpuWord);
    }

    void CPUWordFind()
    {
        isCPUTrue = true;
        time = UIManager.Instance.timer;
    }
}
