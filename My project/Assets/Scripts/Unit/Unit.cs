using UnityEngine;

public class Unit : MonoBehaviour
{
  
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private LayAction layAction;
    private BaseAction[] baseActionArray;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        layAction = GetComponent<LayAction>();

        // store all the components attached to the unit that extend from BaseAction
        baseActionArray = GetComponents<BaseAction>();
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

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

}
