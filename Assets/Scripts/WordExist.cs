using System.Collections.Generic;
using UnityEngine;

public class WordExist : MonoBehaviour
{
    private HashSet<string> dictionary;
    public List<string> validWords;
    void Start()
    {
        LoadDictionary();
    }

    public void CheckWord()
    {
        string word = string.Empty;
        for(int i = 0; i< CardManager.instance.randomLetters.Count; i++)
        {
            word += CardManager.instance.randomLetters[i];
        }
        validWords = GetValidWords(word);
        if (validWords.Count > 0)
        {
            Debug.Log("The letters can form the " + validWords.Count + " words.");
            // foreach (var words in validWords)
            // {
            //     Debug.Log(words);
            // }
        }
        else
        {
            Debug.Log("The letters cannot form any word in the dictionary.");
        }
    }

    void LoadDictionary()
    {
        dictionary = new HashSet<string>();
        TextAsset wordFile = Resources.Load<TextAsset>("dictionary");
        string[] words = wordFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words)
        {
            if (word.Length >= 4 && word.Length <= 8)
            {
                dictionary.Add(word.ToLower());
            }
        }
    }

    public bool CanFormWord(string letters)
    {
        var combinations = GetCombinations(letters.ToLower());
        foreach (var comb in combinations)
        {
            if (dictionary.Contains(comb))
            {
                return true;
            }
        }
        return false;
    }

    public List<string> GetValidWords(string letters)
    {
        var validWords = new List<string>();
        var combinations = GetCombinations(letters.ToLower());
        foreach (var comb in combinations)
        {
            if (dictionary.Contains(comb))
            {
                validWords.Add(comb);
            }
        }
        return validWords;
    }

    IEnumerable<string> GetCombinations(string letters)
    {
        var combinations = new HashSet<string>();
        for (int i = 4; i <= 8; i++)
        {
            GetCombinationsRecursive(letters, "", i, combinations);
        }
        return combinations;
    }

    void GetCombinationsRecursive(string letters, string prefix, int k, HashSet<string> combinations)
    {
        if (k == 0)
        {
            combinations.Add(prefix);
            return;
        }

        for (int i = 0; i < letters.Length; i++)
        {
            GetCombinationsRecursive(letters.Remove(i, 1), prefix + letters[i], k - 1, combinations);
        }
    }
}