using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WordChecker : MonoBehaviour
{
    public HashSet<string> dictionary;
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

    public List<string> words = new();

    void Start()
    {
        LoadDictionary();
    }

    public void IsWordPossible(string letters)
    {
        // Example usage
        int minLength = 4;
        int maxLength = 8;
        words = new();
        List<string> validWords = GetValidWords(letters, minLength, maxLength);
        Debug.Log("validWords - " + validWords.Count);
        foreach (string word in validWords)
        {
            Debug.Log(word);
            words.Add(word);
        }
    }

    public void LoadDictionary()
    {
        dictionary = new HashSet<string>();
        TextAsset dictionaryTextAsset = Resources.Load<TextAsset>("dictionary");

        if (dictionaryTextAsset != null)
        {
            using StringReader reader = new(dictionaryTextAsset.text);
            string word;
            while ((word = reader.ReadLine()) != null)
            {
                if (word.Length >= 4 && word.Length <= 8)
                {
                    dictionary.Add(word.ToLower());
                }
            }
            Debug.Log("Dictionary contains " + dictionary.Count + " words.");
        }
        else
        {
            Debug.LogError("Dictionary file not found!");
        }
    }

    public List<string> GetValidWords(string letters, int minLength, int maxLength)
    {
        List<string> validWords = new();
        List<string> expandedLetters = ExpandWordWithWildcards(letters);

        foreach (string expanded in expandedLetters)
        {
            char[] charArray = expanded.ToCharArray();
            List<string> permutations = GetPermutations(charArray, minLength, maxLength);

            foreach (string word in permutations)
            {
                if (dictionary.Contains(word))
                {
                    validWords.Add(word);
                }
            }
        }

        return validWords;
    }

    public List<string> ExpandWordWithWildcards(string word)
    {
        List<string> results = new() { word };

        while (results.Exists(w => w.Contains("?")))
        {
            List<string> newResults = new();
            foreach (string result in results)
            {
                List<string> expandedWords = ExpandWordWithOneWildcard(result);
                newResults.AddRange(expandedWords);
            }
            results = newResults;
        }

        return results;
    }

    public List<string> ExpandWordWithOneWildcard(string word)
    {
        List<string> results = new() { word };

        while (results.Exists(w => w.Contains("?")))
        {
            List<string> newResults = new();
            foreach (string result in results)
            {
                int index = result.IndexOf('?');
                if (index >= 0)
                {
                    foreach (char c in Alphabet)
                    {
                        newResults.Add(result[..index] + c + result[(index + 1)..]);
                    }
                }
                else
                {
                    newResults.Add(result);
                }
            }
            results = newResults;
        }

        return results;
    }

    private List<string> GetPermutations(char[] letters, int minLength, int maxLength)
    {
        List<string> permutations = new();

        for (int length = minLength; length <= maxLength; length++)
        {
            GetPermutations(letters, "", length, permutations);
        }

        return permutations;
    }

    private void GetPermutations(char[] letters, string current, int maxLength, List<string> permutations)
    {
        if (current.Length == maxLength)
        {
            permutations.Add(current);
            return;
        }

        for (int i = 0; i < letters.Length; i++)
        {
            char[] remainingLetters = new char[letters.Length - 1];
            int index = 0;

            for (int j = 0; j < letters.Length; j++)
            {
                if (i != j)
                {
                    remainingLetters[index++] = letters[j];
                }
            }

            GetPermutations(remainingLetters, current + letters[i], maxLength, permutations);
        }
    }
}