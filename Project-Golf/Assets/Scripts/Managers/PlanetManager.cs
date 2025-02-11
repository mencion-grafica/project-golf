using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager Instance { get; private set; }

    [SerializeField] private List<Planet> planets = new List<Planet>();
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public List<Planet> GetPlanets()
    {
        return planets;
    }
}
