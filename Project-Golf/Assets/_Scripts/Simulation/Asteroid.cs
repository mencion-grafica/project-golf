using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid : CelestialBody
{
    public override void StartSimulation()
    {
        base.Start();
        _velocity = initialVelocity;
    }
    
    public override void StopSimulation()
    {
        _velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<WormHole>()) return;
        SimulationManager.Instance.StopSimulation();
    }
}
