using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class ExampleWindow : EditorWindow
{
    private List<Planet> _planets;
    
    [MenuItem("Level Editor/Show Window")]
    public static void ShowWindow()
    {
        GetWindow<ExampleWindow>("Level Editor");
    }
    
    [MenuItem("Level Editor/Hide Window")]
    public static void HideWindow()
    {
        GetWindow<ExampleWindow>().Close();
    }
    
    private void Notify(string message, float time)
    {
        ShowNotification(new GUIContent(message), time);
    }
    
    private void ProgressBar(Action function, string title, string info, float time)
    {
        var step = 0.1f;
        function();
        for (float t = 0; t < time; t += step)
        {
            EditorUtility.DisplayProgressBar(title, info, t / time);
            Thread.Sleep((int)(step * 100.0f)); // Simulate work
        }
        EditorUtility.ClearProgressBar();
    }

    private void GetPlanets()
    {
        _planets = new List<Planet>(FindObjectsOfType<Planet>());
    }
    
    private void CreateLevel()
    {
        Debug.Log("Creating level...");
        ProgressBar(GetPlanets, "Creating Level", "Getting all obstacles, planets and points for a level...", 5.0f);
        foreach (Planet planet in _planets) Debug.Log("Planet: " + planet.name);
        Notify("Level created!", 1.0f);
    }

    private void OnGUI()
    {
        GUILayout.Space(15);
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("This will get all obstacles, planets and points for a level.", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.Space(15);
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Create Level")) CreateLevel();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
