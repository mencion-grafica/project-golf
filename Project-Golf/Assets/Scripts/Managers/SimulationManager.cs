using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance { get; private set; }

    [Header("Simulation Settings")]
    [SerializeField] private float gravity = 0.0001f;
    [SerializeField] private float physicsTimeStep = 0.01f;
    
    [Header("Simulation Objects")]
    [SerializeField] private Asteroid activeAsteroid = null;
    [SerializeField] private List<Planet> planets = new List<Planet>();
    
    [Header("Simulation State")]
    [SerializeField] private bool isSimulationRunning = false;
    
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
        
        Time.fixedDeltaTime = physicsTimeStep;
        Debug.Log("Simulation Manager initialized...");
        Debug.Log("Physics time step: " + physicsTimeStep);
    }

    private void FixedUpdate()
    {
        if (!isSimulationRunning) return;
        
        activeAsteroid?.UpdateVelocity(physicsTimeStep);
    }

    public List<Planet> GetPlanets()
    {
        return planets;
    }
}
