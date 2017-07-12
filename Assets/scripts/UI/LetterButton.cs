using System.Collections;
using System.Collections.Generic;
using Assets.scripts.utils;
using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour
{
    public int Index;
    public Text Text;
    public RectTransform Rt;
    public Vector3 PlaceholderPosition;
    public Button Button;

    public delegate void OnClickListener(int index, string letter);

    public bool AsAnswer;

    public void Init(int i, string correctLetter,
        OnClickListener onClickListener)
    {
        Index = i;
        Rt = GetComponent<RectTransform>();
        Button = GetComponent<Button>();

        Transform[] childList = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in childList)
        {
            if (child.name.Equals("(Text)"))
            {
                Text = child.gameObject.GetComponent<Text>();
            }
        }

        Text.text = correctLetter;
        
        Debug.Log(Rt.sizeDelta.x);
        //Text.fontSize = utils.GetLetterFontSize(Rt);

        Button.onClick.AddListener(() =>
        {
            if (AsAnswer)
            {
                Main.Instance().GameController.OnWordsClick();
            }
            else
            {
                onClickListener(Index, Text.text);
            }
        });
    }

    public void UpdateLetterScale(Vector2 newValue)
    {
        Rt.sizeDelta = newValue;
    }
}