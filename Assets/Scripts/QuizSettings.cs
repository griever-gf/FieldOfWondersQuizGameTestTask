using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuizSettings")]
public class QuizSettings : ScriptableObject
{
    [SerializeField]
    private int minimalWordLength = 3;

    [SerializeField]
    private int playerTries = 20;

    public int GetMinWordLen()
    {
        return minimalWordLength;
    }

    public int GetPlayerTries()
    {
        return playerTries;
    }
}
