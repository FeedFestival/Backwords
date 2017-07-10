using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    public int Index;
    public bool Predefined;
    public string Text;
    public RectTransform Rt;
    public int PlaceholderIndex;
    
    public void Init(int i, string c)
    {
        Index = i;
        Rt = GetComponent<RectTransform>();

        if (c == " ")
            Predefined = false;
        else
            Predefined = true;
        
        Text = c;
    }
}