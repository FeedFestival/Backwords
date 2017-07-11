using System.Collections;
using System.Collections.Generic;
using Assets.scripts.utils;
using UnityEngine;
using UnityEngine.UI;

public class AnswersController : MonoBehaviour
{
    public Dictionary<int, LetterButton> AllLetters;

    public GameObject LetterContainer;

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

        if (Main.Instance().GameController.QuestionController.CorrectLetters.Count < 7)
        {
            // just to have it a little bit more random
            totalLetters = Main.Instance().GameController.QuestionController.CorrectLetters.Count * 3;
            totalLetters += utils.GetPennyToss();
            totalLetters += utils.GetPennyToss();
            gridLayoutGroup.constraintCount = 8;
        }
        else
        {
            var tot = (float)Main.Instance().GameController.QuestionController.CorrectLetters.Count * 1.5f;
            totalLetters = (int)tot;
            totalLetters += utils.GetPennyToss();
            totalLetters += utils.GetPennyToss();
            //
            gridLayoutGroup.constraintCount = (totalLetters / 2) + 1;
        }

        var cellSize = width / totalLetters;
        if (cellSize > 70)
            cellSize = 70;
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
