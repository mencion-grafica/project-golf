using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{
    [SerializeField] private SOLevelData.PlanetType type;
    [SerializeField, Range(0.0f, 100000.0f)] private float mass = 1.0f;
    private bool _isActive = false;
    
    private Transform _center;
    private Rigidbody _rigidbody;
    private XRGrabInteractable _grabInteractable;
    private Collider collider;
    private int childCount = 0;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _center = transform;
        _rigidbody.mass = mass;
        _grabInteractable = GetComponent<XRGrabInteractable>();
        //_grabInteractable.useDynamicAttach = false;
        collider = GetComponent<Collider>();
        _isActive = false;
        childCount = GetChildCount() + 1;
    }

    private int GetChildCount()
    {
        int count = 0;
        foreach (Transform child in transform) count++;
        return count;
    }
    
    public void IsDynamicAttach()
    {
        IXRInteractor interactor = _grabInteractable.GetOldestInteractorSelecting();
        if (interactor.interactionLayers == InteractionLayerMask.GetMask("PlanetSocket"))
        {
            _grabInteractable.attachTransform = null;
            _grabInteractable.useDynamicAttach = false;
        }
        else
        {
            _grabInteractable.useDynamicAttach = true;
        }
    }

    private List<Vector3> GetPlanetPoints()
    {
        List<GameObject> planetPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlanetPoint"));
        List<Vector3> pointsPos = new List<Vector3>();
        foreach (GameObject planetPoint in planetPoints) pointsPos.Add(planetPoint.transform.position);
        return pointsPos;
    }
    
    private string GetAttachName(string name)
    {
        int start = name.IndexOf("[");
        int end = name.IndexOf("]");
        return name.Substring(start + 1, end - start - 1);
    }
    
    public void SetActive(bool active)
    {
        _isActive = false;
        if (!active) return;
        string attachName = GetAttachName(transform.GetChild(childCount - 1).gameObject.name);
        _isActive = attachName == "PlanetPoint";
        Debug.Log(attachName);
        Debug.Log("Planet Active: " + _isActive);
    }
    
    public bool IsActive()
    {
        return _isActive;
    }
    
    public float GetRadius()
    {
        return transform.localScale.x * 0.5f;
    }
    
    public float GetMass()
    {
        return mass;
    }
    
    public SOLevelData.PlanetType GetPlanetType()
    {
        return type;
    }
    
    public void SetPlanetType(SOLevelData.PlanetType type)
    {
        this.type = type;
    }

    public void SetMass(float mass)
    {
        this.mass = mass;
    }
    
    public Vector3 GetPosition()
    {
        if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
        return _rigidbody.position;
    }
}
