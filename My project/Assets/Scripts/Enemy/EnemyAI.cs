using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private float timer;

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

        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            TurnSystem.Instance.NextTurn();
        }

    }


    private void TurnSystem_onTurnChanged(object sender, EventArgs e)
    {
        timer = 5;
    }


}
