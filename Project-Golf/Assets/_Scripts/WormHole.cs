using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.Serialization;

public class WormHole : Planet
{
    [SerializeField] private WormHole wormHole;
    [SerializeField] private int teleportOffset = 10;
    private Vector3 asteroidVelocity;
    private float asteroidVelocityFloat;

    private Vector3 frontPosition;
    private double distanciaAsteroide;
    private double distancia;
    private Vector3 asteroidPosition;
    private Vector3 wormPosition;
    private Vector3 vectorDirectorRecta;

    public WormHole GetWormHole()
    {
        return wormHole;
    }
    
    public void SetWormHole(WormHole newWormHole)
    {
        this.wormHole = newWormHole;
    }
    
    public int GetTeleportOffset()
    {
        return teleportOffset;
    }
    
    public void SetTeleportOffset(int newTeleportOffset)
    {
        this.teleportOffset = newTeleportOffset;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Asteroid asteroid = other.gameObject.GetComponent<Asteroid>();
        if (wormHole != null && wormHole != this)
        {
            // Obtener el vector de movimiento del asteroide (su dirección normalizada)
            vectorDirectorRecta = asteroid.GetCurrentVelocity().normalized;

            // Obtener el vector forward/up del wormhole
            Vector3 vectorUp = transform.up.normalized;

            // Calcular el producto punto
            float productoPunto = Vector3.Dot(vectorDirectorRecta, vectorUp);

            // Obtener el ángulo en radianes
            float anguloRadianes = Mathf.Acos(productoPunto);

            // Convertir a grados
            float anguloGrados = anguloRadianes * Mathf.Rad2Deg;

            // Imprimir el resultado
            if(anguloGrados > 90 && anguloGrados <= 180)
            {
                //Vector3 teleportPosition = planet.GetPosition() + 4 * planet.transform.up;
                Vector3 teleportPosition = wormHole.GetPosition() + 1 * wormHole.transform.up;
                asteroidVelocity = asteroid.GetCurrentVelocity();
                other.transform.position = teleportPosition;
                asteroidVelocityFloat = asteroidVelocity.magnitude;

                asteroid.SetCurrentVelocity(wormHole.transform.up * asteroidVelocityFloat);
            }
            //Debug.Log("Ángulo entre el asteroide y forward del Wormhole: " + asteroid.GetCurrentVelocity() + "°" + anguloGrados);

        }
    }
}
//vectorForward
//180-270 +     +
//90-180 -      -

//0-90 -        +
//270-360 -     -

//0-45  +