using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{
    [SerializeField, Range(0.0f, 10000.0f)] private float mass = 1.0f;
    private Transform _center;
    private Rigidbody _rigidbody;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _center = transform;
        _rigidbody.mass = mass;
    }
    
    public float GetRadius()
    {
        return transform.localScale.x * 0.5f;
    }
    
    public float GetMass()
    {
        return mass;
    }
    
    public Vector3 GetPosition()
    {
        if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
        return _rigidbody.position;
    }
}
