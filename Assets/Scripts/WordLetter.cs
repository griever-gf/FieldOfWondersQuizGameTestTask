using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordLetter : MonoBehaviour
{
    public Image imageHider;
    public Text letter;

    void Start()
    {
    }

    public void HideLetter()
    {
        imageHider.gameObject.SetActive(true);
    }

    public void ShowLetter()
    {
        imageHider.gameObject.SetActive(false);
    }

    public void SetLetter(string val)
    {
        letter.text = val;
    }

    public bool IsLetterHidden()
    {
        return imageHider.gameObject.activeSelf;
    }
}
