
using System.Collections.Generic;
using UnityEngine;

/*
 * Instantiated in every gridPosition
 * keeps track of any unit etc that is on top of it  
 * What position it lies, on the grid
 * What gridSystem made that gridObject
*/

public class GridObject
{
    private List<Unit> unitList;
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;

        unitList = new List<Unit>();

    }

    public override string ToString()
    {
        string unitString = ""; 
        foreach (Unit unit in unitList) {
            unitString += unit + "\n";
        }
        return gridPosition.ToString()+ "\n" + unitString ;
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList; 
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;  
    }

    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return unitList[0];
        }
        else
        {
            return null;
        }
    }

}
