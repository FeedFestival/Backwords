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

    public void Init()
    {
        QuestionController = Main.Instance().scope["QuestionContainer"].GetComponent<QuestionController>();
        AnswersController = Main.Instance().scope["AnswerContainer"].GetComponent<AnswersController>();

        gameObject.SetActive(true);
    }

    // delegates
    public delegate void OnLeveCompleted(int index);

    private OnLeveCompleted _onLeveCompleted;

    private delegate void OnClickLetterCallback();


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

        /*
         anim - s
         */

        QuestionController.WordLetters[QuestionController.CurrentWordLetter].Text.text = " ";
        AnswersController.AllLetters[QuestionController.WordLetters[QuestionController.CurrentWordLetter].HiddenLetterButtonIndex].Show(true);

        /*
         anim - end
         */

    }

    public void OnClickLetter(int index, string letter)
    {
        if (DebugScript)
            Debug.Log(QuestionController.CurrentWordLetter + " - " + QuestionController.LastAvailablePlace);

        var isEndPoint = QuestionController.CalculateAvailableLetterIndex(forward: true);
        if (isEndPoint)
            return;
        
        StartCoroutine(AnimateLetterPlacing(AnswersController.AllLetters[index], QuestionController.WordLetters[QuestionController.CurrentWordLetter], index,
            OnClickLetterEndAnimation));
    }

    private void OnClickLetterEndAnimation()
    {
        QuestionController.CurrentWordLetter++;
        if (QuestionController.CurrentWordLetter == QuestionController.LastAvailablePlace)
        {
            Debug.Log("Check if game can end...");

            CheckIfWordIsCorrect();
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

    private RectTransform _currentAnimatedRt;

    IEnumerator AnimateLetterPlacing(LetterButton answerLetter, Letter questionLetterSpace, int index,
        OnClickLetterCallback onClickLetterEndAnimation)
    {
        float animationTime = 0.5f;

        var globalParent = Main.Instance().scope["GameView"].GetComponent<RectTransform>();

        answerLetter.Show(false);

        var animLetter = GetAnimationLetter(answerLetter.Text.text);

        _currentAnimatedRt = animLetter.GetComponent<RectTransform>();
        _currentAnimatedRt.position = answerLetter.Rt.position;
        _currentAnimatedRt.sizeDelta = new Vector2(answerLetter.Rt.sizeDelta.x, answerLetter.Rt.sizeDelta.y);

        // Animate moving towards the available space
        
        iTween.MoveTo(animLetter, iTween.Hash(
            "position", questionLetterSpace.Rt.position,
            "time", animationTime));

        iTween.ValueTo(gameObject, iTween.Hash(
            "from", _currentAnimatedRt.sizeDelta,
            "to", new Vector2(
                            utils.GetPercent(questionLetterSpace.Rt.sizeDelta.x, 95),
                            utils.GetPercent(questionLetterSpace.Rt.sizeDelta.y, 91)),
            "time", animationTime,
            "onupdatetarget", gameObject,
            "onupdate", "UpdateLetterScale"
        ));
        
        yield return new WaitForSeconds(animationTime);

        // handle everything within a callback.
        onClickLetterEndAnimation();

        // Add the letter to the current index in the word.
        //questionLetterSpace.Text.text = letter;
        questionLetterSpace.HiddenLetterButtonIndex = index;
    }

    public void UpdateLetterScale(Vector2 newValue)
    {
        _currentAnimatedRt.sizeDelta = newValue;
    }

    private GameObject _animationLetter;
    private GameObject GetAnimationLetter(string c)
    {
        if (_animationLetter != null)
        {
            _animationLetter.GetComponent<LetterButton>().Text.text = c;
            return _animationLetter;
        }
        else
        {
            _animationLetter = Instantiate(Resources.Load("Prefabs/LetterButton", typeof(GameObject))) as GameObject;
            if (_animationLetter != null)
            {
                var layE = _animationLetter.gameObject.AddComponent<LayoutElement>();
                layE.ignoreLayout = true;

                _animationLetter.transform.SetParent(Main.Instance().scope["GameView"].GetComponent<RectTransform>());
                _animationLetter.transform.localScale = new Vector3(1, 1, 1);

                var letB = _animationLetter.GetComponent<LetterButton>();
                letB.Init(100, c,
                    null);
            }
            return _animationLetter;
        }
    }
}