using System;
using System.Collections.Generic;
using UnityEngine;


/*
 * Script that controlls the move Action of our units
 * calculates validGridPositions the unit can move too
 * by storing all the valid gridpositions units can move to in a list 
*/

public class MoveAction : BaseAction
{

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private int maxMoveDistance = 4;


    private float stoppingDistance = .1f;

    private List<Vector3> positionList;
    private int currentPositionIndex;

    private void Update()
    {

        if (!isActive)
        {
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];

        // Desired direction from the unit pos to targetPos normalized
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        // Handles rotation by manipulating the units forward direction... Faces towards target position
        float rotateSpeed = 10f;
        transform.forward += Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

        // prevent unit from continuously pulling past the stoppingDistance
        float distance = Vector3.Distance(transform.position, targetPosition);


        // therefore if unit hasn't reached the target position movement logic executes 
        if (distance > stoppingDistance)
        {
            float moveSpeed = 7f;
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }
        else
        {
            currentPositionIndex++;

            if(currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }          
        }

    }


    // function to move unit to targeted position
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach(GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        // Nested Loop to store valid gridpositions 4 units to the left, right, up and down. 
        // based on units current position

        for(int x = -maxMoveDistance; x<= maxMoveDistance; x++)
        {
            for(int z = -maxMoveDistance; z<= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if(unitGridPosition == testGridPosition)
                {
                    // Unit is already on that position
                    continue;
                }
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //GridPosition you want to go to is occupied with another unit
                    continue;
                }

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }

                if(!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {
                    continue;
                }

                int pathFindingDistanceMultiplier = 10;
                if(Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathFindingDistanceMultiplier)
                {
                    // Path length too long
                    continue;
                }


                validGridPositionList.Add(testGridPosition);

            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {

        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {

            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,

        };
    }

}
