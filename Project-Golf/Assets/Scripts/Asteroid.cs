using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour
{
    [SerializeField, Range(0.0f, 10.0f)] private float speed = 0.0f;
    [SerializeField, Range(0.0f, 100.0f)] private float mass = 1.0f;
    [SerializeField] private bool canMove = false;
    
    private Rigidbody _rigidbody;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void StartMoving()
    {
        _rigidbody.AddForce(Vector3.forward * speed, ForceMode.Impulse);
    }
    
    private void StopMoving()
    {
        _rigidbody.AddForce(Vector3.back * speed, ForceMode.Impulse);
    }

    private void AddForce(Vector3 direction)
    {
        _rigidbody.AddForce(direction, ForceMode.Impulse);
    }

    private Vector3 CalculateForce(Planet planet)
    {
        Vector3 direction = planet.GetCenter().position - transform.position;
        float distance = direction.magnitude;
        float force = planet.GetGravity() * (planet.GetMass() * mass) / Mathf.Pow(distance, 2);
        Debug.Log(force);
        return direction.normalized * force;
    }
    
    private void CalculateDirection()
    {
        List<Planet> planets = PlanetManager.Instance.GetPlanets();
        Vector3 direction = Vector3.zero;
        foreach (Planet planet in planets) direction += CalculateForce(planet);
        AddForce(direction);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartMoving();
            canMove = true;
        }
        
        if (canMove) CalculateDirection();
    }
}
