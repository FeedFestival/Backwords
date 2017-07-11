using System.Collections;
using System.Collections.Generic;
using Assets.scripts.utils;
using UnityEngine;

public class Main : MonoBehaviour
{
    public bool DebugScript;

    private static Main _main;
    public static Main Instance()
    {
        return _main;
    }

    public User LoggedUser;


    [HideInInspector]
    public DataService DataService;

    [HideInInspector]
    public Dictionary<string, GameObject> scope;
    [HideInInspector]
    public GameController GameController;
    [HideInInspector]
    public LevelController LevelController;
    [HideInInspector]
    public LocaleController LocaleController;

    void Awake()
    {
        _main = GetComponent<Main>();
    }

    void Start()
    {
        scope = CanvasController.Instance().GetScope();

        utils.Setup();

        GameController = scope["GameView"].GetComponent<GameController>();
        GameController.gameObject.SetActive(false);

        LevelController = scope["LevelView"].GetComponent<LevelController>();
        LevelController.gameObject.SetActive(false);

        LocaleController = scope["LocaleView"].GetComponent<LocaleController>();
        LocaleController.gameObject.SetActive(false);

        scope["Splash"].SetActive(true);
        StartCoroutine(
            ShowIntro(() =>
        {
            scope["Splash"].SetActive(false);

            LocaleController.Init();
        }));

        scope["LevelCompletedSplash"].SetActive(false);
        
        /*
         * ---------------------------------------------------------------------
         * * ---------------------------------------------------------------------
         * * ---------------------------------------------------------------------
         */

        DataService = new DataService("Database.db");
        CreateSession();
    }

    // callbacks
    public delegate void OnSplashFinish();


    IEnumerator ShowIntro(OnSplashFinish onSplashFinish)
    {
        yield return new WaitForSeconds(2f);

        onSplashFinish();
    }

    public void CreateSession()
    {
        LoggedUser = DataService.GetUser();
        if (LoggedUser == null)
        {
            DataService.CreateDB();
            var user = new User
            {
                Id = 1,
                Name = "Player 1",
                Language = "en_US",
                Maps = 0
            };
            Debug.Log("This is a fresh install. Creating user..." + user);
            DataService.CreateUser(user);
            //
            LoggedUser = DataService.GetUser();
        }
        Debug.Log("Logged in. " + LoggedUser);
    }
}