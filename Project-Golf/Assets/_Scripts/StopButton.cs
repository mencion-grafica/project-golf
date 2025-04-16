using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButton : MonoBehaviour
{
    public void onClick()
    {
        if (SimulationManager.Instance.IsSimulationRunning())
        {
            //MoveLevel.Instance.AdoptNonActivePlanets();
            SimulationManager.Instance.StopSimulation();
        }
        else
        {
            MoveLevel.Instance.OrphanNonActivePlanets();
            StartCoroutine(MoveLevel.Instance.MoveToSpawn());
        }
            
    }
}
