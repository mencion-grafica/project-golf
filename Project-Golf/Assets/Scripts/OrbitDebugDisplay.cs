using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitDebugDisplay : MonoBehaviour
{
    [SerializeField] private int numSteps = 1000;
    [SerializeField] private float timeStep = 0.1f;
    [SerializeField] private bool usePhysicsTimeStep;
    [SerializeField] private bool displayOrbits = true;

    [Space(10)] 
    
    [SerializeField] private Asteroid asteroid;
    
    private void Start()
    {
        if (Application.isPlaying) HideOrbits();
    }

    private void Update()
    {
        if (!Application.isPlaying) DrawOrbits();
    }

    private struct VirtualBody
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public VirtualBody(Asteroid asteroid)
        {
            position = asteroid.GetPosition();
            velocity = asteroid.GetVelocity();
            mass = asteroid.GetMass();
        }
    }
    
    private void DrawOrbits()
    {
        List<Planet> planets = SimulationManager.Instance.GetPlanets();
        List<Vector3> drawPoints = new List<Vector3>();
        int referenceFrameIndex = 0;
        Vector3 referenceBodyInitialPosition = Vector3.zero;

        
    }
    
    private void HideOrbits()
    {
        throw new NotImplementedException();
    }
}
