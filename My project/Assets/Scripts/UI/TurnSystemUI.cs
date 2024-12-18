using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class TurnSystemUI : MonoBehaviour
{

    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberText;


    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.onTurnChanged += TurnSystem_onTurnChanged;

        UpdateTurnText();

    }

    private void TurnSystem_onTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
    }

    public void UpdateTurnText()
    {
        turnNumberText.text = "Turn " + TurnSystem.Instance.GetTurnNumber();
    }


}
