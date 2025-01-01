using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    public static event EventHandler OnDamageTaken;

    public event EventHandler<OnShootEventArgs> OnShoot;


    //Eventargs class 
    //Add extra data to our event
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;

    private State state;
    private int maxShootDistance = 7;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;


    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:

                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized ;

                float rotateSpeed = 10f;
                transform.forward += Vector3.Slerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break; 
            
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;

            case State.Cooloff:
               
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }

    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
              ActionComplete();
                break;
        }


    }


    private void Shoot()
    {

        OnAnyShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        //Pass through required data for the event 
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        //Set the damageAmount to a random range between 10 and 40
        damageAmount = UnityEngine.Random.Range(25, 41);

        targetUnit.Damage(damageAmount);

        //ShowDamageOnUnit(targetUnit,damageAmount);

        // Access the targets UnitWorldUI component to activate popUp
        UnitWorldUI targetUnitWorldUI = targetUnit.GetComponentInChildren<UnitWorldUI>();
        if (targetUnitWorldUI != null)
        {
           targetUnitWorldUI.ShowDamage(damageAmount);
        }
        
        OnDamageTaken?.Invoke(this, EventArgs.Empty);
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Nested Loop to store valid gridpositions 4 units to the left, right, up and down. 
        // based on units current position

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
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
                if(testDistance > maxShootDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //GridPosition has no enemy unit to shoot at
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    //Both teams are on the same team
                    continue;
                }

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                float unitShoulderHeight = 1f;

                if(Physics.Raycast(
                    unitWorldPosition + Vector3.up * unitShoulderHeight, 
                    shootDir, 
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                    obstaclesLayerMask))
                {
                    // Blocked by obstacle
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        canShootBullet = true;

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        ActionStart(onActionComplete);

    }

    public int GetDamageAmount()
    {
        return damageAmount;
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {

       Unit targetUnit =  LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
      

        return new EnemyAIAction
        {

            gridPosition = gridPosition,

            //Shooting a unit with half health will give a higher actionValue
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized())*100),

        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

}
