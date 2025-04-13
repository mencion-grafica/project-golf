using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButton : MonoBehaviour
{
    public void onClick()
    {
        if (SimulationManager.Instance.IsSimulationRunning())
            SimulationManager.Instance.StopSimulation();
        else
        {
            SimulationManager.Instance.StartSimulation();
            SimulationManager.Instance.ShootAsteroid();
        }
            
    }
}
