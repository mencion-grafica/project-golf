using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Rendering;

public class MoveLevel : MonoBehaviour
{
    public static MoveLevel Instance;

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
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        simulationInitialPosition = simulation.transform.position;
    }

    private void Start()
    {
        currentNonActivePlanets = new List<Planet>();
        currentNonActivePlanets = SimulationManager.Instance.GetNonActivePlanets();
        isSimulationStart = false;
    }

    private void Update()
    {
        if (LevelManager.Instance.IsCinematicStarted()) return;
        
        if (isMoving)
        {
            simulation.transform.position = Vector3.MoveTowards(simulation.transform.position, maxPoint.position * direction, speed * Time.deltaTime);
            //Debug.Log("Bot�n pulsado");
        }
    }

    public void MoveLevelUp()
    {
        if (LevelManager.Instance.IsCinematicStarted()) return;
        isMoving = true;
        direction = 1;
    }

    public void MoveLevelDown()
    {
        if (LevelManager.Instance.IsCinematicStarted()) return;
        isMoving = true;
        direction = -1;
    }

    public void StopMoving()
    {
        if (LevelManager.Instance.IsCinematicStarted()) return;
        isMoving = false;
    }

    public IEnumerator MoveToSpawn()
    {
        Debug.Log("Moviendome a posicion original");

        while (transform.position != simulationInitialPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, simulationInitialPosition, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Alcanzada posicion original, empezando simulacion");

        SimulationManager.Instance.StartSimulation();
        SimulationManager.Instance.ShootAsteroid();

        yield return null;
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
}