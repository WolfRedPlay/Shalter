using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensTransition : MonoBehaviour
{
    Animator animator;

    Action middleAction;
    Action finalAction;

    private void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("Transition", false);
    }


    public void StartTransition()
    {
        animator.SetBool("Transition", true);
    }

    public void SetTransitionMiddleAction(Action action)
    {
        middleAction = action;
    }
    
    public void SetTransitionFinalAction(Action action)
    {
        finalAction = action;
    }


    public void MiddleTransition()
    {
        middleAction?.Invoke();
        middleAction = null;
    }


    public void FinishTransition()
    {
        finalAction?.Invoke();
        finalAction = null;
        animator.SetBool("Transition", false);
    }

}
