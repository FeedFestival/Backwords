using System;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts.utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(DomainLogic))]
public class DomainLogicEditor : Editor
{
    private DomainLogic _myScript;

    private bool _setupConfirm;

    public enum InspectorButton
    {
        RecreateDataBase, CleanUpUsers, LoadLevelsCsv, UpdateMap
    }

    private InspectorButton _actionTool;
    private InspectorButton _action
    {
        get { return _actionTool; }
        set
        {
            _actionTool = value;
            _setupConfirm = true;
        }
    }
    
    public override void OnInspectorGUI()
    {
        _myScript = (DomainLogic)target;

        EditorGUILayout.BeginVertical("box");
        GUILayout.Space(5);

        GUILayout.Label("Database");
        GUILayout.Space(5);

        if (GUILayout.Button("Recreate Database"))
            _action = InspectorButton.RecreateDataBase;

        if (GUILayout.Button("Clean Up Users"))
            _action = InspectorButton.CleanUpUsers;

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        GUILayout.Space(10);    // MAP
                                //--------------------------------------------------------------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        GUILayout.Space(10);    // CREATE
                                //--------------------------------------------------------------------------------------------------------------------------------------------------------

        EditorGUILayout.BeginVertical("box");
        GUILayout.Space(5);

        GUILayout.Label("Load levels form csv file and put it in sqlite db.");
        GUILayout.Space(5);
        
        if (GUILayout.Button("Load levels"))
            _action = InspectorButton.LoadLevelsCsv;

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        GUILayout.Space(10);    // UPDATE
                                //--------------------------------------------------------------------------------------------------------------------------------------------------------

        EditorGUILayout.BeginVertical("box");
        GUILayout.Space(5);

        GUILayout.Label("Map Update");
        GUILayout.Space(5);

        //EditorGUILayout.BeginHorizontal();

        //if (GUILayout.Button("Get maps", GUILayout.Width(UiUtils.GetPercent(Screen.width, 25)), GUILayout.Height(30)))
        //    _myScript.GetMaps();

        //GUILayout.FlexibleSpace();

        //EditorGUILayout.EndHorizontal();

        // Map List
        //if (_myScript.Maps != null)
        //{
        //    EditorGUILayout.BeginVertical("box");

        //    foreach (var map in _myScript.Maps)
        //    {
        //        EditorGUILayout.BeginHorizontal();

        //        EditorGUILayout.BeginVertical();

        //        GUILayout.Label("Id:" + map.Id, GUILayout.Width(UiUtils.GetPercent(Screen.width, 10)));

        //        EditorGUILayout.BeginHorizontal();

        //        GUILayout.Label("Name :");
        //        map.Name = EditorGUILayout.TextField(map.Name);

        //        EditorGUILayout.EndHorizontal();

        //        EditorGUILayout.BeginHorizontal();

        //        GUILayout.Label("Number :");
        //        map.Number = EditorGUILayout.IntField(map.Number);

        //        EditorGUILayout.EndHorizontal();

        //        EditorGUILayout.EndVertical();

        //        GUILayout.Space(UiUtils.GetPercent(Screen.width, 5));

        //        EditorGUILayout.BeginVertical();

        //        if (GUILayout.Button("Generate Map", GUILayout.Width(UiUtils.GetPercent(Screen.width, 25))))
        //        {
        //            _myScript.SetupCurrentMap(map.Id);
        //            _myScript.LoadLevelsCsv(true);
        //        }
        //        if (_myScript.CurrentMap != null && _myScript.CurrentMap.Id == map.Id)
        //            if (GUILayout.Button("Update ?", GUILayout.Width(UiUtils.GetPercent(Screen.width, 25))))
        //                _myScript.MapId = map.Id;

        //        EditorGUILayout.EndVertical();


        //        EditorGUILayout.EndHorizontal();
        //        GUILayout.Space(5);
        //    }

        //    EditorGUILayout.EndVertical();
        //}

        GUILayout.Space(25);

        //if (_myScript.CurrentMap != null && _myScript.MapId != 0)
        //{

        //    EditorGUILayout.BeginVertical("box");
        //    GUILayout.Space(5);


        //    EditorGUILayout.BeginVertical("box");

        //    //--
        //    EditorGUILayout.BeginHorizontal();

        //    EditorGUILayout.BeginVertical();

        //    GUILayout.Label("Id:" + _myScript.CurrentMap.Id, GUILayout.Width(UiUtils.GetPercent(Screen.width, 10)));

        //    EditorGUILayout.BeginHorizontal();

        //    GUILayout.Label("Map :");
        //    _myScript.CurrentMap.GameObject = EditorGUILayout.ObjectField(_myScript.CurrentMap.GameObject, typeof(GameObject), true) as GameObject;

        //    EditorGUILayout.EndHorizontal();

        //    EditorGUILayout.BeginHorizontal();

        //    GUILayout.Label("Name :");
        //    _myScript.CurrentMap.Name = EditorGUILayout.TextField(_myScript.CurrentMap.Name);

        //    EditorGUILayout.EndHorizontal();

        //    EditorGUILayout.BeginHorizontal();

        //    GUILayout.Label("Number :");
        //    _myScript.CurrentMap.Number = EditorGUILayout.IntField(_myScript.CurrentMap.Number);

        //    EditorGUILayout.EndHorizontal();

        //    EditorGUILayout.EndVertical();

        //    GUILayout.Space(UiUtils.GetPercent(Screen.width, 5));

        //    if (GUILayout.Button("Update Map", GUILayout.Width(UiUtils.GetPercent(Screen.width, 25)), GUILayout.Height(50)))
        //        _action = InspectorButton.UpdateMap;

        //    EditorGUILayout.EndHorizontal();
        //    GUILayout.Space(5);

        //    //--

        //    EditorGUILayout.EndVertical();

        //    GUILayout.Space(5);
        //    EditorGUILayout.EndVertical();

        //}

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        GUILayout.Space(20);    // CONFIRM
                                //--------------------------------------------------------------------------------------------------------------------------------------------------------

        if (_setupConfirm)
        {
            EditorGUILayout.BeginVertical("box");
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Confirm", GUILayout.Width(utils.GetPercent(Screen.width, 25)), GUILayout.Height(50)))
                ConfirmAccepted();

            if (GUILayout.Button("Cancel", GUILayout.Width(utils.GetPercent(Screen.width, 25)), GUILayout.Height(50)))
                _setupConfirm = false;

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.BeginVertical();
        }
    }

    private void ConfirmAccepted()
    {
        switch (_action)
        {
            case InspectorButton.RecreateDataBase:

                _myScript.RecreateDataBase();
                break;

            case InspectorButton.CleanUpUsers:

                _myScript.CleanUpUsers();
                break;

            case InspectorButton.LoadLevelsCsv:

                _myScript.LoadLevelsFromCsv();
                break;

            //case InspectorButton.UpdateMap:

            //    _myScript.GenerateMapSql(_myScript.CurrentMap);
            //    break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        _setupConfirm = false;
    }
}
