using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Assets.scripts.utils;
using UnityEngine;

public class style : MonoBehaviour
{
    //public bool RelativeToParent;
    public RectTransform RelativeTo;

    private RectTransform _rt;
    private RectTransform _parentRt;

    public float Width;

    void Start()
    {
        Init();

        Setup();
    }

    public void Init()
    {
        _rt = GetComponent<RectTransform>();

        if (RelativeTo == null)
        {
            _parentRt = _rt.parent.GetComponent<RectTransform>();
        }
        else
        {
            _parentRt = RelativeTo;
        }
    }

    private void Setup()
    {
        _rt.sizeDelta = new Vector2(utils.GetPercent(_parentRt.sizeDelta.x, Width), _rt.sizeDelta.y);
    }
}