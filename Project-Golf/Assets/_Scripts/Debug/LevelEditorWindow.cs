using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;

public class LevelEditorWindow : EditorWindow
{
    private const string IconPath = "Assets/_Scripts/Debug/gear_icon_20px.png";

    private List<GameObject> _planets;
    private List<GameObject> _planetPoints;
    private List<GameObject> _obstacles;
    private GameObject _targetPlanet;
    private GameObject _asteroidSpawner;
    private List<bool> _planetFoldouts;
    private List<bool> _pointFoldouts;
    private List<bool> _obstacleFoldouts;
    
    private int _selectedToolbarIndex = 0;
    private bool _levelCreated = false;
    private string _levelName = "Level 1";
    private float _bigSpace = 15.0f;
    private float _smallSpace = 5.0f;
    private const string SavingAssetPath = "Assets/_Scripts/ScriptableObjectsData/";
    private const bool OpenedFoldoutsFromStart = true;
    
    [MenuItem("Level Editor/Show Window %l", priority = 0)] // Ctrl + L
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<LevelEditorWindow>("Level Editor", true, Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll"));
        Texture icon = AssetDatabase.LoadAssetAtPath<Texture>(IconPath);
        window.titleContent = new GUIContent("Level Editor", icon);
        window.autoRepaintOnSceneChange = true;
    }
    
    [MenuItem("Level Editor/Hide Window %&l", priority = 1)] // Ctrl + Alt + L
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
            Thread.Sleep((int)(step * 1000.0f));
        }
        EditorUtility.ClearProgressBar();
    }

    private void GetLevelData()
    {
        _planets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));
        _obstacles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Obstacle"));
        _planetPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlanetPoint"));
        _targetPlanet = GameObject.FindGameObjectWithTag("TargetPlanet");
        _asteroidSpawner = GameObject.FindGameObjectWithTag("AsteroidSpawner");
        _planetFoldouts = new List<bool>(_planets.Count);
        _pointFoldouts = new List<bool>(_planetPoints.Count);
        _obstacleFoldouts = new List<bool>(_obstacles.Count);
        for (int i = 0; i < _planets.Count; i++) _planetFoldouts.Add(OpenedFoldoutsFromStart);
        for (int i = 0; i < _planetPoints.Count; i++) _pointFoldouts.Add(OpenedFoldoutsFromStart);
        for (int i = 0; i < _obstacles.Count; i++) _obstacleFoldouts.Add(OpenedFoldoutsFromStart);
        
        _planets.Sort((a, b) => a.name.CompareTo(b.name));
        _obstacles.Sort((a, b) => a.name.CompareTo(b.name));
        _planetPoints.Sort((a, b) => a.name.CompareTo(b.name));
    }
    
    private void CreateLevel()
    {
        _levelCreated = true;
        ProgressBar(GetLevelData, "Creating Level", "Getting all obstacles, planets and points...", 0.5f);
        Notify("Level created!", 1.0f);
    }

    private void CreateSOLevelData()
    {
        try
        {
            SOLevelData levelData = CreateInstance<SOLevelData>();
            
            levelData.planets = new List<SOLevelData.PlanetData>();
            foreach (Planet planet in _planets.ConvertAll(planet => planet.GetComponent<Planet>()))
            {
                levelData.planets.Add(new SOLevelData.PlanetData
                {
                    name = planet.name,
                    transform = new SOLevelData.TransformData
                    {
                        position = planet.transform.position,
                        rotation = planet.transform.rotation,
                        scale = planet.transform.localScale
                    },
                    type = planet.GetPlanetType(),
                    mass = planet.GetMass()
                });
            }
            
            levelData.planetPoints = new List<SOLevelData.PlanetPointData>();
            foreach (GameObject point in _planetPoints)
            {
                levelData.planetPoints.Add(new SOLevelData.PlanetPointData
                {
                    name = point.name,
                    transform = new SOLevelData.TransformData
                    {
                        position = point.transform.position,
                        rotation = point.transform.rotation,
                        scale = point.transform.localScale
                    }
                });
            }
            
            levelData.obstacles = new List<SOLevelData.ObstacleData>();
            foreach (GameObject obstacle in _obstacles)
            {
                SOLevelData.ObstacleType type = GetObstacleType(obstacle);
                if (type == SOLevelData.ObstacleType.Satellite)
                {
                    Satellite satellite = obstacle.GetComponent<Satellite>();
                    levelData.obstacles.Add(new SOLevelData.ObstacleData
                    {
                        name = obstacle.name,
                        transform = new SOLevelData.TransformData
                        {
                            position = obstacle.transform.position,
                            rotation = obstacle.transform.rotation,
                            scale = obstacle.transform.localScale
                        },
                        type = type,
                        satellite = new SOLevelData.SatelliteData
                        {
                            planet = satellite.GetPlanet().name
                        }
                    });
                }
                else if (type == SOLevelData.ObstacleType.AsteroidRing)
                {
                    AsteroidRing asteroidRing = obstacle.GetComponent<AsteroidRing>();
                    levelData.obstacles.Add(new SOLevelData.ObstacleData
                    {
                        name = obstacle.name,
                        transform = new SOLevelData.TransformData
                        {
                            position = obstacle.transform.position,
                            rotation = obstacle.transform.rotation,
                            scale = obstacle.transform.localScale
                        },
                        type = type,
                        asteroidRing = new SOLevelData.AsteroidRingData
                        {
                            rotationalVelocity = asteroidRing.GetRotationalVelocity()
                        }
                    });
                }
                else if (type == SOLevelData.ObstacleType.BlackHole)
                {
                    Planet planet = obstacle.GetComponent<Planet>();
                    levelData.obstacles.Add(new SOLevelData.ObstacleData
                    {
                        name = obstacle.name,
                        transform = new SOLevelData.TransformData
                        {
                            position = obstacle.transform.position,
                            rotation = obstacle.transform.rotation,
                            scale = obstacle.transform.localScale
                        },
                        type = type,
                        blackHole = new SOLevelData.BlackHoleData
                        {
                            mass = planet.GetMass()
                        }
                    });
                }
                else if (type == SOLevelData.ObstacleType.WormHole)
                {
                    WormHole wormHole = obstacle.GetComponent<WormHole>();
                    levelData.obstacles.Add(new SOLevelData.ObstacleData
                    {
                        name = obstacle.name,
                        transform = new SOLevelData.TransformData
                        {
                            position = obstacle.transform.position,
                            rotation = obstacle.transform.rotation,
                            scale = obstacle.transform.localScale
                        },
                        type = type,
                        wormHole = new SOLevelData.WormHoleData
                        {
                            planet = wormHole.GetWormHole().name,
                            teleportOffset = wormHole.GetTeleportOffset()
                        }
                    });
                }
            }
            
            levelData.asteroidSpawner = new SOLevelData.AsteroidSpawnerData
            {
                name = _asteroidSpawner.name,
                transform = new SOLevelData.TransformData
                {
                    position = _asteroidSpawner.transform.position,
                    rotation = _asteroidSpawner.transform.rotation,
                    scale = _asteroidSpawner.transform.localScale
                }
            };
            
            levelData.targetPlanet = new SOLevelData.TargetPlanetData
            {
                name = _targetPlanet.name,
                transform = new SOLevelData.TransformData
                {
                    position = _targetPlanet.transform.position,
                    rotation = _targetPlanet.transform.rotation,
                    scale = _targetPlanet.transform.localScale
                }
            };
            
            AssetDatabase.CreateAsset(levelData, SavingAssetPath + _levelName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Selection.activeObject = levelData;
        }
        catch (Exception e)
        {
            Notify(e.Message, 2.0f);
            Debug.LogError(e, this);
        }
    }
    
    private void CloseLevel()
    {
        if (EditorUtility.DisplayDialog("Close Level", "Are you sure you want to close the level?", "Yes", "No"))
        {
            _levelCreated = false;
            Notify("Level closed!", 1.0f);
        }
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
        
        ProgressBar(CreateSOLevelData, "Saving " + _levelName, "Saving all obstacles, planets and points...", 1.0f);
        
        Notify("Level saved!", 1.0f);
        _levelCreated = false;
    }

    private void LoadLevel()
    {
        string path = EditorUtility.OpenFilePanel("Open Level from Scriptable Object", SavingAssetPath, "asset");
        string[] paths = path.Split("/");
        path = paths[^1];
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<SOLevelData>(SavingAssetPath + path);
        
        if (Selection.activeObject is not SOLevelData levelData)
        {
            Notify("Select a level asset!", 1.0f);
            Debug.LogError("Select a level asset!");
            return;
        }
        
        if (Selection.assetGUIDs.Length > 1)
        {
            Notify("Select only one level asset!", 1.0f);
            Debug.LogError("Select only one level asset!");
            return;
        }
            
        ProgressBar(() => { }, "Loading Level", "Loading all obstacles, planets and points...", 0.5f);

        _levelCreated = true;
        _levelName = levelData.name;
        
        _planets = new List<GameObject>();
        _obstacles = new List<GameObject>();
        _planetPoints = new List<GameObject>();

        GameObject activateButton = GameObject.FindWithTag("ActivateButton");
            
        List<GameObject> previousPlanets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));
        List<GameObject> previousObstacles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Obstacle"));
        List<GameObject> previousPlanetPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlanetPoint"));
        GameObject previousTargetPlanet = GameObject.FindGameObjectWithTag("TargetPlanet");
        GameObject previousAsteroidSpawner = GameObject.FindGameObjectWithTag("AsteroidSpawner");
        foreach (GameObject planet in previousPlanets) DestroyImmediate(planet);
        foreach (GameObject obstacle in previousObstacles) DestroyImmediate(obstacle);
        foreach (GameObject point in previousPlanetPoints) DestroyImmediate(point);
        if (previousAsteroidSpawner) DestroyImmediate(previousAsteroidSpawner);
        if (previousTargetPlanet) DestroyImmediate(previousTargetPlanet);
        
        GameObject level = GameObject.FindWithTag("Level");
        foreach (SOLevelData.PlanetPointData pointData in levelData.planetPoints)
        {
            var point = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlanetPoint.prefab"), level.transform, true);
            point.name = pointData.name;
            point.transform.position = pointData.transform.position;
            point.transform.rotation = pointData.transform.rotation;
            point.transform.localScale = pointData.transform.scale;
            point.gameObject.tag = "PlanetPoint";
            _planetPoints.Add(point);
        }
        
        foreach (SOLevelData.PlanetData planetData in levelData.planets)
        {
            var planet = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlanetPrototype.prefab"), level.transform, true);
            planet.name = planetData.name;
            planet.transform.position = planetData.transform.position;
            planet.transform.rotation = planetData.transform.rotation;
            planet.transform.localScale = planetData.transform.scale;
            planet.GetComponent<Planet>().SetPlanetType(planetData.type);
            planet.GetComponent<Planet>().SetMass(planetData.mass);
            planet.gameObject.tag = "Planet";
            _planets.Add(planet);
        }
        
        foreach (SOLevelData.ObstacleData obstacleData in levelData.obstacles)
        {
            SOLevelData.ObstacleType type = obstacleData.type;

            if (type == SOLevelData.ObstacleType.Satellite)
            {
                var satellite = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Satellite.prefab"), level.transform, true);
                satellite.name = obstacleData.name;
                satellite.transform.position = obstacleData.transform.position;
                satellite.transform.rotation = obstacleData.transform.rotation;
                satellite.transform.localScale = obstacleData.transform.scale;
                satellite.GetComponent<Satellite>().SetPlanet(_planets.Find(planet => planet.name == obstacleData.satellite.planet).GetComponent<Planet>());
                satellite.gameObject.tag = "Obstacle";
                _obstacles.Add(satellite);
            }
            else if (type == SOLevelData.ObstacleType.AsteroidRing)
            {
                var asteroidRing = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/AsteroidRing.prefab"), level.transform, true);
                asteroidRing.name = obstacleData.name;
                asteroidRing.transform.position = obstacleData.transform.position;
                asteroidRing.transform.rotation = obstacleData.transform.rotation;
                asteroidRing.transform.localScale = obstacleData.transform.scale;
                asteroidRing.GetComponent<AsteroidRing>().SetRotationalVelocity(obstacleData.asteroidRing.rotationalVelocity);
                asteroidRing.gameObject.tag = "Obstacle";
                _obstacles.Add(asteroidRing);
            }
            else if (type == SOLevelData.ObstacleType.BlackHole)
            {
                var blackHole = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/BlackHole.prefab"), level.transform, true);
                blackHole.name = obstacleData.name;
                blackHole.transform.position = obstacleData.transform.position;
                blackHole.transform.rotation = obstacleData.transform.rotation;
                blackHole.transform.localScale = obstacleData.transform.scale;
                blackHole.GetComponent<Planet>().SetPlanetType(SOLevelData.PlanetType.HighMass);
                blackHole.GetComponent<Planet>().SetMass(obstacleData.blackHole.mass);
                blackHole.gameObject.tag = "Obstacle";
                _obstacles.Add(blackHole);
            }
            else if (type == SOLevelData.ObstacleType.WormHole)
            {
                var wormHole = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/WormHole.prefab"), level.transform, true);
                wormHole.name = obstacleData.name;
                wormHole.transform.position = obstacleData.transform.position;
                wormHole.transform.rotation = obstacleData.transform.rotation;
                wormHole.transform.localScale = obstacleData.transform.scale;
                wormHole.GetComponent<WormHole>().SetTeleportOffset(obstacleData.wormHole.teleportOffset);
                wormHole.gameObject.tag = "Obstacle";
                _obstacles.Add(wormHole);
            }
        }

        foreach (SOLevelData.ObstacleData obstacleData in levelData.obstacles)
        {
            SOLevelData.ObstacleType type = obstacleData.type;
            if (type == SOLevelData.ObstacleType.WormHole)
            {
                WormHole wormHoleComponent = _obstacles.Find(obstacle => obstacle.name == obstacleData.name).GetComponent<WormHole>();
                wormHoleComponent.SetWormHole(_obstacles.Find(obstacle => obstacle.name == obstacleData.wormHole.planet).GetComponent<WormHole>());
            }
        }
        
        _targetPlanet = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TargetPlanet.prefab"), level.transform, true);
        _targetPlanet.name = levelData.targetPlanet.name;
        _targetPlanet.transform.position = levelData.targetPlanet.transform.position;
        _targetPlanet.transform.rotation = levelData.targetPlanet.transform.rotation;
        _targetPlanet.transform.localScale = levelData.targetPlanet.transform.scale;
        _targetPlanet.gameObject.tag = "TargetPlanet";
        
        _asteroidSpawner = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/AsteroidSpawner.prefab"), level.transform, true);
        _asteroidSpawner.name = levelData.asteroidSpawner.name;
        _asteroidSpawner.transform.position = levelData.asteroidSpawner.transform.position;
        _asteroidSpawner.transform.rotation = levelData.asteroidSpawner.transform.rotation;
        _asteroidSpawner.transform.localScale = levelData.asteroidSpawner.transform.scale;
        _asteroidSpawner.gameObject.tag = "AsteroidSpawner";

        if (activateButton)
        {
            activateButton.GetComponent<XRSimpleInteractable>().selectEntered.RemoveAllListeners();
            Debug.Log("Removed all listeners from activate button");
            activateButton.GetComponent<XRSimpleInteractable>().selectEntered.AddListener(_asteroidSpawner.GetComponent<Shoot>().ShootAsteroidFromButton);
            Debug.Log("Added listener to activate button");
        }
        
        _levelCreated = true;
        GetLevelData();
        
        Notify("Level loaded!", 1.0f);
    }

    private void Title(string title, string tooltip = "", bool center = true)
    {
        if (center) GUILayout.BeginHorizontal();
        if (center) GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent(title, tooltip), EditorStyles.boldLabel);
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
    
    private void FinishHorizontal()
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
    
    private void Label(string text, string tooltip = "")
    {
        GUILayout.Label(new GUIContent(text, tooltip));
    }
    
    private void PrefixLabel(string text, string tooltip = "")
    {
        EditorGUILayout.PrefixLabel(new GUIContent(text, tooltip));
    }
    
    private void Space(float space)
    {
        GUILayout.Space(space);
    }
    
    private void Separator()
    {
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
    }
    
    private void BeginHorizontal()
    {
        GUILayout.BeginHorizontal();
    }
    
    private void EndHorizontal()
    {
        GUILayout.EndHorizontal();
    }

    private SOLevelData.ObstacleType GetObstacleType(GameObject obstacle)
    {
        if (obstacle.GetComponent<WormHole>()) return SOLevelData.ObstacleType.WormHole;
        if (obstacle.GetComponent<AsteroidRing>()) return SOLevelData.ObstacleType.AsteroidRing;
        if (obstacle.GetComponent<Satellite>()) return SOLevelData.ObstacleType.Satellite;
        if (obstacle.GetComponent<Planet>()) return SOLevelData.ObstacleType.BlackHole;
        return SOLevelData.ObstacleType.Null;
    }
    
    private void OnGUI()
    {
        Space(_bigSpace);
        Title(!_levelCreated ? "This will get all obstacles, planets and points for a level." : "Level Editor");
        
        Space(_bigSpace);
        Separator();
        Space(_bigSpace);
        
        StartHorizontal();
        if (!_levelCreated) Button("Create Level", CreateLevel, "Create the level data from all obstacles, planets and points");
        if (!_levelCreated) Button("Load Level", LoadLevel, "Load the level data from a ScriptableObject asset");
        FinishHorizontal();
        
        if (_levelCreated)
        {
            BeginHorizontal();
            Label("Level Name");
            _levelName = EditorGUILayout.TextField("", _levelName);
            EndHorizontal();
            
            Space(_bigSpace);
            
            string[] toolbarOptions = {"Obstacles", "Planets", "Points", "Asteroid Spawn", "Target"};
            List<string> toolbarOptionsList = new List<string>(toolbarOptions);
            _selectedToolbarIndex = GUILayout.Toolbar(_selectedToolbarIndex, toolbarOptions);
            Space(_bigSpace);
            
            if (_selectedToolbarIndex == toolbarOptionsList.IndexOf("Planets"))
            {
                foreach (Planet planet in _planets.ConvertAll(planet => planet.GetComponent<Planet>()))
                {
                    _planetFoldouts[_planets.IndexOf(planet.transform.gameObject)] = EditorGUILayout.Foldout(_planetFoldouts[_planets.IndexOf(planet.transform.gameObject)], planet.name, true);
                    if (_planetFoldouts[_planets.IndexOf(planet.transform.gameObject)])
                    {
                        Label("Transform");
                        BeginHorizontal();
                        PrefixLabel("Position");
                        planet.transform.position = EditorGUILayout.Vector3Field("", planet.transform.position);
                        EndHorizontal();

                        BeginHorizontal();
                        PrefixLabel("Rotation");
                        planet.transform.eulerAngles = EditorGUILayout.Vector3Field("", planet.transform.rotation.eulerAngles);
                        EndHorizontal();

                        BeginHorizontal();
                        PrefixLabel("Scale");
                        planet.transform.localScale = EditorGUILayout.Vector3Field("", planet.transform.localScale);
                        EndHorizontal();

                        Space(_smallSpace);

                        BeginHorizontal();
                        PrefixLabel("Mass");
                        planet.SetMass(EditorGUILayout.FloatField("", planet.GetMass()));
                        EndHorizontal();

                        BeginHorizontal();
                        PrefixLabel("Planet Type");
                        planet.SetPlanetType((SOLevelData.PlanetType) EditorGUILayout.EnumPopup("", planet.GetPlanetType()));
                        EndHorizontal();
                        
                        Space(_bigSpace);
                    }
                }
            }
            else if (_selectedToolbarIndex == toolbarOptionsList.IndexOf("Obstacles"))
            {
                foreach (GameObject obstacle in _obstacles)
                {
                    _obstacleFoldouts[_obstacles.IndexOf(obstacle)] = EditorGUILayout.Foldout(_obstacleFoldouts[_obstacles.IndexOf(obstacle)], obstacle.name, true);
                    if (_obstacleFoldouts[_obstacles.IndexOf(obstacle)])
                    {
                        Label("Transform");
                        BeginHorizontal();
                        PrefixLabel("Position");
                        obstacle.transform.position = EditorGUILayout.Vector3Field("", obstacle.transform.position);
                        EndHorizontal();

                        BeginHorizontal();
                        PrefixLabel("Rotation");
                        obstacle.transform.eulerAngles = EditorGUILayout.Vector3Field("", obstacle.transform.rotation.eulerAngles);
                        EndHorizontal();

                        BeginHorizontal();
                        PrefixLabel("Scale");
                        obstacle.transform.localScale = EditorGUILayout.Vector3Field("", obstacle.transform.localScale);
                        EndHorizontal();

                        Space(_smallSpace);

                        BeginHorizontal();
                        PrefixLabel("Obstacle Type", "This type is immutable.");
                        SOLevelData.ObstacleType type = GetObstacleType(obstacle);
                        EditorGUILayout.EnumPopup("", type);
                        EndHorizontal();
                        
                        Space(_smallSpace);
                        
                        if (type == SOLevelData.ObstacleType.Satellite)
                        {
                            Satellite satellite = obstacle.GetComponent<Satellite>();
                            BeginHorizontal();
                            PrefixLabel("Planet");
                            satellite.SetPlanet((Planet) EditorGUILayout.ObjectField("", satellite.GetPlanet(), typeof(Planet), true));
                            EndHorizontal();
                        }
                        else if (type == SOLevelData.ObstacleType.AsteroidRing)
                        {
                            AsteroidRing asteroidRing = obstacle.GetComponent<AsteroidRing>();
                            BeginHorizontal();
                            PrefixLabel("Rotational Velocity");
                            asteroidRing.SetRotationalVelocity(EditorGUILayout.FloatField("", asteroidRing.GetRotationalVelocity()));
                            EndHorizontal();
                        }
                        else if (type == SOLevelData.ObstacleType.BlackHole)
                        {
                            Planet planet = obstacle.GetComponent<Planet>();
                            
                            BeginHorizontal();
                            PrefixLabel("Mass Type", "Black Holes should always have type HighMass.");
                            planet.SetPlanetType((SOLevelData.PlanetType) EditorGUILayout.EnumPopup("", planet.GetPlanetType()));
                            EndHorizontal();
                            
                            BeginHorizontal();
                            PrefixLabel("Mass");
                            planet.SetMass(EditorGUILayout.FloatField("", planet.GetMass()));
                            EndHorizontal();
                        }
                        else if (type == SOLevelData.ObstacleType.WormHole)
                        {
                            WormHole wormHole = obstacle.GetComponent<WormHole>();
                            
                            BeginHorizontal();
                            PrefixLabel("Worm Hole", "The worm hole to teleport to.");
                            wormHole.SetWormHole((WormHole) EditorGUILayout.ObjectField("", wormHole.GetWormHole(), typeof(WormHole), true));
                            EndHorizontal();
                            
                            BeginHorizontal();
                            PrefixLabel("Teleport Offset");
                            wormHole.SetTeleportOffset(EditorGUILayout.IntField("", wormHole.GetTeleportOffset()));
                            EndHorizontal();
                        }
                        
                        Space(_bigSpace);
                    }
                }
            }
            else if (_selectedToolbarIndex == toolbarOptionsList.IndexOf("Points"))
            {
                foreach (GameObject point in _planetPoints)
                {
                    _pointFoldouts[_planetPoints.IndexOf(point)] = EditorGUILayout.Foldout(_pointFoldouts[_planetPoints.IndexOf(point)], point.name, true);
                    if (_pointFoldouts[_planetPoints.IndexOf(point)])
                    {
                        Label("Transform");
                        BeginHorizontal();
                        PrefixLabel("Position");
                        point.transform.position = EditorGUILayout.Vector3Field("", point.transform.position);
                        EndHorizontal();

                        BeginHorizontal();
                        PrefixLabel("Rotation");
                        point.transform.eulerAngles = EditorGUILayout.Vector3Field("", point.transform.rotation.eulerAngles);
                        EndHorizontal();

                        BeginHorizontal();
                        PrefixLabel("Scale");
                        point.transform.localScale = EditorGUILayout.Vector3Field("", point.transform.localScale);
                        EndHorizontal();
                        
                        Space(_bigSpace);
                    }
                }
            }
            else if (_selectedToolbarIndex == toolbarOptionsList.IndexOf("Target"))
            {
                Label(_targetPlanet.name);
                Label("Transform");
                BeginHorizontal();
                PrefixLabel("Position");
                _targetPlanet.transform.position = EditorGUILayout.Vector3Field("", _targetPlanet.transform.position);
                EndHorizontal();

                BeginHorizontal();
                PrefixLabel("Rotation");
                _targetPlanet.transform.eulerAngles = EditorGUILayout.Vector3Field("", _targetPlanet.transform.rotation.eulerAngles);
                EndHorizontal();

                BeginHorizontal();
                PrefixLabel("Scale");
                _targetPlanet.transform.localScale = EditorGUILayout.Vector3Field("", _targetPlanet.transform.localScale);
                EndHorizontal();
            }
            else if (_selectedToolbarIndex == toolbarOptionsList.IndexOf("Asteroid Spawn"))
            {
                Label(_asteroidSpawner.name);
                Label("Transform");
                BeginHorizontal();
                PrefixLabel("Position");
                _asteroidSpawner.transform.position = EditorGUILayout.Vector3Field("", _asteroidSpawner.transform.position);
                EndHorizontal();

                BeginHorizontal();
                PrefixLabel("Rotation");
                _asteroidSpawner.transform.eulerAngles = EditorGUILayout.Vector3Field("", _asteroidSpawner.transform.rotation.eulerAngles);
                EndHorizontal();

                BeginHorizontal();
                PrefixLabel("Scale");
                _asteroidSpawner.transform.localScale = EditorGUILayout.Vector3Field("", _asteroidSpawner.transform.localScale);
                EndHorizontal();
            }
            
            Space(_bigSpace);
            Separator();
            Space(_bigSpace);
            
            StartHorizontal();
            Button("Close Level", CloseLevel, "Close the level loaded. Does not delete the level data");
            Button("Save Level", SaveLevel, "Save the level data to a ScriptableObject asset at " + SavingAssetPath);
            FinishHorizontal();
        }
    }
}
#endif