using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnimator : MonoBehaviour
{
    public Animator animator;
    
    public void StartTransition()
    {
        animator.SetTrigger("FadeOut");
    }

    public void PutOnGlasses()
    {
        animator.SetBool("GogglesOn", true);
    }
    
    public void TakeOffGlasses()
    {
        animator.SetBool("GogglesOn", false);
    }
}
