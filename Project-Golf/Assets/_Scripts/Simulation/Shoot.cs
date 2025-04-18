using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private GameObject asteroid;

    public void ShootAsteroid()
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

    public void ShootAsteroidFromButton(SelectEnterEventArgs args)
    {
        ShootAsteroid();
    }
}