using System.Collections;
using UnityEngine;

public class WormHole : Planet
{
    [SerializeField] private WormHole planet; // Wormhole destino
    public bool tpCouldown = false; // Control de cooldown
    [SerializeField] private float teleportOffset = 2f; // Distancia extra en la dirección del movimiento

    private void OnTriggerEnter(Collider other)
    {
        if (planet != null && planet != this && !tpCouldown)
        {
            Debug.Log("mover");

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 moveDirection = rb.velocity.normalized; // Obtener la dirección del movimiento
                Vector3 teleportPosition = planet.GetCenter().position + moveDirection * 5; // Nueva posición con offset
                Debug.Log(moveDirection);
                Debug.Log(moveDirection * 5);
                other.transform.position = teleportPosition; // Teletransportar con desplazamiento
                rb.velocity = rb.velocity; // Mantener la velocidad original

                // Activar cooldown en ambos wormholes
               // tpCouldown = true;
               // planet.tpCouldown = true;

                StartCoroutine(ResetCooldown());
                planet.StartCoroutine(planet.ResetCooldown());
            }
        }
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(0.1f); // Esperar 3 segundos
        tpCouldown = false; // Desactivar cooldown
    }
}

