using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    public bool DebugScript;

    private static Main _main;
    public static Main Instance()
    {
        return _main;
    }

    public Dictionary<string, GameObject> scope;
    public GameController GameController;
    public LevelController LevelController;

    void Awake()
    {
        _main = GetComponent<Main>();
    }

    void Start()
    {
        scope = CanvasController.Instance().GetScope();

        GameController = scope["GameView"].GetComponent<GameController>();
        GameController.gameObject.SetActive(false);

        LevelController = scope["LevelView"].GetComponent<LevelController>();
        LevelController.gameObject.SetActive(false);

        scope["Splash"].SetActive(true);
        StartCoroutine(
            ShowIntro(() =>
        {
            scope["Splash"].SetActive(false);

            LevelController.Init();
        }));

        scope["LevelCompletedSplash"].SetActive(false);
    }

    // callbacks
    public delegate void OnSplashFinish();


    IEnumerator ShowIntro(OnSplashFinish onSplashFinish)
    {
        yield return new WaitForSeconds(3f);

        onSplashFinish();
    }

    public int GetPennyToss()
    {
        var randomNumber = Random.Range(0, 100);
        return (randomNumber > 50) ? 1 : 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //GameController.StartGame();
        }
    }
}