using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolButton : MonoBehaviour
{
    public void SetSymbol(char val)
    {
        GetComponentInChildren<Text>().text = val.ToString();
    }

    public char GetSymbol()
    {
        return (GetComponentInChildren<Text>().text[0]);
    }
}
