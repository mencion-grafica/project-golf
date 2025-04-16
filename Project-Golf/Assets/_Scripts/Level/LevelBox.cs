using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LevelBox : MonoBehaviour
{
    [SerializeField]
    private XRSocketInteractor ownInteractor;

    public void OnCardInserted()
    {
        LevelManager.Instance.LoadLevel(ownInteractor.GetOldestInteractableSelected().transform.GetComponent<LevelCard>()?.GetLevelData());
    }
}
