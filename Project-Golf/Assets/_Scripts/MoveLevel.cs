using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLevel : MonoBehaviour
{
    [SerializeField]
    GameObject simulation;
    [SerializeField]
    Transform maxPoint;
    [SerializeField]
    float speed;

    private Vector3 simulationInitialPosition;
    private bool isMoving;
    private float direction;

    private void Awake()
    {
        simulationInitialPosition = simulation.transform.position;
    }

    private void Update()
    {
        if(isMoving)
        {
            simulation.transform.position = Vector3.MoveTowards(simulation.transform.position, maxPoint.position * direction, speed * Time.deltaTime);
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
}
