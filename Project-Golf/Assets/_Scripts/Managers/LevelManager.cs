using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private List<SOLevelData> levels;
    [SerializeField] private SOLevelData currentLevel;
    
    private int _currentLevelIndex;

    private List<GameObject> _planets;
    private List<GameObject> _obstacles;
    private List<GameObject> _planetPoints;
    
    private GameObject _targetPlanet;
    private GameObject _asteroidSpawner;
    
    private bool _isCinematicStarted = false;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _currentLevelIndex = 0;
    }

    public void SetIsCinematic(bool isCinematic)
    {
        _isCinematicStarted = isCinematic;
    }
    
    public bool IsCinematicStarted()
    {
        return _isCinematicStarted;
    }
    
    public void PreviousLevel()
    {
        _currentLevelIndex = Mathf.Clamp(_currentLevelIndex - 1, 0, levels.Count - 1);
        LoadLevel(levels[_currentLevelIndex]);
    }
    
    public void NextLevel()
    {
        _currentLevelIndex = Math.Clamp(_currentLevelIndex + 1, 0, levels.Count - 1);
        LoadLevel(levels[_currentLevelIndex]);
    }
    
    public bool IsLastLevel()
    {
        return _currentLevelIndex == levels.Count - 1;
    }
    
    public SOLevelData GetCurrentLevel()
    {
        return currentLevel;
    }
    
    public void SetCurrentLevel(SOLevelData newLevel)
    {
        currentLevel = newLevel;
    }

    #if UNITY_EDITOR
    public void GetLevels()
    {
        levels = new List<SOLevelData>();

        string[] guids = AssetDatabase.FindAssets("t:SOLevelData", new[] { "Assets/_Scripts/ScriptableObjectsData" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SOLevelData levelData = AssetDatabase.LoadAssetAtPath<SOLevelData>(path);
            if (levelData) levels.Add(levelData);
        }
    }
    #endif

    private void CleanLevel()
    {
        _planets = new List<GameObject>();
        _obstacles = new List<GameObject>();
        _planetPoints = new List<GameObject>();
        
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
        
        GameObject asteroid = GameObject.FindGameObjectWithTag("Asteroid");
        if (asteroid) DestroyImmediate(asteroid);
    }
    
    public void LoadLevel()
    {
        CleanLevel();
        
        GameObject level = GameObject.FindWithTag("Level");
        foreach (SOLevelData.PlanetPointData pointData in currentLevel.planetPoints)
        {
            var point = Instantiate(Resources.Load<GameObject>("PlanetPoint"), level.transform, true);
            point.name = pointData.name;
            point.transform.position = pointData.transform.position;
            point.transform.rotation = pointData.transform.rotation;
            point.transform.localScale = pointData.transform.scale;
            point.gameObject.tag = "PlanetPoint";
            _planetPoints.Add(point);
        }
        
        foreach (SOLevelData.PlanetData planetData in currentLevel.planets)
        {
            var planet = Instantiate(Resources.Load<GameObject>("PlanetPrototype"), level.transform, true);
            planet.name = planetData.name;
            planet.transform.position = planetData.transform.position;
            planet.transform.rotation = planetData.transform.rotation;
            planet.transform.localScale = planetData.transform.scale;
            planet.GetComponent<Planet>().SetPlanetType(planetData.type);
            planet.GetComponent<Planet>().SetMass(planetData.mass);
            planet.gameObject.tag = "Planet";
            _planets.Add(planet);
        }
        
        foreach (SOLevelData.ObstacleData obstacleData in currentLevel.obstacles)
        {
            SOLevelData.ObstacleType type = obstacleData.type;

            if (type == SOLevelData.ObstacleType.ObstaclePlanet)
            {
                var obstaclePlanet = Instantiate(Resources.Load<GameObject>("ObstaclePlanet"), level.transform, true);
                obstaclePlanet.name = obstacleData.name;
                obstaclePlanet.transform.position = obstacleData.transform.position;
                obstaclePlanet.transform.rotation = obstacleData.transform.rotation;
                obstaclePlanet.transform.localScale = obstacleData.transform.scale;
                obstaclePlanet.GetComponent<Planet>().SetPlanetType(obstacleData.obstaclePlanet.planetType);
                obstaclePlanet.GetComponent<Planet>().SetMass(obstacleData.obstaclePlanet.mass);
                obstaclePlanet.gameObject.tag = "Obstacle";
                _obstacles.Add(obstaclePlanet);
            }
            else if (type == SOLevelData.ObstacleType.Satellite)
            {
                var satellite = Instantiate(Resources.Load<GameObject>("Satellite"), level.transform, true);
                satellite.name = obstacleData.name;
                satellite.transform.position = obstacleData.transform.position;
                satellite.transform.rotation = obstacleData.transform.rotation;
                satellite.transform.localScale = obstacleData.transform.scale;
                satellite.GetComponent<Satellite>().SetPlanet(_obstacles.Find(obstacle => obstacle.name == obstacleData.satellite.planet).GetComponent<Planet>());
                satellite.gameObject.tag = "Obstacle";
                _obstacles.Add(satellite);
            }
            else if (type == SOLevelData.ObstacleType.AsteroidRing)
            {
                var asteroidRing = Instantiate(Resources.Load<GameObject>("AsteroidRing"), level.transform, true);
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
                var blackHole = Instantiate(Resources.Load<GameObject>("BlackHole"), level.transform, true);
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
                var wormHole = Instantiate(Resources.Load<GameObject>("WormHole"), level.transform, true);
                wormHole.name = obstacleData.name;
                wormHole.transform.position = obstacleData.transform.position;
                wormHole.transform.rotation = obstacleData.transform.rotation;
                wormHole.transform.localScale = obstacleData.transform.scale;
                wormHole.GetComponent<WormHole>().SetTeleportOffset(obstacleData.wormHole.teleportOffset);
                wormHole.gameObject.tag = "Obstacle";
                _obstacles.Add(wormHole);
            }
        }

        foreach (SOLevelData.ObstacleData obstacleData in currentLevel.obstacles)
        {
            SOLevelData.ObstacleType type = obstacleData.type;
            if (type == SOLevelData.ObstacleType.WormHole)
            {
                WormHole wormHoleComponent = _obstacles.Find(obstacle => obstacle.name == obstacleData.name).GetComponent<WormHole>();
                wormHoleComponent.SetWormHole(_obstacles.Find(obstacle => obstacle.name == obstacleData.wormHole.planet).GetComponent<WormHole>());
            }
        }
        
        _targetPlanet = Instantiate(Resources.Load<GameObject>("TargetPlanet"), level.transform, true);
        _targetPlanet.name = currentLevel.targetPlanet.name;
        _targetPlanet.transform.position = currentLevel.targetPlanet.transform.position;
        _targetPlanet.transform.rotation = currentLevel.targetPlanet.transform.rotation;
        _targetPlanet.transform.localScale = currentLevel.targetPlanet.transform.scale;
        _targetPlanet.gameObject.tag = "TargetPlanet";
        
        _asteroidSpawner = Instantiate(Resources.Load<GameObject>("AsteroidSpawner"), level.transform, true);
        _asteroidSpawner.name = currentLevel.asteroidSpawner.name;
        _asteroidSpawner.transform.position = currentLevel.asteroidSpawner.transform.position;
        _asteroidSpawner.transform.rotation = currentLevel.asteroidSpawner.transform.rotation;
        _asteroidSpawner.transform.localScale = currentLevel.asteroidSpawner.transform.scale;
        _asteroidSpawner.gameObject.tag = "AsteroidSpawner";
        
        SimulationManager.Instance.SetUpSimulation();
    }
    
    public void LoadLevel(SOLevelData level)
    {
        currentLevel = level;
        LoadLevel();
    }

    public void LoadLevel(int index)
    {
        _currentLevelIndex = Math.Clamp(index, 0, levels.Count - 1);
        LoadLevel(levels[_currentLevelIndex]);
    }
}