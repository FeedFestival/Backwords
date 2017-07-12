using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CButton : MonoBehaviour
{
    public int Index;

    public Text Text;

    public Sprite NormalSprite;

    public delegate void OnButtonClicked(int index);

    public void Init(int index, string text, OnButtonClicked onButtonClicked, bool locked)
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

        if (locked)
        {
            var but = GetComponent<Button>();
            but.interactable = false;
            var image = GetComponent<Image>();
            image.sprite = Resources.Load<Sprite>("UI/level-locked");
            Text.enabled = false;
        }

        GetComponent<Button>().onClick.AddListener(() =>
        {
            onButtonClicked(Index);
        });
    }
}
