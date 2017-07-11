using System;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts.utils;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool DebugScript;

    public QuestionController QuestionController;
    public AnswersController AnswersController;

    public string PublicWord;

    private Level _currentLevel;

    private float _animationTime = 0.5f;

    private Coroutine _endAnimation;
    private bool _placingAnimationRunning;

    public void Init()
    {
        QuestionController = Main.Instance().scope["QuestionContainer"].GetComponent<QuestionController>();
        AnswersController = Main.Instance().scope["AnswerContainer"].GetComponent<AnswersController>();

        gameObject.SetActive(true);
    }

    // delegates
    public delegate void OnLeveCompleted(int index);

    private OnLeveCompleted _onLeveCompleted;


    //

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

        var img = Main.Instance().scope["QuestionPicture"].GetComponent<Image>();
        var sprite = Resources.Load<Sprite>("Pictures/" + PublicWord);
        img.sprite = sprite;

        // statr the game view
        QuestionController.Init();
        AnswersController.Init();
    }

    public void OnWordsClick()  // Remove the Letter from the question.
    {
        if (DebugScript)
            Debug.Log(QuestionController.WordLetters[QuestionController.CurrentWordLetter - 1].Predefined);

        var isEndPoint = QuestionController.CalculateAvailableLetterIndex(forward: false);
        if (isEndPoint)
            return;

        StartCoroutine(
            AnimteLetterUnplacing(AnswersController.AllLetters[QuestionController.WordLetters[QuestionController.CurrentWordLetter].PlaceholderIndex])
            );

        QuestionController.WordLetters[QuestionController.CurrentWordLetter].Text = " ";
    }

    public void OnClickLetter(int index, string letter)
    {
        if (DebugScript)
            Debug.Log(QuestionController.CurrentWordLetter + " - " + QuestionController.LastAvailablePlace);

        var isEndPoint = QuestionController.CalculateAvailableLetterIndex(forward: true);
        if (isEndPoint)
            return;

        StartCoroutine(
            AnimateLetterPlacing(AnswersController.AllLetters[index], QuestionController.WordLetters[QuestionController.CurrentWordLetter])
            );
        
        // Add the letter to the current index in the word.
        QuestionController.WordLetters[QuestionController.CurrentWordLetter].Text = AnswersController.AllLetters[index].Text.text;

        QuestionController.CurrentWordLetter++;


        if (_placingAnimationRunning)
        {
            StopCoroutine(_endAnimation);
            _endAnimation = StartCoroutine(OnClickLetterEndAnimation());
        }
        else
            _endAnimation = StartCoroutine(OnClickLetterEndAnimation());
    }

    // only check for correct word when all animations have stopped.
    IEnumerator OnClickLetterEndAnimation()
    {
        _placingAnimationRunning = true;

        yield return new WaitForSeconds(_animationTime);

        _placingAnimationRunning = false;
        if (QuestionController.CurrentWordLetter == QuestionController.LastAvailablePlace)
            CheckIfWordIsCorrect();
    }

    void CheckIfWordIsCorrect()
    {
        var compiledWord = string.Empty;

        foreach (KeyValuePair<int, Letter> letter in QuestionController.WordLetters)
        {
            compiledWord += letter.Value.Text;
        }

        if (Reverse(compiledWord.ToUpper()) == PublicWord.ToUpper())
        {
            Debug.Log(compiledWord.ToUpper() + " = " + PublicWord.ToUpper() + " este Correct !");
            _onLeveCompleted(_currentLevel.Id);
        }
        else
        {
            Debug.Log(compiledWord.ToUpper() + " != " + PublicWord.ToUpper() + "Incorrect :(");

            var lastIndex = QuestionController.LastAvailablePlace - 1;
            StartCoroutine(
                ReturnLastIncorrectLetter(AnswersController.AllLetters[QuestionController.WordLetters[lastIndex].PlaceholderIndex], QuestionController.WordLetters[lastIndex - 1])
            );
        }
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

    public Vector2 PlaceholderSize;
    public Vector2 QuestionLetterSize;

    IEnumerator AnimateLetterPlacing(LetterButton answerLetter, Letter questionLetterSpace)
    {
        // this should be moved in a function where the posible answers are created.
        ////

        answerLetter.PlaceholderPosition = answerLetter.Rt.position;
        PlaceholderSize = answerLetter.Rt.sizeDelta;

        //var layE = answerLetter.gameObject.AddComponent<LayoutElement>();
        //layE.ignoreLayout = true;

        //answerLetter.transform.SetParent(Main.Instance().scope["GameView"].GetComponent<RectTransform>());
        //answerLetter.transform.localScale = new Vector3(1, 1, 1);

        ////
        // move it !!


        // disabled the button
        answerLetter.AsAnswer = true;

        // Animate moving towards the available space

        questionLetterSpace.PlaceholderIndex = answerLetter.Index;
        QuestionLetterSize = new Vector2(
            utils.GetPercent(questionLetterSpace.Rt.sizeDelta.x, 95),
            utils.GetPercent(questionLetterSpace.Rt.sizeDelta.y, 91));

        iTween.MoveTo(answerLetter.gameObject, iTween.Hash(
            "position", questionLetterSpace.Rt.position,
            "time", _animationTime));

        iTween.ValueTo(answerLetter.gameObject, iTween.Hash(
            "from", answerLetter.Rt.sizeDelta,
            "to", QuestionLetterSize,
            "time", _animationTime,
            "onupdatetarget", answerLetter.gameObject,
            "onupdate", "UpdateLetterScale"
        ));

        yield return new WaitForSeconds(_animationTime);
    }

    IEnumerator AnimteLetterUnplacing(LetterButton answerLetter)
    {
        iTween.MoveTo(answerLetter.gameObject, iTween.Hash(
            "position", answerLetter.PlaceholderPosition,
            "time", _animationTime));

        iTween.ValueTo(answerLetter.gameObject, iTween.Hash(
            "from", answerLetter.Rt.sizeDelta,
            "to", PlaceholderSize,
            "time", _animationTime,
            "onupdatetarget", answerLetter.gameObject,
            "onupdate", "UpdateLetterScale"
        ));

        yield return new WaitForSeconds(_animationTime);

        // enable the button
        answerLetter.AsAnswer = false;
    }

    private float _nonoSplitDuration = 0.075f;

    IEnumerator ReturnLastIncorrectLetter(LetterButton answerLetter, Letter letterSpace)
    {
        // stop all attempt at placing. 

        // play the no-no animation.
        var width = letterSpace.Rt.sizeDelta.x;
        
        var tempPos = transform.InverseTransformPoint(answerLetter.Rt.position);
        tempPos = new Vector3(tempPos.x - (width / 6), tempPos.y, tempPos.z);

        var leftPos = transform.TransformPoint(tempPos);

        tempPos = new Vector3(tempPos.x + (width / 3), tempPos.y, tempPos.z);

        var rightPos = transform.TransformPoint(tempPos);

        var originalPos =  answerLetter.Rt.position;

        //Debug.Log(letterSpace.Rt.position);

        iTween.MoveTo(answerLetter.gameObject, iTween.Hash(
            "position", leftPos,
            "time", _nonoSplitDuration / 2,
            "easetype", "easeInCirc"));

        yield return new WaitForSeconds(_nonoSplitDuration);

        iTween.MoveTo(answerLetter.gameObject, iTween.Hash(
            "position", rightPos,
            "time", _nonoSplitDuration,
            "easetype", "easeInCirc"));

        yield return new WaitForSeconds(_nonoSplitDuration);

        iTween.MoveTo(answerLetter.gameObject, iTween.Hash(
            "position", leftPos,
            "time", _nonoSplitDuration,
            "easetype", "easeInCirc"));

        yield return new WaitForSeconds(_nonoSplitDuration);

        iTween.MoveTo(answerLetter.gameObject, iTween.Hash(
            "position", rightPos,
            "time", _nonoSplitDuration,
            "easetype", "easeInCirc"));

        yield return new WaitForSeconds(_nonoSplitDuration);

        iTween.MoveTo(answerLetter.gameObject, iTween.Hash(
            "position", leftPos,
            "time", _nonoSplitDuration,
            "easetype", "easeInCirc"));

        yield return new WaitForSeconds(_nonoSplitDuration);

        iTween.MoveTo(answerLetter.gameObject, iTween.Hash(
            "position", originalPos,
            "time", _nonoSplitDuration /2,
            "easetype", "easeInCirc"));

        yield return new WaitForSeconds(_nonoSplitDuration);

        // leave back all attempt at placing to true.

        OnWordsClick();
    }
}