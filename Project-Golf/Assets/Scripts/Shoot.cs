using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private Asteroid asteroid; 

    public void shoot()
    {
        Instantiate(asteroid, this.transform.position, Quaternion.identity);
    }
}