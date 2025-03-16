using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject[] hands;

    private void OnEnable()
    {
        SimulationManager.onSimulationStart += SimulationPhase;
        SimulationManager.onSimulationStop += PuzzlePhase;
    }

    private void OnDisable()
    {
        SimulationManager.onSimulationStart -= SimulationPhase;
        SimulationManager.onSimulationStop -= PuzzlePhase;
    }

    /*private void Start()
    {
        foreach (GameObject hand in hands)
        {
            hand.layer = 6;
        }
    }*/

    public void SimulationPhase()
    {
        foreach(GameObject hand in hands)
        {
            hand.layer = 6;
        }
        //this.gameObject.layer = 6; //SimulationMode Layer, por si pasara algo al hacer merge
    }

    public void PuzzlePhase()
    {
        foreach (GameObject hand in hands)
        {
            hand.layer = 0;
        }
        //this.gameObject.layer = 0; //Default Layer. En caso de que se necesite otra se cambia
    }
}
