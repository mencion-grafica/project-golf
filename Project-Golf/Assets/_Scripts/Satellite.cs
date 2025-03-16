using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class Satellite : CelestialBody
{
    [Tooltip("The planet that the satellite is orbiting around.")]
    [SerializeField] private Planet planet;
    
    [ContextMenu("Calculate Orbit")]
    public Vector3 CalculateOrbit()
    {
        if (!planet)
        {
            Debug.LogError("No planet assigned to Satellite component.");
            return Vector3.zero;
        }

        Vector3 planetPosition = planet.GetPosition();
        Vector3 satellitePosition = transform.position;
        Vector3 direction = (planetPosition - satellitePosition).normalized;
        float distance = Vector3.Distance(planetPosition, satellitePosition);
        float velocity = Mathf.Sqrt(planet.GetMass() * SimulationManager.Gravity / distance);
        Vector3 velocityVector = Vector3.Cross(direction, Vector3.up).normalized * velocity;
        initialVelocity = velocityVector;
        
        return velocityVector;
    }
    
    public override void StartSimulation()
    {
        initialVelocity = CalculateOrbit();
        base.Start();
        _velocity = initialVelocity;
    }

    public override void StopSimulation()
    {
        _velocity = Vector3.zero;
    }

    public Planet GetPlanet()
    {
        return planet;
    }
    
    public override void UpdateVelocity(float timeStep)
    {
        float sqrDistance = (planet.GetPosition() - _rigidbody.position).sqrMagnitude;
        Vector3 forceDirection = (planet.GetPosition() - _rigidbody.position).normalized;
        Vector3 acceleration = forceDirection * (SimulationManager.Instance.GetGravitationalConstant() * planet.GetMass()) / sqrDistance;
        _velocity += acceleration * timeStep;
        UpdatePosition(timeStep);
    }

    private void OnCollisionEnter(Collision other)
    {
        SimulationManager.Instance.StopSimulation();
    }
}
