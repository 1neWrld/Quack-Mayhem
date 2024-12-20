using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.onTurnChanged += TurnSystem_onTurnChanged;
    }

    private void Update()
    {

        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:

                break;

            case State.TakingTurn:
                timer -= Time.deltaTime;
                 if(timer <= 0f)
                 {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // All Enemies have run out of actions... End enemy turn
                        TurnSystem.Instance.NextTurn();
                    }
                 }

                break;

            case State.Busy:
                break;
        }


    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_onTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2;

        }
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {

       

        Debug.Log("Take an enemy actiion");
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {

            if (enemyUnit == null)
            {
                Debug.Log("Enemy Unit in enemyUnitListIsNull");
                continue;
            }
            Debug.Log("Take EnemyAI action");
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {    
                return true;
            }
        }
        return false;
    }


    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        LayAction layAction = enemyUnit.GetLayAction();

        GridPosition actionGridPosition = enemyUnit.GetGridPosition();
        if (!layAction.IsValidActionGridPosition(actionGridPosition))
        {
            return false;
        }

        if (!enemyUnit.TrySpendActionPointsToTakeAction(layAction))
        {
            return false;
        }

        Debug.Log("Doing a Lay Action");
        layAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
        return true;
    }

}
