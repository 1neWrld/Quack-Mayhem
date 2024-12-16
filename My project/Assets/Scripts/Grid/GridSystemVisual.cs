using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{

    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] Transform gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one GridSystemVisual" + transform + Instance);
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {

        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
            ];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++ )
        {
            for (int z = 0; z < LevelGrid.Instance.GetWidth(); z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);

                Transform gridSystemVisualSingleTransform = 
                    Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingleArray[x,z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();

            }
        }
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void HideAllGridPositions()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetWidth(); z++)
            { 
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    public void ShowAllGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach(GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x,gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPositions();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        ShowAllGridPositionList(selectedUnit.GetMoveAction().GetValidActionGridPositionList());
    }

}
