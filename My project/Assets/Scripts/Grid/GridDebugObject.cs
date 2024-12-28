using UnityEngine;
using TMPro;


public class GridDebugObject : MonoBehaviour
{
   
    //Generic "object" type
    private object gridObject;
    
    [SerializeField] private TextMeshPro gridDebugText;

    public virtual void SetGridDebugObject(object gridObject)
    {
        this.gridObject = gridObject;
    }



    protected virtual void Update()
    {
        gridDebugText.text = gridObject.ToString();
    }

}
