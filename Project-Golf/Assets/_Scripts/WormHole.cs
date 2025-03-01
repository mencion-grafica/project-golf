using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class WormHole : Planet
{
    [SerializeField] private WormHole planet; // Wormhole destino
    [SerializeField] private int teleportOffset = 10; // Distancia extra en la dirección del movimiento
    private Vector3 asteroidPosition;
    private Vector3 asteroidVelocity;
    private float asteroidVelocityFloat;

    private Vector3 frontPosition;
    private double distancia;

    private Vector3 wormPosition;
    private Vector3 linea1;
    private Vector3 linea2;
    private Vector3 vectorDirectorPlano;
    private Vector3 vectorDirectorRecta;

    private void OnCollisionEnter(Collision other)
    {
        if (planet != null && planet != this)
        {
            Debug.Log("mover");

            Asteroid asteroid = other.gameObject.GetComponent<Asteroid>();
            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
            frontPosition = rb.transform.forward * (rb.transform.localScale[0] / 2) + rb.transform.position;
            distancia = Math.Sqrt(Math.Pow(frontPosition[0] - asteroid.transform.position[0], 2) +
                                  Math.Pow(frontPosition[1] - asteroid.transform.position[1], 2) +
                                  Math.Pow(frontPosition[2] - asteroid.transform.position[2], 2));
            if (asteroid != null && distancia < 6)
            {
               // Vector3 moveDirection = rb.velocity.normalized; // Obtener la dirección del movimiento
                Vector3 teleportPosition = planet.GetPosition() + 4 * planet.transform.forward; // Nueva posición con offset
                Debug.Log(teleportPosition);
                Debug.Log("transform.forward: " + planet.transform.forward);
                Debug.Log("planet.GetPosition(): " + planet.GetPosition());
                Debug.Log("transform.position: " + planet.transform.position);
                asteroidVelocity = asteroid.GetCurrentVelocity();
                other.transform.position = teleportPosition; // Teletransportar con desplazamiento
                asteroidVelocityFloat = Math.Abs(asteroidVelocity[0] + asteroidVelocity[1] + asteroidVelocity[2]);


                //rb.transform.forward = planet.transform.forward;
                asteroid.SetCurrentVelocity(planet.transform.forward * asteroidVelocityFloat); // Teletransportar con desplazamiento
                Debug.Log("asteroidVelocity: " + planet.transform.forward * asteroidVelocityFloat);
                // rb.velocity = rb.velocity; // Mantener la velocidad original
            }
           
            Debug.Log("La distancia es: " + distancia);
        }
    }
}

