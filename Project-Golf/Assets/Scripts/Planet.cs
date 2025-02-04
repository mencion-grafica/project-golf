using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField, Range(0.0f, 50.0f)] private float gravity = 9.8f;
    [SerializeField, Range(0.0f, 100.0f)] private float mass = 1.0f;
    private Transform _center;

    private void Start()
    {
        _center = transform;
    }
    
    public float GetGravity()
    {
        return gravity;
    }
    
    public float GetMass()
    {
        return mass;
    }
    
    public Transform GetCenter()
    {
        return _center;
    }
}
