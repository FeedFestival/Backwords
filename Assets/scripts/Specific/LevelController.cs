using System.Collections;
using System.Collections.Generic;
using Assets.scripts.utils;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public Dictionary<int, Level> Levels = new Dictionary<int, Level>();

    public List<CButton> LevelButtons;

    public void Init()
    {
        Levels.Add(1, new Level
        {
            Id = 1,
            Word = "ace"
        });

        Levels.Add(2, new Level
        {
            Id = 2,
            Word = "Pasare"
        });

        Levels.Add(3, new Level
        {
            Id = 3,
            Word = "cub"
        });

        Levels.Add(4, new Level
        {
            Id = 4,
            Word = "felinar"
        });

        Levels.Add(5, new Level
        {
            Id = 5,
            Word = "bujii"
        });

        Levels.Add(6, new Level
        {
            Id = 6,
            Word = "desoxirribonucleic"
        });

        /*
         * 
         * 
         * */

        for (var i = 7; i < 35; i++)
        {
            Levels.Add(i, new Level { Id = i, Word = "test" });
        }

        gameObject.SetActive(true);

        StartCoroutine(ShowLevels());
    }

    IEnumerator ShowLevels()
    {
        yield return new WaitForSeconds(0.01f);

        var rt = Main.Instance().scope["LevelGrid"].GetComponent<RectTransform>();
        var gridLayoutGroup = Main.Instance().scope["LevelGrid"].GetComponent<GridLayoutGroup>();

        var width = rt.sizeDelta.x;
        
        var cellSize = width / 6;
        
        var paddingX = utils.GetPercent(cellSize, 20);
        
        gridLayoutGroup.cellSize = new Vector2(cellSize - paddingX, cellSize - paddingX);

        var rows = (int)Levels.Count / 6;
        var parentHeight = rows * (cellSize - paddingX);
        parentHeight = parentHeight + utils.GetPercent(parentHeight, 5);

        rt.sizeDelta = new Vector2(rt.sizeDelta.x, parentHeight);

        LevelButtons = new List<CButton>();
        for (var i = 1; i <= Levels.Count; i++)
        {
            GameObject letter = Instantiate(Resources.Load("Prefabs/LevelButton", typeof(GameObject))) as GameObject;
            if (letter != null)
            {
                letter.transform.SetParent(rt);
                letter.transform.localScale = new Vector3(1, 1, 1);

                var but = letter.GetComponent<CButton>();
                but.Init(i, i.ToString(),
                    OnLevelSelected);

                LevelButtons.Add(but);
            }
        }
    }

    private void OnLevelSelected(int index)
    {
        Debug.Log("Selected Level:" + index);

        Main.Instance().GameController.Init();
        Main.Instance().GameController.StartGame(Levels[index], OnLeveCompleted);

        gameObject.SetActive(false);
    }

    private void OnLeveCompleted(int index)
    {
        Main.Instance().scope["LevelCompletedSplash"].SetActive(true);

        // Coroutine requirements
        Main.Instance().scope["LevelGrid"].SetActive(false);
        gameObject.SetActive(true);

        StartCoroutine(LevelCompletedSplash(index));
    }

    IEnumerator LevelCompletedSplash(int index)
    {
        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);
        Main.Instance().scope["LevelCompletedSplash"].SetActive(false);

        Main.Instance().GameController.Init();
        Main.Instance().GameController.StartGame(Levels[index + 1], OnLeveCompleted);
    }
}

public class Level
{
    public int Id;
    public string Word;
}