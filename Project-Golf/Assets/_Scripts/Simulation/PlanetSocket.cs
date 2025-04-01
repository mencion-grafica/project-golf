using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlanetSocket : MonoBehaviour
{
    private XRSocketInteractor interactor;

    // Start is called before the first frame update
    void Start()
    {
        interactor = GetComponent<XRSocketInteractor>();
    }

    public void TeleportToCenter()
    {
        interactor.interactablesSelected[0].transform.position = transform.position;
    }
}
