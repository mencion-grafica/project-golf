using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance { get; private set; }

    [SerializeField] private float gravity = 0.0001f;
    [SerializeField] private float physicsTimeStep = 0.01f;
    
    [SerializeField] private List<Planet> planets = new List<Planet>();
    
    [ContextMenu("Get All Planets")]
    private void GetAllPlanets()
    {
        planets = new List<Planet>(FindObjectsOfType<Planet>());
    }
    
    public float GetGravitationalConstant()
    {
        return gravity;
    }
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public List<Planet> GetPlanets()
    {
        return planets;
    }
}
