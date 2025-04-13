using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CelestialBody : MonoBehaviour
{
    [SerializeField] protected Vector3 initialVelocity = Vector3.zero;
    [SerializeField, Range(0.0f, 100.0f)] protected float mass = 1.0f;
    [SerializeField, Range(0.0f, 1000.0f)] protected float radius = 1.0f;
    [SerializeField, Range(0.0f, 1000.0f)] protected float surfaceGravity = 1.0f;

    protected List<Planet> _planets;
    protected Rigidbody _rigidbody;
    protected Vector3 _velocity;
    protected LineRenderer _lineRenderer;

    protected void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, _rigidbody.position);
        mass = surfaceGravity * radius * radius / (SimulationManager.Gravity);
        //transform.localScale = Vector3.one * radius;
        _rigidbody.mass = mass;
        _velocity = initialVelocity;
        _planets = Application.isPlaying ? SimulationManager.Instance.GetPlanets() : new List<Planet>(FindObjectsOfType<Planet>());
    }

    public virtual void StartSimulation() { }
    public virtual void StopSimulation() { }

    public virtual void UpdateVelocity(float timeStep)
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
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _rigidbody.position);
    }

    public float GetRadius()
    {
        return transform.localScale.x * 0.5f;
    }

    public Vector3 GetPosition()
    {
        if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
        return _rigidbody.position;
    }

    public Vector3 GetVelocity()
    {
        return initialVelocity;
    }

    public Vector3 GetCurrentVelocity()
    {
        return _velocity;
    }

    public void SetCurrentVelocity(Vector3 newVelocity)
    {
        _velocity = newVelocity;
    }

    public float GetMass()
    {
        return mass;
    }
}