using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    public int Index;
    public bool Predefined;
    public Text Text;
    public int HiddenLetterButtonIndex;

    public void Init(int i, string c)
    {
        Index = i;

        if (c == " ")
            Predefined = false;
        else
            Predefined = true;

        Transform[] childList = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in childList)
        {
            if (child.name.Equals("(Text)"))
            {
                Text = child.gameObject.GetComponent<Text>();
            }
        }

        Text.text= c;
    }
}