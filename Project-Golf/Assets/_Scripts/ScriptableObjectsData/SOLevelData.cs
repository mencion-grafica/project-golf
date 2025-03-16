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
        Null
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
        
    }
    
    [Header("Planets")]
    public List<PlanetData> planets;
    
    [Header("Planet Points")]
    public List<PlanetPointData> planetPoints;
    
    [Header("Obstacles")]
    public List<ObstacleData> obstacles;
}