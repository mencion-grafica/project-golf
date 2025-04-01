using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private GameObject asteroid; 

    public void shoot()
    {

        SimulationManager.Instance.StopSimulation();
        var obj =Instantiate(asteroid, this.transform.position, Quaternion.identity);
        SimulationManager.Instance.AddCelestialBody(obj);
        SimulationManager.Instance.StartSimulation();
    }
}