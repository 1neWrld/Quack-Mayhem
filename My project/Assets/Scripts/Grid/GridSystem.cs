using System;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystem<TGridObject> 
{
    /*
     * GridSystem script handles the making of our grid system in our game world
     * Convert gridposition to world position and vice versa 
     * Use cellsize field to multiply our gridPosition in order to create bigger sized cells in the world on the grid
     * GridPosition = the logical coordinates of our cells 
     * WorldPosition = the logical coords in the physical game world 
     * Create GridObjects that will be instantiated into every gridPosition in our grid.
    */


    private int height;
    private int width;
    private float  cellSize;

    // Store our gridObjects into our 2d Array
    private TGridObject[,] gridObjectArray;

    // Constructor to set up the grid dimensions
    // Pass a delegate of type TGridObject as an argument to create gridObjects 
    public GridSystem(int height, int width, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        // size of our array is determined by the width and height
        gridObjectArray = new TGridObject[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);

                //for each x and z position we create a new gridObject instance
                gridObjectArray[x,z] = createGridObject(this, gridPosition);
            }
        }
    }

    // returns physical gridPosition
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    

    // returns logical gridposition 
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
            );
    }
    
    // Function to instantiate a gridDebugObject in each GridObject
    public void CreateDebugObject(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // new gridPosition instance in each gridObject
                GridPosition gridPosition = new GridPosition(x,z);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition),Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridDebugObject(GetGridObject(gridPosition));

                //Debug.Log($"Instantiated at {gridPosition}");

            }
        }
    }

    // Function to return the gridObject 2D Array takes a parameter of type gridPositon
    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    // return grid positions that are within the width and height of the grid + 'x' & 'z' positions must be greater or equal to zero 
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && 
               gridPosition.z >= 0 &&
               gridPosition.x < width && 
               gridPosition.z < height;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

}
