using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    private LevelManager _levelManager;

    private static int level = 0;
    
    private void OnEnable()
    {
        _levelManager = (LevelManager) target;
    }
    
    private void Button(string text, Action function, string tooltip = "", bool center = false, bool space = true)
    {
        if (space) GUILayout.Space(10);
        if (center) GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent(text, tooltip))) function();
        GUILayout.FlexibleSpace();
        if (center) GUILayout.EndHorizontal();
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUILayout.Space(15);
        Button("Get All Levels", _levelManager.GetLevels, "Get all scriptable objects to load them in the level manager. NOTE: This will sort the levels alphabetically. Make sure to reorder them in the inspector if you want to change the order.");
        GUILayout.Space(15);
        
        EditorGUILayout.HelpBox("Following buttons are for testing the Level Manager in Play Mode.", MessageType.Warning);
        GUILayout.Space(15);
        
        Button("Load Current Level", _levelManager.LoadLevel, "This will load the level that is currently set in the Level Manager.");
        Button("Next Level", _levelManager.NextLevel, "This will load the next level in the list.");
        Button("Previous Level", _levelManager.PreviousLevel, "This will load the previous level in the list.");
        
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Level to load", "This will load the level of the index from the list."));
        level = EditorGUILayout.IntField("", level);
        GUILayout.EndHorizontal();
        Button("Load Level", () => _levelManager.LoadLevel(level), "This will load the level of the index from the list.", false, false);
    }
}
#endif