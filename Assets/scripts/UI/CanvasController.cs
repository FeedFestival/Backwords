using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    private static CanvasController _canvasController;
    public static CanvasController Instance()
    {
        return _canvasController;
    }

    void Awake()
    {
        _canvasController = GetComponent<CanvasController>();
    }

    public Dictionary<string, GameObject> GetScope()
    {
        var list = new Dictionary<string, GameObject>();

        Transform[] childList = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in childList)
        {
            if (child.name.Contains("{"))
            {
                int pTo = child.name.LastIndexOf("}");
                var name = child.name.Substring(1, pTo - 1);
                //Debug.Log(name);
                list.Add(name, child.gameObject);
            }
        }

        return list;
    }
}
