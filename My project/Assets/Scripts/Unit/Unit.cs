using UnityEngine;

public class Unit : MonoBehaviour
{
  
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private LayAction layAction;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        layAction = GetComponent<LayAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition( gridPosition, this);
    }

    private void Update()
    {


       GridPosition newgridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if(newgridPosition != gridPosition)
        {
            // Unit has moved from initial position 
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newgridPosition);
            gridPosition = newgridPosition;

        }

    }
    
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public LayAction GetLayAction()
    {
        return layAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

}
