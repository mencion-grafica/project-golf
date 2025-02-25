using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance { get; private set; }

    [Header("Simulation Settings")]
    [SerializeField] private float gravity = 0.0001f;
    [SerializeField] private float physicsTimeStep = 0.01f;
    public static float Gravity = 0.1f;
    
    [Header("Simulation Objects")]
    [SerializeField] private Asteroid activeAsteroid = null;
    private List<Planet> _planets = new List<Planet>();
    
    [Header("Simulation State")]
    [SerializeField] private bool isSimulationRunning = false;
    
    [ContextMenu("Get All Planets")]
    private void GetAllPlanets()
    {
        _planets = new List<Planet>(FindObjectsOfType<Planet>());
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

    private void Start()
    {
        Time.fixedDeltaTime = physicsTimeStep;
        _planets = new List<Planet>(FindObjectsOfType<Planet>());
    }

    public void StartSimulation()
    {
        activeAsteroid?.StartSimulation();
        isSimulationRunning = true;
    }
    
    public void StopSimulation()
    {
        activeAsteroid?.StopSimulation();
        isSimulationRunning = false;
    }

    private void FixedUpdate()
    {
        if (!isSimulationRunning) return;
        
        activeAsteroid?.UpdateVelocity(physicsTimeStep);
    }

    public List<Planet> GetPlanets()
    {
        return _planets;
    }
}
