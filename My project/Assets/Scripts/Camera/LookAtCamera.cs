using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    [SerializeField] private bool invert;

    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    // we are telling this transform to look at the camera after every update
    private void LateUpdate()
    {
        if (invert)
        {
           // if it's inverted. Get the direction of the camera and look the opposite direction
            
           Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCamera* -1);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }
}
