using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour
{
    public int Index;

    public Text Text;

    public RectTransform Rt;

    public delegate void OnClickListener(int index, string letter);

    public void Init(int i, string correctLetter,
        OnClickListener onClickListener)
    {
        Index = i;
        Rt = GetComponent<RectTransform>();

        Transform[] childList = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in childList)
        {
            if (child.name.Equals("(Text)"))
            {
                Text = child.gameObject.GetComponent<Text>();
            }
        }

        Text.text = correctLetter;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            onClickListener(Index, Text.text);
        });
    }

    public void Show(bool val)
    {
        GetComponent<Image>().enabled = val;
        Text.enabled = val;
    }
}