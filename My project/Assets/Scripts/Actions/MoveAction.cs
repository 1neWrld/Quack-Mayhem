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

    private const string IS_WALKING = "IsMoving";
    private Animator animator;

    [SerializeField] private int maxMoveDistance = 4;


    private float stoppingDistance = .1f;

    private Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();

        // Accesses the animator component within the child DuckVisual gameObject 
        animator = GetComponentInChildren<Animator>();
        // units stay exatly where they are if not selected/commanded to.
        targetPosition = transform.position;
    }

    private void Update()
    {

        if (!isActive)
        {
            return;
        }


        // Desired direction from the unit pos to targetPos normalized
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        // prevent unit from continuously pulling past the stoppingDistance
        float distance = Vector3.Distance(transform.position, targetPosition);


        // therefore if unit hasn't reached the target position movement logic executes 
        if (distance > stoppingDistance)
        {
            float moveSpeed = 7f;
            transform.position += moveDir * Time.deltaTime * moveSpeed;

            animator.SetBool(IS_WALKING, true);
        }
        else
        {
            animator.SetBool(IS_WALKING, false);
            isActive = false;

            OnActionComplete();

        }

        // Handles rotation by manipulating the units forward direction... Faces towards target position
        float rotateSpeed = 10f;
        transform.forward += Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    }


    // function to move object to targeted position
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.OnActionComplete= onActionComplete;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
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
                
                validGridPositionList.Add(testGridPosition);

            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

}
