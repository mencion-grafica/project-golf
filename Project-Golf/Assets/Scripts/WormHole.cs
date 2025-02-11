using System.Collections;
using UnityEngine;

public class WormHole : Planet
{
    [SerializeField] private WormHole planet; // Wormhole destino
    [SerializeField] private int teleportOffset = 10; // Distancia extra en la dirección del movimiento

    private void OnCollisionEnter(Collision other)
    {
        if (planet != null && planet != this)
        {
            Debug.Log("mover");

            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 moveDirection = rb.velocity.normalized; // Obtener la dirección del movimiento
                Vector3 teleportPosition = planet.GetPosition() + new Vector3(0,0,4); // Nueva posición con offset
                Debug.Log(teleportPosition);
                other.transform.position = teleportPosition; // Teletransportar con desplazamiento
                rb.velocity = rb.velocity; // Mantener la velocidad original
            }
        }
    }
}

