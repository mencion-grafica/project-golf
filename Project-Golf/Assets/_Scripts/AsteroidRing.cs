using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRing : MonoBehaviour
{
    [Tooltip("Valores inferiores a 1 haran que rote cada vez mas lento")]
    [SerializeField, Range(0, 50)]
    private float rotationVelocity = 1.0f;
    float y; 

    void Update()
    {
        y += Time.deltaTime * rotationVelocity;
        transform.rotation = Quaternion.Euler(0, y, 0);
    }
}
