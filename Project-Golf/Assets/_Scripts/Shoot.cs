using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private GameObject asteroid; 

    public void shoot()
    {
        var objs = GameObject.FindObjectsOfType<Asteroid>();

        SimulationManager.Instance.StopSimulation();

        foreach (Asteroid asteroid in objs)
        {
            SimulationManager.Instance.RemoveCelestialBody(asteroid.gameObject);
            Destroy(asteroid.gameObject);
        }

        
        var obj = Instantiate(asteroid, this.transform.position, Quaternion.identity);
        SimulationManager.Instance.AddCelestialBody(obj);
        SimulationManager.Instance.StartSimulation();
    }
}