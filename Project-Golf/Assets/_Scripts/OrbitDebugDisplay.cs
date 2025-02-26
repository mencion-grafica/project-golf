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
        List<Planet> planets = new List<Planet>(FindObjectsOfType<Planet>());
        List<Vector3> drawPoints = new List<Vector3>();
        VirtualBody virtualBody = new VirtualBody(asteroid);
        
        for (int i = 0; i < numSteps; i++)
        {
            foreach (Planet planet in planets)
            {
                float sqrDistance = (planet.GetPosition() - virtualBody.position).sqrMagnitude;
                Vector3 forceDirection = (planet.GetPosition() - virtualBody.position).normalized;
                Vector3 acceleration = forceDirection * (SimulationManager.Gravity * planet.GetMass()) / sqrDistance;
                virtualBody.velocity += acceleration * timeStep;
            }
            virtualBody.position += virtualBody.velocity * timeStep;
            drawPoints.Add(virtualBody.position);
        }
        
        LineRenderer lineRenderer = asteroid.GetComponent<LineRenderer>();
        lineRenderer.positionCount = drawPoints.Count;
        lineRenderer.SetPositions(drawPoints.ToArray());
    }
    
    private void HideOrbits()
    {
        LineRenderer lineRenderer = asteroid.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }
}
