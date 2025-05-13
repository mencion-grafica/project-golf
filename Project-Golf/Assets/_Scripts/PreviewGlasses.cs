using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewGlasses : MonoBehaviour
{
    [SerializeField] private TransitionAnimator transition;
    [SerializeField] private OrbitDebugDisplay orbitDebugDisplay;

    public void OnGlassesOn()
    {
        transition.PutOnGlasses();
        orbitDebugDisplay.SetDisplay(true);
    }
    
    public void OnGlassesOff()
    {
        transition.TakeOffGlasses();
        orbitDebugDisplay.SetDisplay(false);
    }
}
