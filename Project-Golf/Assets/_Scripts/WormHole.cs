using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class WormHole : Planet
{
    [SerializeField] private WormHole planet;
    [SerializeField] private int teleportOffset = 10;
    private Vector3 asteroidVelocity;
    private float asteroidVelocityFloat;

    private Vector3 frontPosition;
    private double distanciaAsteroide;
    private double distancia;


    private void OnCollisionEnter(Collision other)
    {
        if (planet != null && planet != this)
        {
            Asteroid asteroid = other.gameObject.GetComponent<Asteroid>();
            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
            frontPosition = rb.transform.forward * (rb.transform.localScale[0] / 2) + rb.transform.position;
            distanciaAsteroide = Math.Sqrt(Math.Pow(frontPosition[0] - asteroid.transform.position[0], 2) +
                                  Math.Pow(frontPosition[1] - asteroid.transform.position[1], 2) +
                                  Math.Pow(frontPosition[2] - asteroid.transform.position[2], 2));
            distancia = Math.Sqrt(2 * Math.Pow(rb.transform.localScale[0] / 2, 2));
            if (asteroid != null && distanciaAsteroide < distancia + 0.5)
            {
                Vector3 teleportPosition = planet.GetPosition() + 4 * planet.transform.forward;
                asteroidVelocity = asteroid.GetCurrentVelocity();
                other.transform.position = teleportPosition;
                asteroidVelocityFloat = Math.Abs(asteroidVelocity[0] + asteroidVelocity[1] + asteroidVelocity[2]);
                asteroid.SetCurrentVelocity(planet.transform.forward * asteroidVelocityFloat);
            }
        }
    }
}

