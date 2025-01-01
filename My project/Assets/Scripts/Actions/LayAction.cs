using System;
using System.Collections.Generic;
using UnityEngine;

public class LayAction : BaseAction
{
    /*
     * Defines a delegate of type void
     * The signature of the delegate must match the one(function) you pass through
    */

    public event EventHandler OnLayEgg;
   
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
       OnLayEgg?.Invoke(this, EventArgs.Empty);
       ActionStart(onActionComplete);
    }

    public void OnLayEggAnimationComplete()
    {
        ActionComplete();
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

    public override int GetActionPointsCost()
    {
        return 1;
    }

    public override EnemyAIAction GetEnemyAIAction (GridPosition gridPosition)
    {
        return new EnemyAIAction
        {

            gridPosition = gridPosition,
            actionValue = 0,

        };
    }

}
