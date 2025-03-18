using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRing : MonoBehaviour
{
    [Tooltip("Valores inferiores a 1 haran que rote cada vez mas lento")]
    [SerializeField, Range(0, 50)]
    private float rotationVelocity = 1.0f;

    void Update()
    {
        Vector3 rotation = new Vector3 (0, rotationVelocity, 0);
        transform.Rotate(rotation);
    }
}
