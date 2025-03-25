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
    float y; 

    void Update()
    {
        y += Time.deltaTime * rotationalVelocity;
        transform.rotation = Quaternion.Euler(transform.rotation.x, y, transform.rotation.z);
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
