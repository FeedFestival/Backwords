﻿using System.Collections;
using System.Collections.Generic;
using Assets.scripts.utils;
using UnityEngine;
using UnityEngine.UI;

public class AnswersController : MonoBehaviour
{
    public Dictionary<int, LetterButton> AllLetters;

    public GameObject LetterContainer;

    private const int MaxAdditionalLetters = 4;
    private const int MinAdditionalLetters = 3;

    public void Init()
    {
        LetterContainer = Main.Instance().scope["LetterContainer"];

        BuildAnswers();
    }

    private void BuildAnswers()
    {
        int totalLetters;

        var gridLayoutGroup = LetterContainer.GetComponent<GridLayoutGroup>();
        var rt = LetterContainer.GetComponent<RectTransform>();

        var width = rt.sizeDelta.x;

        if (Main.Instance().GameController.QuestionController.CorrectLetters.Count < 5)
        {
            totalLetters = Main.Instance().GameController.QuestionController.CorrectLetters.Count + 1;

            var additionalLetters = 0;
            for (var t = 0; t < Main.Instance().GameController.QuestionController.CorrectLetters.Count; t++)
            {
                var totLetters = totalLetters + utils.GetPennyToss();
                if (totLetters != totalLetters)
                {
                    additionalLetters++;
                    totalLetters = totLetters;
                }

                if (MaxAdditionalLetters == additionalLetters)
                    break;
            }
            if (additionalLetters < MinAdditionalLetters)
                totalLetters++;

            gridLayoutGroup.constraintCount = 8;
        }
        else
        {
            var tot = (float)Main.Instance().GameController.QuestionController.CorrectLetters.Count / 2;
            totalLetters = (int)tot;

            var additionalLetters = 0;
            for (var t = 0; t < totalLetters; t++)
            {
                var totLetters = totalLetters + utils.GetPennyToss();
                if (totLetters != totalLetters)
                {
                    additionalLetters++;
                    totalLetters = totLetters;
                }

                if (MaxAdditionalLetters == additionalLetters)
                    break;
            }
            if (additionalLetters < MinAdditionalLetters)
                totalLetters++;

            //
            gridLayoutGroup.constraintCount = (totalLetters / 2) + 1;
        }

        var cellSize = width / totalLetters;
        if (cellSize > 90)
            cellSize = 90;
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        //

        var alphabet = "abcdefghijklmnopqrstuvwxyz";

        var addedLettersCount = totalLetters - Main.Instance().GameController.QuestionController.CorrectLetters.Count;

        List<string> possibleLetters = new List<string>();

        int l = 0;
        while (l < addedLettersCount)
        {
            var letter = alphabet.Substring(Random.Range(0, alphabet.Length), 1);
            if (LetterExists(letter.ToUpper()) == false)
            {
                possibleLetters.Add(letter.ToUpper());
                l++;
            }
        }

        foreach (KeyValuePair<int, string> correctLetter in Main.Instance().GameController.QuestionController.CorrectLetters)
        {
            var position = Random.Range(0, possibleLetters.Count);
            possibleLetters.Insert(position, correctLetter.Value);
        }

        //
        AllLetters = new Dictionary<int, LetterButton>();
        for (var i = 0; i < possibleLetters.Count; i++)
        {
            GameObject placeholder = Instantiate(Resources.Load("Prefabs/placeholder", typeof(GameObject))) as GameObject;

            if (placeholder != null)
            {
                placeholder.transform.SetParent(rt);
                placeholder.transform.localScale = new Vector3(1, 1, 1);
            }

            GameObject letter = Instantiate(Resources.Load("Prefabs/LetterButton", typeof(GameObject))) as GameObject;
            if (letter != null)
            {
                letter.transform.SetParent(placeholder.GetComponent<RectTransform>());
                letter.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize, cellSize);

                letter.transform.localScale = new Vector3(1, 1, 1);

                var letB = letter.GetComponent<LetterButton>();
                letB.Init(i, possibleLetters[i]
                    , Main.Instance().GameController.OnClickLetter);

                AllLetters.Add(i, letB);
            }
        }
    }

    private bool LetterExists(string newLetter)
    {
        foreach (KeyValuePair<int, string> letter in Main.Instance().GameController.QuestionController.CorrectLetters)
        {
            if (newLetter == letter.Value)
                return true;
        }
        return false;
    }
}
