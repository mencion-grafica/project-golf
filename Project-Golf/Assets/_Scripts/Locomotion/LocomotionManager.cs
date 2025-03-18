using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionManager : MonoBehaviour
{
    [SerializeField] private ContinuousMoveProviderBase continuousMoveProvider;

    [SerializeField] private InputActionReference toggleTeleportAction;
    
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor teleportRayInteractor;

    // Start is called before the first frame update
    void Start()
    {
        continuousMoveProvider = this.GetComponent<ContinuousMoveProviderBase>();
        teleportRayInteractor.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        toggleTeleportAction.action.started += EnableTeleport;
        toggleTeleportAction.action.canceled += DisableTeleport;
    }

    private void OnDisable()
    {
        toggleTeleportAction.action.started -= EnableTeleport;
        toggleTeleportAction.action.canceled -= DisableTeleport;
    }

    private void EnableTeleport(InputAction.CallbackContext obj)
    {
        teleportRayInteractor.gameObject.SetActive(true);
    }

    private void DisableTeleport(InputAction.CallbackContext obj)
    {
        teleportRayInteractor.gameObject.SetActive(false);
    }

    public void ToggleMoveProvider(bool enabled)
    {
        continuousMoveProvider.enabled = enabled;
    }
}
