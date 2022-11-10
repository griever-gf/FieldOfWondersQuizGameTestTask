using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGroupsSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject groupPlay;
    [SerializeField]
    private GameObject groupWordGuessed;
    [SerializeField]
    private GameObject groupGameFinished;

    void HideAll()
    {
        groupPlay.SetActive(false);
        groupWordGuessed.SetActive(false);
        groupGameFinished.SetActive(false);
    }

    public void ShowUIGroupPlay()
    {
        HideAll();
        groupPlay.SetActive(true);
    }

    public void ShowUIGroupWordGuessed()
    {
        HideAll();
        groupWordGuessed.SetActive(true);
    }

    public void ShowUIGroupGameFinished()
    {
        HideAll();
        groupGameFinished.SetActive(true);
    }
}
