using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] private Vector3 initialVelocity = Vector3.forward;
    [SerializeField, Range(0.0f, 100.0f)] private float mass = 1.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float radius = 1.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float surfaceGravity = 1.0f;
    [SerializeField] private bool canMove = false;
    
    private Rigidbody _rigidbody;
    private Vector3 _velocity;
    private LineRenderer _lineRenderer;
    private List<Planet> _planets;
    
    [SerializeField] private int numSteps = 1000;
    private int _index = 1;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, _rigidbody.position);
        mass = surfaceGravity * radius * radius / SimulationManager.Instance.GetGravitationalConstant();
        transform.localScale = Vector3.one * radius;
        _rigidbody.mass = mass;
        _velocity = initialVelocity;
        _planets = SimulationManager.Instance.GetPlanets();
    }

    public void UpdateVelocity(float timeStep)
    {
        foreach (Planet planet in _planets)
        {
            float sqrDistance = (planet.GetPosition() - _rigidbody.position).sqrMagnitude;
            Vector3 forceDirection = (planet.GetPosition() - _rigidbody.position).normalized;
            Vector3 acceleration = forceDirection * (SimulationManager.Instance.GetGravitationalConstant() * planet.GetMass()) / sqrDistance;
            _velocity += acceleration * timeStep;
        }
        UpdatePosition(timeStep);
    }
    
    public void UpdateVelocity(Vector3 acceleration, float timeStep)
    {
        _velocity += acceleration * timeStep;
    }
    
    public void UpdatePosition(float timeStep)
    {
        _rigidbody.MovePosition(_rigidbody.position + _velocity * timeStep);
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_index++, _rigidbody.position);
    }

    public void StartSimulation()
    {
        _velocity = initialVelocity;
    }
    
    public void StopSimulation()
    {
        _velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("WormHole")) return;
        SimulationManager.Instance.StopSimulation();
    }

    public Vector3 GetPosition()
    {
        return _rigidbody.position;
    }
    
    public Vector3 GetVelocity()
    {
        return initialVelocity;
    }
    
    public float GetMass()
    {
        return mass;
    }
}
