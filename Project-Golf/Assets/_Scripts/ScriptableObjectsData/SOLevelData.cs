using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data SO", menuName = "ScriptableObjects/Level Data", order = 0)]
public class SOLevelData : ScriptableObject
{
    public enum PlanetType
    {
        LowMass,
        MediumMass,
        HighMass
    }

    public enum ObstacleType
    {
        AsteroidRing,
        BlackHole,
        WormHole,
        Satellite,
        ObstaclePlanet,
        Null
    }
    
    [Serializable]
    public struct AsteroidRingData
    {
        public float rotationalVelocity;
    }
    
    [Serializable]
    public struct SatelliteData
    {
        public string planet;
        public Vector3 initialVelocity;
    }
    
    [Serializable]
    public struct BlackHoleData
    {
        public float mass;
    }
    
    [Serializable]
    public struct WormHoleData
    {
        public string planet;
        public int teleportOffset;
    }

    [Serializable]
    public struct ObstaclePlanetData
    {
        public PlanetType planetType;
        public float mass;
    }
    
    [Serializable]
    public struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }
    
    [Serializable]
    public struct PlanetData
    {
        public string name;
        public TransformData transform;
        public PlanetType type;
        public float mass;
    }
    
    [Serializable]
    public struct PlanetPointData
    {
        public string name;
        public TransformData transform;
    }

    [Serializable]
    public struct ObstacleData
    {
        public string name;
        public TransformData transform;
        public ObstacleType type;
        public SatelliteData satellite;
        public BlackHoleData blackHole;
        public WormHoleData wormHole;
        public AsteroidRingData asteroidRing;
        public ObstaclePlanetData obstaclePlanet;
    }
    
    [Serializable]
    public struct TargetPlanetData
    {
        public string name;
        public TransformData transform;
    }
    
    [Serializable]
    public struct AsteroidSpawnerData
    {
        public string name;
        public TransformData transform;
    }
    
    [Header("Planets")]
    public List<PlanetData> planets;
    
    [Header("Planet Points")]
    public List<PlanetPointData> planetPoints;
    
    [Header("Obstacles")]
    public List<ObstacleData> obstacles;
    
    [Header("Target Planet")]
    public TargetPlanetData targetPlanet;
    
    [Header("Asteroid Spawner")]
    public AsteroidSpawnerData asteroidSpawner;
}