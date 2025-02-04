using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class HandAnimController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private InputActionReference grabActionReference;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        grabActionReference.action.performed += GrabAnim;
        grabActionReference.action.canceled += ReleaseAnim;
    }

    private void OnDisable()
    {
        grabActionReference.action.performed -= GrabAnim;
        grabActionReference.action.canceled -= ReleaseAnim;
    }
    
    private void GrabAnim(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Grab");
    }
    
    private void ReleaseAnim(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Release");
    }
}