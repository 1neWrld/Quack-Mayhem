using UnityEngine;
using TMPro;


public class GridDebugObject : MonoBehaviour
{
   
    private GridObject gridObject;
    
    [SerializeField] private TextMeshPro gridDebugText;

    public void SetGridDebugObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }



    public void Update()
    {
        gridDebugText.text = gridObject.ToString()  ;
    }

}
