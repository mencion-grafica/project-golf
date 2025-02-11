using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] private Vector3 initialVelocity = Vector3.zero;
    [SerializeField, Range(0.0f, 100.0f)] private float mass = 1.0f;
    [SerializeField] private bool canMove = false;
    
    private Rigidbody _rigidbody;
    private Vector3 _direction;
    private LineRenderer _lineRenderer;
    
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _direction = Vector3.forward;
        _rigidbody.mass = mass;
    }
    
    private void StopMoving()
    {
        _rigidbody.velocity = Vector3.zero;
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public Vector3 GetVelocity()
    {
        return _direction;
    }
    
    public float GetMass()
    {
        return mass;
    }

    private void AddForce(Vector3 direction)
    {
        _rigidbody.AddForce(direction, ForceMode.Impulse);
    }

    private Vector3 CalculateForce(Planet planet)
    {
        Vector3 direction = planet.GetCenter().position - transform.position;
        float distance = direction.sqrMagnitude;
        float force = SimulationManager.Instance.GetGravitationalConstant() * (planet.GetMass() * mass) / distance;
        return direction.normalized * force;
    }

    private void CalculateDirection()
    {
        List<Planet> planets = SimulationManager.Instance.GetPlanets();
        Vector3 direction = Vector3.zero;
        foreach (Planet planet in planets) direction += CalculateForce(planet);
        _direction = direction;
        _lineRenderer.SetPosition(1, direction.normalized);
        AddForce(direction);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            canMove = !canMove;
            if (!canMove) StopMoving();
            else AddForce(initialVelocity);
        }
        
        if (canMove) CalculateDirection();
    }
}
