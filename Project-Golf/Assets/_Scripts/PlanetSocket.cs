using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlanetSocket : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor interactor;

    // Start is called before the first frame update
    void Start()
    {
        interactor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    }

    public void TeleportToCenter()
    {
        interactor.interactablesSelected[0].transform.position = transform.position;
    }
}
