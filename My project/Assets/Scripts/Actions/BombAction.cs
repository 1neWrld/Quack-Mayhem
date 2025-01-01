using System;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : BaseAction
{

    [SerializeField] private Transform eggBombProjectilePrefab;

    private int maxThrowDistance = 7;
    private Unit targetUnit;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

     
    }

    public override string GetActionName()
    {
        return "Bomb";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);


        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
        
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        // Nested Loop to store valid gridpositions 4 units to the left, right, up and down. 
        // based on units current position

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                // Total distance to point
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                //compare testDistance to maxShoot distance
                if (testDistance > maxThrowDistance)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        Transform eggBombTransform = Instantiate(eggBombProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        EggBombProjectile eggBombProjectile= eggBombTransform.GetComponent<EggBombProjectile>();
        eggBombProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);

        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }

    /*
    public void GetShowDamageOnUnit(Unit targetUnit, int damageAmount)
    {
        this.targetUnit = targetUnit;
        ShowDamageOnUnit(targetUnit, damageAmount);
    }
    */
}
