using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data SO", menuName = "ScriptableObjects/Level Data", order = 0)]
public class SOLevelData : ScriptableObject
{
    [Serializable]
    public struct PlanetData
    {
        public string name;
        public Vector3 position;
        public float mass;
        public GameObject prefab;
    }
    
    [Header("Planets")]
    public List<PlanetData> planets;
}