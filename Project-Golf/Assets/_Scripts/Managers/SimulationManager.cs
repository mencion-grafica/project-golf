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
    private List<CelestialBody> _celestialBodies = new List<CelestialBody>();
    private List<Planet> _planets = new List<Planet>();
    
    [Header("Simulation State")]
    [SerializeField] private bool isSimulationRunning = false;
    
    [ContextMenu("Get All Planets")]
    private void GetAllPlanets()
    {
        _planets = new List<Planet>(FindObjectsOfType<Planet>());
        _planets = _planets.FindAll(planet => planet.gameObject.activeInHierarchy);
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
        _celestialBodies = new List<CelestialBody>(FindObjectsOfType<CelestialBody>());
        GetAllPlanets();
        StartSimulation();
    }
    
    public void StartSimulation()
    {
        isSimulationRunning = true;
        foreach (CelestialBody celestialBody in _celestialBodies) celestialBody.StartSimulation();
    }
    
    public void StopSimulation()
    {
        isSimulationRunning = false;
        foreach (CelestialBody celestialBody in _celestialBodies) celestialBody.StopSimulation();
    }

    private void FixedUpdate()
    {
        if (!isSimulationRunning) return;
        foreach (CelestialBody celestialBody in _celestialBodies) celestialBody.UpdateVelocity(physicsTimeStep);
    }

    public List<Planet> GetPlanets()
    {
        return _planets;
    }

    public void AddCelestialBody(GameObject obj)
    {
        _celestialBodies.Add(obj.GetComponent<CelestialBody>());
    }
}
