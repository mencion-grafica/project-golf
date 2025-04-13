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
    
    [Tooltip("The celestial body to display the orbit of.")]
    [SerializeField] private CelestialBody body;
    private List<CelestialBody> _bodies;
    
    private struct VirtualBody
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;
        public float radius;

        public VirtualBody(CelestialBody celestialBody)
        {
            position = celestialBody.GetPosition();
            velocity = celestialBody.GetVelocity();
            mass = celestialBody.GetMass();
            radius = celestialBody.GetRadius();
        }
    }
    
    private void Start()
    {
        _bodies = new List<CelestialBody>(FindObjectsOfType<CelestialBody>());
        if (Application.isPlaying) HideOrbits();
    }

    private void Update()
    {
        _bodies = new List<CelestialBody>(FindObjectsOfType<CelestialBody>());
        if (!Application.isPlaying) DrawOrbits();
    }

    private bool CheckCollisions(List<Planet> planets, VirtualBody virtualBody)
    {
        foreach (Planet planet in planets) if (CheckCollision(planet, virtualBody)) return true;
        return false;
    }

    private bool CheckCollision(Planet planet, VirtualBody virtualBody)
    {
        float sqrDistance = (planet.GetPosition() - virtualBody.position).sqrMagnitude;
        float sqrRadius = planet.GetRadius() + virtualBody.radius;
        return sqrDistance < sqrRadius * sqrRadius;
    }

    private void DrawOrbits()
    {
        if (!displayOrbits)
        {
            HideOrbits();
            return;
        }
        
        if (!body) DrawAllOrbits();
        else HideOrbits();
        
        if (body is Asteroid) DrawAsteroidOrbit(body);
        else if (body is Satellite) DrawSatelliteOrbit(body);
    }

    private void DrawAllOrbits()
    {
        foreach (CelestialBody forBody in _bodies) DrawOrbit(forBody);
    }

    private void DrawOrbit(CelestialBody fBody)
    {
        if (fBody is Asteroid) DrawAsteroidOrbit(fBody);
        else if (fBody is Satellite) DrawSatelliteOrbit(fBody);
    }
    
    private void DrawSatelliteOrbit(CelestialBody fBody)
    {
        List<Vector3> drawPoints = new List<Vector3>();
        VirtualBody virtualBody = new VirtualBody(fBody);
        virtualBody.velocity = ((Satellite) fBody).CalculateOrbit();
        Planet planet = ((Satellite) fBody).GetPlanet();
        bool collision = false;
        
        for (int i = 0; i < numSteps && !collision; i++)
        {
            float sqrDistance = (planet.GetPosition() - virtualBody.position).sqrMagnitude;
            Vector3 forceDirection = (planet.GetPosition() - virtualBody.position).normalized;
            Vector3 acceleration = forceDirection * (SimulationManager.Gravity * planet.GetMass()) / sqrDistance;
            virtualBody.velocity += acceleration * timeStep;
            virtualBody.position += virtualBody.velocity * timeStep;
            drawPoints.Add(virtualBody.position);
            if (CheckCollision(planet, virtualBody)) collision = true;
        }
        
        LineRenderer lineRenderer = fBody.GetComponent<LineRenderer>();
        lineRenderer.positionCount = drawPoints.Count;
        lineRenderer.SetPositions(drawPoints.ToArray());
        lineRenderer.startColor = collision ? Color.red : Color.green;
        lineRenderer.endColor = collision ? Color.red : Color.green;
    }
    
    private void DrawAsteroidOrbit(CelestialBody fBody)
    {
        List<Planet> planets = new List<Planet>(FindObjectsOfType<Planet>());
        List<Vector3> drawPoints = new List<Vector3>();
        VirtualBody virtualBody = new VirtualBody(fBody);
        bool collision = false;
        
        for (int i = 0; i < numSteps && !collision; i++)
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
            if (CheckCollisions(planets, virtualBody)) collision = true;
        }
        
        LineRenderer lineRenderer = fBody.GetComponent<LineRenderer>();
        lineRenderer.positionCount = drawPoints.Count;
        lineRenderer.SetPositions(drawPoints.ToArray());
        lineRenderer.startColor = collision ? Color.red : Color.green;
        lineRenderer.endColor = collision ? Color.red : Color.green;
    }
    
    private void HideOrbits()
    {
        LineRenderer lineRenderer = null;
        foreach (CelestialBody fBody in _bodies)
        {
            lineRenderer = fBody.GetComponent<LineRenderer>();
            if (lineRenderer) lineRenderer.positionCount = 0;
        }
    
        lineRenderer = body?.GetComponent<LineRenderer>();
        if (lineRenderer) lineRenderer.positionCount = 0;
    }
}