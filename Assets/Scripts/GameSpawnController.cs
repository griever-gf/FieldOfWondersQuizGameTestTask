using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using UnityEngine.UI;

public class GameSpawnController : MonoBehaviour
{
    public GameObject prefabWordLetter;
    public Transform pointSpawnWord;
    List<GameObject> wordLetters;

    public GameObject prefabSymbolButton;
    public Transform pointSpawnSymbolButtons;
    List<GameObject> symbolButtons;

    public Text labelTriesLeft;
    public Text labelScore;

    public Action<char> OnAnySymbolButtonPressed;

    public void SpawnWord(string word)
    {
        if (wordLetters != null)
            foreach (GameObject go in wordLetters)
                Destroy(go);

        wordLetters = new List<GameObject>();
        for (int i=0; i < word.Length; i++)
        {
            GameObject letter = Instantiate(prefabWordLetter, pointSpawnWord);
            Vector3[] corners = new Vector3[4];
            letter.GetComponent<RectTransform>().GetWorldCorners(corners);
            float w = Vector3.Distance(corners[0], corners[1]);
            letter.transform.position += new Vector3((w * 1.075f) * ((float)i + 0.5f - (float)word.Length/2f), 0);
            letter.GetComponent<WordLetter>().SetLetter(word[i].ToString());
            letter.GetComponent<WordLetter>().HideLetter();
            wordLetters.Add(letter);
        }
    }

    public void SpawnLetterButtons()
    {
        if (symbolButtons != null)
            foreach (GameObject go in symbolButtons)
                Destroy(go);

        List<char> symbols = new List<char>();
        for (char c = 'A'; c <= 'Z'; ++c)
            symbols.Add(c);
        symbols.Add('\'');

        symbolButtons = new List<GameObject>();
        for (int i = 0; i < symbols.Count; i++)
        {
            GameObject btn = Instantiate(prefabSymbolButton, pointSpawnSymbolButtons);
            Vector3[] corners = new Vector3[4];
            btn.GetComponent<RectTransform>().GetWorldCorners(corners);
            float w = Vector3.Distance(corners[0], corners[1]);
            float h = Vector3.Distance(corners[1], corners[2]);
            btn.transform.position += new Vector3((w * 1.1f) * ((float)(symbolButtons.Count % 9) - 4f), (h * 1.1f) * (-1 * symbolButtons.Count / 9));
            btn.GetComponent<SymbolButton>().SetSymbol(symbols[i]);
            btn.GetComponent<Button>().onClick.AddListener(() => SymbolButtonClicked(btn.GetComponent<Button>(), btn.GetComponent<SymbolButton>().GetSymbol()));
            symbolButtons.Add(btn);
        }
    }

    private void SymbolButtonClicked(Button sender, char c)
    {
        Destroy(sender.gameObject);
        OnAnySymbolButtonPressed.Invoke(c);
    }

    public void RefreshLabelTries(int val)
    {
        labelTriesLeft.text = "Осталось нажатий: " + val.ToString();
    }

    public void RefreshLabelScores(int val)
    {
        labelScore.text = "Заработано очков: " + val.ToString();
    }

    public void OpenWordLetters(bool[] states)
    {
        for (int i = 0; i < states.Length; i++)
        {
            WordLetter wl = wordLetters[i].GetComponent<WordLetter>();
            if (wl.IsLetterHidden() && states[i])
                wl.ShowLetter();
        }
    }

    public void OpenWordFull()
    {
        for (int i = 0; i < wordLetters.Count(); i++)
        {
            WordLetter wl = wordLetters[i].GetComponent<WordLetter>();
            if (wl.IsLetterHidden())
                wl.ShowLetter();
        }
    }
}
