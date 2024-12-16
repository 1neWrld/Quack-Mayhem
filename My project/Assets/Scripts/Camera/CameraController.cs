using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.CompilerServices;

public class CameraController : MonoBehaviour
{

    /*
     * Script to handle camera movement 
     * Create 2 Vector 3 objects for the movedirection and rotation  
     * Based on WASD movement 
    */

    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
         cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }


        float moveSpeed = 10f;

        // forward and right movement will be based of the camera's rotation
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {

        Vector3 rotationVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = 1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }

        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;

    }

    private void HandleZoom()
    {
        float zoomAmount = 1f;
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomAmount;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomAmount;
        }

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed); ;
    }
}
