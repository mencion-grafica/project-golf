using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Rendering;

public class MoveLevel : MonoBehaviour
{
    [SerializeField]
    GameObject simulation;
    [SerializeField]
    GameObject planets;
    [SerializeField]
    Transform maxPoint;
    [SerializeField]
    float speed;

    private Vector3 simulationInitialPosition;
    private bool isMoving;
    private float direction;
    private List<Planet> currentNonActivePlanets;
    private bool isSimulationStart;

    private void Awake()
    {
        simulationInitialPosition = simulation.transform.position;
    }

    private void Start()
    {
        currentNonActivePlanets = new List<Planet>();
        currentNonActivePlanets = SimulationManager.Instance.GetNonActivePlanets();
    }

    private void Update()
    {
        if (isMoving)
        {
            simulation.transform.position = Vector3.MoveTowards(simulation.transform.position, maxPoint.position * direction, speed * Time.deltaTime);
            //Debug.Log("Botón pulsado");
        }
    }

    public void MoveLevelUp()
    {
        isMoving = true;
        direction = 1;
    }

    public void MoveLevelDown()
    {
        isMoving = true;
        direction = -1;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    public void ReturnToSpawn()
    {

    }

    public void OrphanNonActivePlanets()
    {
        Debug.Log("Vamos a matar padres");
        foreach(Planet nonActive in currentNonActivePlanets)
        {
            Debug.Log(nonActive.gameObject.name);
            nonActive.transform.parent = null;
        }
        Debug.Log(currentNonActivePlanets.Count);
    }

    public void AdoptNonActivePlanets()
    {
        foreach(Planet nonActive in currentNonActivePlanets)
        {
            Debug.Log(nonActive.gameObject.name);
            nonActive.transform.parent = simulation.transform;
        }
    }

    /*private IEnumerator MoveToSpawn()
    {

    }*/
}