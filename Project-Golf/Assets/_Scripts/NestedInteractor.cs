using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class NestedInteractor : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable xrGrabInteractable;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor xrDirectInteractor;

    private ActionBasedController nestedXrController;

    private InputActionProperty defaultInputActionProperty;

    [SerializeField]
    private InputActionProperty leftInputAction;
    [SerializeField]
    private InputActionProperty rightInputAction;

    private void Awake()
    {
        xrGrabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        xrDirectInteractor = GetComponentInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor>();
        nestedXrController = GetComponentInChildren<ActionBasedController>();
        defaultInputActionProperty = nestedXrController.activateAction;
    }

    private void OnEnable()
    {
        xrGrabInteractable.selectEntered.AddListener(InjectControllerAction);
        xrGrabInteractable.selectExited.AddListener(RemoveControllerAction);
    }

    private void OnDisable()
    {
        xrGrabInteractable.selectEntered.RemoveListener(InjectControllerAction);
        xrGrabInteractable.selectExited.RemoveListener(RemoveControllerAction);
    }

    private void RemoveControllerAction(SelectExitEventArgs arg0)
    {
        nestedXrController.selectAction = defaultInputActionProperty;
    }

    [Obsolete("Obsolete")]
    private void InjectControllerAction(SelectEnterEventArgs arg0)
    {

        //var selectControllerAction = arg0.interactor.GetComponent<ActionBasedController>()?.activateAction;
        if (arg0.interactor.CompareTag("Left")) nestedXrController.selectAction = leftInputAction;
        else nestedXrController.selectAction = rightInputAction;
        //nestedXrController.selectAction = (InputActionProperty)selectControllerAction;
    }
}
