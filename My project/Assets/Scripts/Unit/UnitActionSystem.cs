using System;
using UnityEngine;

/* 
 * Allocated the script to run before default time so it's functions are read 1st before any other scripts functions
 * Script to handle the selected unit and the selected action
 * To make sure isBusy is cleared after a player done a specific action. We use Action as a delegate to notify when the action is done
*/
public class UnitActionSystem : MonoBehaviour
{

    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;




    // field of type unit to store the selectedUnit
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;


    private BaseAction selectedAction;
    private bool isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitActionSystem" + transform + Instance);
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {

        if (isBusy)
        {
            return;
        }

        /* 
         * Clear isBusy state by using delegates
         * pass through the clearbusy function as a delegate. 
         * which sets the isbusy state to false 
         * Ensures that UnitActionSystem is aware of when an action is completed
        */

        if (TryHandleUnitSelection())
        {
            return;
        }

        //When animation is complete OnLayEggAnimationComplete() invokes ClearBusy()

        HandleSelectedAction();

    }


    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            switch (selectedAction)
            {
                case MoveAction moveAction:
                    if (moveAction.IsValidActionGridPosition(mouseGridPosition))
                    {
                        SetBusy();
                        moveAction.Move(mouseGridPosition, ClearBusy);
                    }
                    moveAction.Move(mouseGridPosition, ClearBusy);
                    break;
                case LayAction layAction:
                    SetBusy();
                    selectedUnit.GetLayAction().LayEggAnimation(ClearBusy);
                    layAction.LayEggAnimation(ClearBusy);
                    break;
            }
        }
    }


    private void SetBusy()
    {
        isBusy = true;
    }

    private void ClearBusy()
    {
        isBusy = false;
    }


    /* Bool function that tries to select gameObject of type Unit
     * Returns true if selected a unit
     * Returns false if selected nothing
     * This also prevents our units from pre-moving when selected
    */
    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButton(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast that Returns true if we hit an object of type unit
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {

                // trys to get a component of a given type
                 if(raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                 {
                     SetSelectedUnit(unit);
                     return true;
                 }

            }
        }

        return false;

    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    

}
