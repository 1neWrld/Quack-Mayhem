using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;


public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtonUIList;


    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged; 
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointChanged;


        TurnSystem.Instance.onTurnChanged += TurnSystem_onTurnChanged;

        UpdateActionPoints();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    // Method to insantiate Action button prefabs onto the screen 
    private void CreateUnitActionButtons()
    {

        foreach(Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach( BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();

            actionButtonUI.SetBaseAction(baseAction);

            // Stores each action button into list
            actionButtonUIList.Add(actionButtonUI);

        }

    }

    /*
     * To update action button prefabs when a new duck is selected 
     * For example all ducks have a move and layegg action.
     * The buttons will update if we remove one action from one of the ducks
     * So that specific duck will only have the move action if we remove its action to layeggs
    */
    private void UnitActionSystem_OnSelectedUnitChanged(object sender , EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender , EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnActionStarted(object sender , EventArgs e)
    {
        UpdateActionPoints();
    }

    private void TurnSystem_onTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    // Updates selectedAction visual based on if the action button is the selected
    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
    }

}
