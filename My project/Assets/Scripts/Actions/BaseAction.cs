using System;
using System.Collections.Generic;
using UnityEngine;

// Abstract forbids us to create an instance of the BaseAction class
public abstract class BaseAction : MonoBehaviour
{

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    protected Unit unit;
    protected bool isActive; 
    protected int damageAmount;

    // Callback function to clear actions in order for player to do another 
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);

    }



    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);

    }

    /*
    protected void ShowDamageOnUnit(Unit targetUnit, int damageAmount)
    {



        UnitWorldUI targetUnitWorldUI = targetUnit.GetComponentInChildren<UnitWorldUI>();
        if (targetUnitWorldUI != null)
        {
            targetUnitWorldUI.ShowDamage(damageAmount);
        }
    }
    */

    public Unit GetUnit()
    {
        return unit;
    }

    // Cycles through gridPositions calculates an AIAction, returns best one
    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        // Cycle through the validGridPositions in the list and create a valid action for the enemy to take.. Add it to the enemyAiAction list
        foreach(GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }

        if(enemyAIActionList.Count > 0)
        {
           // Sort list to get the best enemyAIAction
            //List is sorted based on ActionValue
           enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);

             return enemyAIActionList[0];

        }
        else
        {
            //No possible enemyAIActions
            return null;
        }


    }

    // abstract class of type enemyAIAction that calculates a score for each action
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);



}
