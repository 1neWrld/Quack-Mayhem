using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    // Scripts that shoots a ray from the main camera to the world 

    // Singleton, therefore only one isnstance of MouseWorld is in our game 
    public static MouseWorld Instance { get; private set; }

    [SerializeField] private LayerMask mousePlaneLayerMask;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one MouseWorld" + transform + Instance);
            Destroy(gameObject);
        }

        Instance = this;
    }

    //Function to recieve exact position mouse is hovering over
    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance.mousePlaneLayerMask);

        return raycastHit.point; 

    }

}
