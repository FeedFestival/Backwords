using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool DebugScript;

    public QuestionController QuestionController;
    public AnswersController AnswersController;

    public string PublicWord;

    public int CurrentWordLetter = 0;

    private Level _currentLevel;

    public void Init()
    {
        QuestionController = Main.Instance().scope["QuestionContainer"].GetComponent<QuestionController>();
        AnswersController = Main.Instance().scope["AnswerContainer"].GetComponent<AnswersController>();

        gameObject.SetActive(true);
    }

    // delegates
    public delegate void OnLeveCompleted(int index);

    private OnLeveCompleted _onLeveCompleted;

    // for test we just send the level.
    public void StartGame(Level level, OnLeveCompleted onLeveCompleted)
    {
        _onLeveCompleted = onLeveCompleted;
        _currentLevel = level;

        // get from factory the level and word.

        // reset current state
        foreach (Transform child in Main.Instance().scope["Words"].transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in Main.Instance().scope["LetterContainer"].transform)
        {
            Destroy(child.gameObject);
        }

        PublicWord = level.Word;
        CurrentWordLetter = 0;

        var img = Main.Instance().scope["QuestionPicture"].GetComponent<Image>();
        var sprite = Resources.Load<Sprite>("Pictures/" + PublicWord);
        img.sprite = sprite;

        // statr the game view
        QuestionController.Init();
        AnswersController.Init();
    }

    public void OnWordsClick()  // move to GameLogic
    {
        Debug.Log(QuestionController.WordLetters[CurrentWordLetter - 1].Predefined);
        if (QuestionController.WordLetters[CurrentWordLetter - 1].Predefined)
        {
            if (DebugScript)
                Debug.Log(QuestionController.WordLetters[CurrentWordLetter].Index + ", t: " +
                          QuestionController.WordLetters[CurrentWordLetter].Text.text + ", predef: " +
                          QuestionController.WordLetters[CurrentWordLetter].Predefined);

            for (var i = CurrentWordLetter - 1; i >= 0; i--)
            {
                if (QuestionController.WordLetters[i].Predefined == false)
                    break;
                CurrentWordLetter--;
            }
        }

        CurrentWordLetter--;

        var index = CurrentWordLetter;
        QuestionController.WordLetters[index].Text.text = " ";

        AnswersController.AllLetters[QuestionController.WordLetters[index].HiddenLetterButtonIndex].Show(true);
    }

    public void OnClickLetter(int index, string letter)
    {
        Debug.Log(CurrentWordLetter + " - " + QuestionController.LastAvailablePlace);
        
        // Destroy the letter from the Letters list or hide using the index.
        AnswersController.AllLetters[index].Show(false);

        // Skip predefined as answered letters.
        if (QuestionController.WordLetters[CurrentWordLetter].Predefined)
        {
            if (DebugScript)
                Debug.Log(QuestionController.WordLetters[CurrentWordLetter].Index + ", t: " + QuestionController.WordLetters[CurrentWordLetter].Text.text + ", predef: " + QuestionController.WordLetters[CurrentWordLetter].Predefined);

            for (var i = CurrentWordLetter; i < QuestionController.WordLetters.Count; i++)
            {
                if (QuestionController.WordLetters[i].Predefined == false)
                    break;
                CurrentWordLetter++;
            }
        }

        // Add the letter to the current index in the word.
        QuestionController.WordLetters[CurrentWordLetter].Text.text = letter;
        QuestionController.WordLetters[CurrentWordLetter].HiddenLetterButtonIndex = index;

        CurrentWordLetter++;

        if (CurrentWordLetter == QuestionController.LastAvailablePlace)
        {
            Debug.Log("Check if game can end...");

            CheckIfWordIsCorrect();
            return;
        }
    }

    void CheckIfWordIsCorrect()
    {
        var compiledWord = string.Empty;

        foreach (KeyValuePair<int, Letter> letter in QuestionController.WordLetters)
        {
            compiledWord += letter.Value.Text.text;
        }

        if (Reverse(compiledWord.ToUpper()) == PublicWord.ToUpper())
        {
            Debug.Log(compiledWord.ToUpper() + " = " + PublicWord.ToUpper() + " este Correct !");
            _onLeveCompleted(_currentLevel.Id);
        }
        else
            Debug.Log(compiledWord.ToUpper() + " != " + PublicWord.ToUpper() + "Incorrect :(");
    }

    public string Reverse(string text)
    {
        text = text.ToUpper();
        char[] cArray = text.ToCharArray();
        string reverse = String.Empty;
        for (int i = cArray.Length - 1; i > -1; i--)
        {
            reverse += cArray[i];
        }
        return reverse;
    }
}