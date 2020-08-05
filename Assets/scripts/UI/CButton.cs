using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CButton : MonoBehaviour
{
    public int Index;

    public Text Text;

    public delegate void OnButtonClicked(int index);

    public void Init(int index, string text, OnButtonClicked onButtonClicked)
    {
        Index = index;

        Transform[] childList = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in childList)
        {
            if (child.name.Equals("(Text)"))
            {
                Text = child.gameObject.GetComponent<Text>();
            }
        }
        Text.text = text;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            onButtonClicked(Index);
        });
    }
}
