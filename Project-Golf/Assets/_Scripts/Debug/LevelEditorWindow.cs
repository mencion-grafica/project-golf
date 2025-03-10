using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private const string IconPath = "Assets/_Scripts/Debug/gear_icon_20px.png";

    private List<Planet> _planets;
    private int _selectedToolbarIndex = 0;
    private bool _levelCreated = false;
    private string _levelName = "Level 1";
    private float _bigSpace = 15.0f;
    private float _smallSpace = 5.0f;
    private string _savingAssetPath = "Assets/_Scripts/ScriptableObjectsData/";
    
    [MenuItem("Level Editor/Show Window")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<LevelEditorWindow>();
        Texture icon = AssetDatabase.LoadAssetAtPath<Texture>(IconPath);
        window.titleContent = new GUIContent("Level Editor", icon);
        window.autoRepaintOnSceneChange = true;
    }
    
    [MenuItem("Level Editor/Hide Window")]
    public static void HideWindow()
    {
        GetWindow<LevelEditorWindow>().Close();
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
        _planets.Sort((a, b) => a.name.CompareTo(b.name));
    }
    
    private void CreateLevel()
    {
        _levelCreated = true;
        ProgressBar(GetPlanets, "Creating Level", "Getting all obstacles, planets and points...", 5.0f);
        Notify("Level created!", 1.0f);
    }
    
    private void SaveLevel()
    {
        if (_levelName == "")
        {
            Notify("Level name cannot be empty!", 1.0f);
            Debug.LogError("Level name cannot be empty!");
            return;
        }
        
        List<string> levelNames = new List<string>();
        foreach (SOLevelData lvlData in Resources.FindObjectsOfTypeAll<SOLevelData>()) levelNames.Add(lvlData.name);
        if (levelNames.Contains(_levelName))
        {
            Notify("Level name already exists!", 1.0f);
            Debug.LogError("Level name already exists!");
            return;
        }
        
        Debug.Log("Saving level " + _levelName + "...");
        ProgressBar(() => { }, "Saving " + _levelName, "Saving all obstacles, planets and points...", 5.0f);
        Notify("Level saved!", 1.0f);
        
        // TODO: Do try catch to notify if the asset was not saved correctly
        SOLevelData levelData = CreateInstance<SOLevelData>();
        levelData.planets = new List<SOLevelData.PlanetData>();
        foreach (Planet planet in _planets)
        {
            levelData.planets.Add(new SOLevelData.PlanetData
            {
                name = planet.name,
                position = planet.transform.position,
                mass = planet.GetMass(),
                prefab = planet.gameObject
            });
        }
        AssetDatabase.CreateAsset(levelData, _savingAssetPath + _levelName + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        _levelCreated = false;
    }

    private void LoadLevel()
    {
        Notify("Level loaded!", 1.0f);
        
        if (Selection.activeObject is SOLevelData levelData)
        {
            if (Selection.assetGUIDs.Length > 1)
            {
                Notify("Select only one level asset!", 1.0f);
                Debug.LogError("Select only one level asset!");
                return;
            }
            
            ProgressBar(() => { }, "Loading Level", "Loading all obstacles, planets and points...", 5.0f);

            _levelCreated = true;
            _levelName = levelData.name;
            _planets = new List<Planet>();
            
            foreach (SOLevelData.PlanetData planetData in levelData.planets)
            {
                GameObject planet = Instantiate(planetData.prefab, planetData.position, Quaternion.identity);
                //planet.transform.position = planetData.position;
                //planet.AddComponent<Rigidbody>();
                //planet.AddComponent<Planet>().SetMass(planetData.mass);
                planet.name = planetData.name;
                planet.GetComponent<Planet>().SetMass(planetData.mass);
                _planets.Add(planet.GetComponent<Planet>());
            }
        }
        else
        {
            Notify("Select a level asset!", 1.0f);
            Debug.LogError("Select a level asset!");
        }
    }

    private void Title(string title, bool center = true)
    {
        if (center) GUILayout.BeginHorizontal();
        if (center) GUILayout.FlexibleSpace();
        GUILayout.Label(title, EditorStyles.boldLabel);
        if (center) GUILayout.FlexibleSpace();
        if (center) GUILayout.EndHorizontal();
    }

    private void Button(string text, Action function, string tooltip = "")
    {
        if (GUILayout.Button(new GUIContent(text, tooltip))) function();
    }
    
    private void StartHorizontal()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
    }
    
    private void EndHorizontal()
    {
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
    
    private void StartVertical()
    {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
    }
    
    private void EndVertical()
    {
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }
    
    private void Space(float space)
    {
        GUILayout.Space(space);
    }
    
    private void OnGUI()
    {
        Space(_bigSpace);
        Title(!_levelCreated ? "This will get all obstacles, planets and points for a level." : "Level Editor");
        Space(_bigSpace);
        
        StartHorizontal();
        Button("Create Level", CreateLevel, "Create the level data from all obstacles, planets and points");
        Button("Load Level", LoadLevel, "Load the level data from a ScriptableObject asset");
        EndHorizontal();
        
        if (_levelCreated)
        {
            Space(_bigSpace);
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Level Name");
            _levelName = EditorGUILayout.TextField("", _levelName);
            GUILayout.EndHorizontal();
            
            Space(_bigSpace);
            _selectedToolbarIndex = GUILayout.Toolbar(_selectedToolbarIndex, new[] {"Obstacles", "Planets", "Points"});
            //Space(_bigSpace);
            
            foreach (Planet planet in _planets)
            {
                if (_selectedToolbarIndex == 1)
                {
                    Title(planet.name, false);
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Position");
                    EditorGUILayout.Vector3Field("", planet.transform.position);
                    GUILayout.EndHorizontal();
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Mass");
                    EditorGUILayout.FloatField("", planet.GetMass());
                    GUILayout.EndHorizontal();
                    
                    Space(_smallSpace);
                }
            }
            
            Space(_bigSpace);
            
            StartHorizontal();
            Button("Close Level", () => _levelCreated = false, "Close the level loaded. Does not delete the level data");
            Button("Save Level", SaveLevel, "Save the level data to a ScriptableObject asset");
            EndHorizontal();
        }
    }
}