using UnityEngine;



public class SelectedUnitVisual : MonoBehaviour
{

    [SerializeField] private Unit unit;
    [SerializeField] private GameObject unitVisualPrefab;


    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnit;
       UpdateVisual();
    }

    private void UnitActionSystem_OnSelectedUnit(object sender, System.EventArgs e)
    {
      UpdateVisual();
    }


    private void Show()
    {
        unitVisualPrefab.SetActive(true);
    }

    private void Hide()
    {
        unitVisualPrefab.SetActive(false);
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    // Built in function that is called when the object is destroyed
    // When unit is destroyed the event unsubscribes;
    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnit;
    }

}
