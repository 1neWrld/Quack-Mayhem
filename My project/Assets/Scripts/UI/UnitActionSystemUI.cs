using System;
using UnityEngine;
using UnityEngine.UI;


public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;


    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged; 
        CreateUnitActionButtons();
    }

    // Method to insantiate Action button prefabs onto the screen 
    private void CreateUnitActionButtons()
    {

        foreach(Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach( BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();

            actionButtonUI.SetBaseAction(baseAction);

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
    }

}
