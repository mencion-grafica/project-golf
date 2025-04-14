using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AsteroidRing : MonoBehaviour
{
    [FormerlySerializedAs("rotationVelocity")]
    [Tooltip("Valores inferiores a 1 haran que rote cada vez mas lento")]
    [SerializeField, Range(0, 50)]
    private float rotationalVelocity = 1.0f;

    void Update()
    {
        Vector3 rotation = new Vector3 (0, rotationalVelocity * Time.deltaTime, 0);
        transform.Rotate(rotation);
    }
    
    public float GetRotationalVelocity()
    {
        return rotationalVelocity;
    }
    
    public void SetRotationalVelocity(float newRotationVelocity)
    {
        rotationalVelocity = newRotationVelocity;
    }
}
