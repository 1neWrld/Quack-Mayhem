using System;
using UnityEngine;

// Allocated the script to run before default time so it's functions are read 1st before any other scripts functions
public class UnitActionSystem : MonoBehaviour
{

    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnit;


    // Script to handle the selected unit and the selected action

    // field of type unit to store the selectedUnit
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

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
        */

        if (Input.GetMouseButtonDown(0))
        {
            if(TryHandleUnitSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                selectedUnit.GetMoveAction().Move(mouseGridPosition);
            }
          
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetBusy();
            selectedUnit.GetLayAction().LayEggAnimation(ClearBusy);
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

        return false;

    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnit?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

}
