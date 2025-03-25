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
        childCount = transform.childCount + 1;
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
    
    public void SetActive(bool active)
    {
        _isActive = false;
        if (!active) return;
        if (transform.childCount != childCount) return; 
        Vector3 pos = transform.GetChild(childCount - 1).position;
        List<Vector3> planetPoints = GetPlanetPoints();
        if (planetPoints.Count == 0) return;
        _isActive = planetPoints.Contains(pos);
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
