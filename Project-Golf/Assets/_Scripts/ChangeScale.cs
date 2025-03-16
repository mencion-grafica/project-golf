using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeScale : MonoBehaviour
{
    private Vector3 initialScale;
    /*[SerializeField]
    private List<float> magnitudes;*/
    [SerializeField]
    private float massMultiplier;
    private float initialMass;
    private float mass;
    private Planet planet;

    private void Awake()
    {
        initialScale = transform.localScale;
        planet = transform.GetComponent<Planet>();
        initialMass = planet.GetMass();
    }

    /*private void Update()
    {
        Rescale(); 
    }*/

    public void Rescale()
    {
        float currentScale = transform.localScale.x;

        mass = initialMass * currentScale * massMultiplier;

        planet.SetMass(mass);
    }
}
