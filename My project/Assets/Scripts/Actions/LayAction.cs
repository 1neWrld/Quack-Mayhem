using System;
using UnityEngine;

public class LayAction : BaseAction
{
    /*
     * Defines a delegate of type void
     * The signature of the delegate must match the one(function) you pass through
    */

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }


    //The onActionComplete delegate allows us to pass in a function such as ClearBusy to be invoked when the animations finishes 
    public void LayEggAnimation(Action onActionComplete)
    {
        this.OnActionComplete = onActionComplete;
        //this.onLayEggComplete = onLayEggComplete;
        animator.SetTrigger("Lay");
        isActive = false;

    }

    public void OnLayEggAnimationComplete()
    {
        isActive = false;

        OnActionComplete();
    }

    public override string GetActionName()
    {
        return "LayEgg";
    }

}
