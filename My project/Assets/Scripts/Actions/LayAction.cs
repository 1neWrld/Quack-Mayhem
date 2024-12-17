using System;
using System.Collections.Generic;
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

    // The gridPosition parameter is never used for this action
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
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


    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition,
        };
    }

}
