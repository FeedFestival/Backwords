using System.Collections.Generic;
using Assets.scripts.utils;
using UnityEngine;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour
{
    public bool DebugScript;

    private int _maxToDelete;

    public Dictionary<int, string> CorrectLetters;

    public Dictionary<int, Letter> WordLetters;

    public int LastAvailablePlace, FirstAvailablePlace;

    public int CurrentWordLetter = 0;

    public void Init()
    {
        CurrentWordLetter = 0;

        BuildWords(Main.Instance().GameController.PublicWord);
    }
    
    public void BuildWords(string word)
    {
        word = Main.Instance().GameController.Reverse(word);
        word = GenerateHidden(word);

        var wordLetters = new List<string>();

        int ci = 0;
        foreach (char c in word)
        {
            wordLetters.Add(c.ToString());
            ci++;
        }

        var rt = Main.Instance().scope["Words"].GetComponent<RectTransform>();
        var gridLayoutGroup = Main.Instance().scope["Words"].GetComponent<GridLayoutGroup>();

        var width = rt.sizeDelta.x;

        var wordLenght = word.Length;

        var cellSize = width / wordLenght;

        if (cellSize > 45)
            cellSize = 45;

        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);

        WordLetters = new Dictionary<int, Letter>();
        for (var i = 0; i < wordLetters.Count; i++)
        {
            GameObject letter = Instantiate(Resources.Load("Prefabs/Letter", typeof(GameObject))) as GameObject;
            if (letter != null)
            {
                letter.transform.SetParent(rt);
                letter.transform.localScale = new Vector3(1, 1, 1);

                var letterObj = letter.GetComponent<Letter>();
                letterObj.Init(i, wordLetters[i]);

                WordLetters.Add(i, letterObj);

                // for predefined build a keyboard letter.

                if (letterObj.Predefined)
                {
                    GameObject aLetter = Instantiate(Resources.Load("Prefabs/LetterButton", typeof(GameObject))) as GameObject;
                    if (aLetter != null)
                    {
                        aLetter.transform.SetParent(letter.GetComponent<RectTransform>());
                        aLetter.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize, cellSize);

                        aLetter.transform.localScale = new Vector3(1, 1, 1);

                        var answerLetter = aLetter.GetComponent<LetterButton>();
                        answerLetter.Init(i, wordLetters[i]
                            , null);

                        // disabled the button
                        answerLetter.Button.interactable = false;
                    }
                }
            }
        }

        CalculateLastAvailablePlace();
        CalculateFirstAvailablePlace();
    }
    
    public string GenerateHidden(string word)
    {
        CorrectLetters = new Dictionary<int, string>();

        float percent = 70.0f;
        float length = (float)word.Length;
        var result = (length / 100.0f) * percent;
        
        _maxToDelete = (int)result;
        var currentDeleted = 0;

        while (currentDeleted < _maxToDelete)
        {
            for (int i = 0; i < length; i++)
            {
                if (i == 0 || i == (int)(length - 1))
                {
                    word = RemoveLetter(i, word, false, ref currentDeleted);
                    continue;
                }

                var penny = utils.GetPennyToss();

                if ((i - 2) >= 0 && word[i - 1] != '#' && word[i - 2] != '#')
                {
                    penny = 0;
                }
                if (currentDeleted >= _maxToDelete)
                    penny = 0;

                if (penny == 0)
                {
                    word = RemoveLetter(i, word, false, ref currentDeleted);
                }
            }
        }

        for (var i = 0; i < length; i++)
        {
            if ((i - 2) >= 0 && word[i - 1] != '#' && word[i - 2] != '#')
            {
                word = RemoveLetter(i, word, true, ref currentDeleted);
            }
        }

        return word;
    }

    public bool CalculateAvailableLetterIndex(bool forward)
    {
        if (forward)
        {
            if (CurrentWordLetter == LastAvailablePlace)
                return true;

            // Skip predefined as answered letters and calculate our new current letter space index.
            if (WordLetters[CurrentWordLetter].Predefined)
            {
                if (DebugScript)
                    Debug.Log(WordLetters[CurrentWordLetter].Index + ", t: " +
                              WordLetters[CurrentWordLetter].Text + ", predef: " +
                              WordLetters[CurrentWordLetter].Predefined);

                for (var i = CurrentWordLetter; i < WordLetters.Count; i++)
                {
                    if (WordLetters[i].Predefined == false)
                        break;
                    CurrentWordLetter++;
                }
            }
        }
        else
        {
            if (CurrentWordLetter == FirstAvailablePlace)
                return true;

            if (WordLetters[CurrentWordLetter - 1].Predefined)
            {
                if (DebugScript)
                    Debug.Log(WordLetters[CurrentWordLetter].Index + ", t: " +
                              WordLetters[CurrentWordLetter].Text + ", predef: " +
                              WordLetters[CurrentWordLetter].Predefined);

                for (var i = CurrentWordLetter - 1; i >= 0; i--)
                {
                    if (WordLetters[i].Predefined == false)
                        break;
                    CurrentWordLetter--;
                }
            }
            CurrentWordLetter--;
        }
        return false;
    }

    private string RemoveLetter(int i, string word, bool forceDelete, ref int cD)
    {
        if (CorrectLetters.ContainsKey(i))
            return word;

        if (cD >= _maxToDelete && forceDelete == false)
            return word;

        CorrectLetters.Add(i, word[i].ToString());
        word = word.Remove(i, 1).Insert(i, "#");
        cD++;

        if (DebugScript)
            Debug.Log("removed at : " + i + " , remaining to remove: " + (_maxToDelete - cD));

        return word;
    }
    
    public void CalculateLastAvailablePlace()
    {
        for (var i = WordLetters.Count - 1; i >= 0; i--)
        {
            if (WordLetters[i].Predefined == false)
            {
                LastAvailablePlace = i + 1;
                
                break;
            }
        }
    }

    public void CalculateFirstAvailablePlace()
    {
        for (var i = 0; i < WordLetters.Count; i++)
        {
            if (WordLetters[i].Predefined == false)
            {
                FirstAvailablePlace = i;
                break;
            }
        }
    }
}
