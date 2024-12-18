using System;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler OnAnyActionPointChanged;

    private GridPosition gridPosition;
    private MoveAction moveAction;
    private LayAction layAction;
    private BaseAction[] baseActionArray;

    private int actionPoints = 2;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        layAction = GetComponent<LayAction>();

        // store all the components attached to the unit that extend from BaseAction
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition( gridPosition, this);

        TurnSystem.Instance.onTurnChanged += TurnSystem_onTurnChanged;

    }

    private void Update()
    {


       GridPosition newgridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if(newgridPosition != gridPosition)
        {
            // Unit has moved from initial position 
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newgridPosition);
            gridPosition = newgridPosition;

        }

    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public LayAction GetLayAction()
    {
        return layAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);

    }

    public int GetActionPoints()
    {
        return actionPoints;
    }


    private void TurnSystem_onTurnChanged(object sender, EventArgs e)
    {
        actionPoints = ACTION_POINTS_MAX;

        OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
    }

}
