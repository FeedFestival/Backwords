using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocaleController : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(true);
    }

    public void OnLanguageSelected(string locale)
    {
        Main.Instance().LoggedUser.Language = locale;

        Main.Instance().LevelController.Init();

        gameObject.SetActive(false);
    }
}
