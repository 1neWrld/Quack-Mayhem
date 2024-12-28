using System;
using System.Globalization;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler OnAnyActionPointChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;


    private int actionPoints = 2;

    [SerializeField] private bool isEnemy;
    [SerializeField] Transform ghostPrefab;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        // store all the components attached to the unit that extend from BaseAction
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition( gridPosition, this);

        TurnSystem.Instance.onTurnChanged += TurnSystem_onTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

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

    // Using Generics we return an action that extends from BaseAction
    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in baseActionArray)
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }
        }

        return null;

    }


    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
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
        //Resets action points for enemy and ally units when its their turn 
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {    
            actionPoints = ACTION_POINTS_MAX;

            OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.TakeDamage(damageAmount);
    }
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(this.gameObject);

        // Instantiate ghost prefab at the exact position and rotation as the killed unit
        Instantiate(ghostPrefab, transform.position, transform.rotation);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);

    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

}
