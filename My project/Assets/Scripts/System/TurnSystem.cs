using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{

    public static TurnSystem Instance { get; private set; }

    public event EventHandler onTurnChanged;

    private int turnNumber = 1;
    private bool isPlayerTurn = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one TurnSystem" + transform + Instance);
            Destroy(gameObject);
        }

        Instance = this;
    }

    
    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;

        onTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    
    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

}
