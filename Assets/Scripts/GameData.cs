using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using System.IO;

public class GameData : MonoBehaviour
{
    [SerializeField]
    private QuizSettings quizSettings;

    List<string> currentListOfWords;
    List<string> initialListOfWords;
    int currentWordIndex;

    int currentScores;
    int currentTries;
    int defaultTries;
    bool[] openedLetters;

    void Awake()
    {
        FillWordData();
    }

    bool FillWordData()
    {
        try
        {
            initialListOfWords = new List<string>();
            using (StreamReader sr = File.OpenText(Application.streamingAssetsPath + "/alice30.txt"))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    foreach (Match match in Regex.Matches(s, @"[a-zA-Z']+"))
                    {
                        if (match.Success && match.Groups.Count > 0)
                        {
                            string word = match.Groups[0].Value.ToUpper();
                            if (word.Length >= quizSettings.GetMinWordLen())
                                if (initialListOfWords.IndexOf(word) == -1)
                                    initialListOfWords.Add(word);
                        }
                    }
                }
            }
            Debug.Log("Total words: " + initialListOfWords.Count.ToString());
            return true;
        }
        catch (Exception Ex)
        {
            Debug.LogError(Ex.Message);
            return false;
        }
    }

    public void ResetWords()
    {
        currentListOfWords = new List<string>(initialListOfWords);
        Debug.Log("Resetting list of words. Current number of words is " + currentListOfWords.Count.ToString());
        currentWordIndex = -1;
    }

    public string SelectRandomWord()
    {
        Debug.Log("Selecting random word of " + currentListOfWords.Count.ToString());
        currentWordIndex = UnityEngine.Random.Range(0, currentListOfWords.Count);
        openedLetters = new bool[currentListOfWords[currentWordIndex].Length];
        return currentListOfWords[currentWordIndex];
    }

    public bool IsAnyWordsLeft()
    {
        //return false; //for debug
        return (currentListOfWords.Count > 0);
    }

    public void RemoveCurrentWord()
    {
        currentListOfWords.Remove(currentListOfWords[currentWordIndex]);
    }

    public void AddScore(int val)
    {
        currentScores += val;
    }

    public void ResetScore()
    {
        currentScores = 0;
    }

    public void SetDefaultTriesCount()
    {
        defaultTries = quizSettings.GetPlayerTries();
    }

    public void ResetTries()
    {
        currentTries = defaultTries;
    }

    public void DescreaseTries()
    {
        currentTries--;
    }

    public bool IsAnyTriesLeft()
    {
        return (currentTries > 0);
    }

    public int GetCurrentTries()
    {
        return currentTries;
    }

    public int GetCurrentScores()
    {
        return currentScores;
    }

    public void CheckCurrentWordForSymbol(Char c)
    {
        Debug.Log("Checking word " + currentListOfWords[currentWordIndex] + " for symbol " + c.ToString());
        for (int i = 0; i < currentListOfWords[currentWordIndex].Length; i++)
            if (currentListOfWords[currentWordIndex][i] == c)
                openedLetters[i] = true;
    }

    public bool IsCurrentWordOpened()
    {
        for (int i = 0; i < openedLetters.Length; i++)
            if (!openedLetters[i])
                return false;
        return true;
    }

    public bool[] GetOpenedLettersStates()
    {
        return openedLetters;
    }
}
