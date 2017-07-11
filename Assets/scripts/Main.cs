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

    private User _loggedUser;

    [HideInInspector]
    public DataService DataService;

    [HideInInspector]
    public Dictionary<string, GameObject> scope;
    [HideInInspector]
    public GameController GameController;
    [HideInInspector]
    public LevelController LevelController;

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

        scope["Splash"].SetActive(true);
        StartCoroutine(
            ShowIntro(() =>
        {
            scope["Splash"].SetActive(false);

            LevelController.Init();
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
        yield return new WaitForSeconds(0.1f);

        onSplashFinish();
    }
    
    public void CreateSession()
    {
        _loggedUser = DataService.GetUser();
        Debug.Log(_loggedUser);
        if (_loggedUser == null)
        {
            DataService.CreateDB();

            var user = new User
            {
                Id = 1,
                Name = "Player 1",
                Maps = 0
            };

            DataService.CreateUser(user);
        }
    }
}