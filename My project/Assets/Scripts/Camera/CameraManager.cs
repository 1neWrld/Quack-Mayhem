using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private GameObject actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;   
        BaseAction.OnAnyActionCompleted+= BaseAction_OnAnyActionCompleted; 
        
        HideActionCamera();

    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    // As this is a generic event, we use a switch to execute logic for a specific action
    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:

                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                // Shoulder height of unit camera is positioned to
                Vector3 cameraCharacterHeight = Vector3.up * 1.4f;

                // direction from shooter unit to targetUnit normalized
                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

               // offset/rotate the camera on  unit shoulder height 
                float shoulderOffsetAmount = 1f;
                Vector3 shoulderOffset = Quaternion.Euler(0,90,0) * shootDir * shoulderOffsetAmount;

                /*
                 * Camera positioning
                 * The camera is set on the shooter unit
                 * set on a height(Y) of cameraCharcterHeight slightly to the right (shoulderOffset)
                 * shootDir multyplied by -1.5f to move it slighlt backwards 
                */
                Vector3 actionCameraPoition = shooterUnit.GetWorldPosition() + 
                          cameraCharacterHeight + 
                          shoulderOffset + 
                          (shootDir * -1.5f);

                // set the action camera transform position to the actionCameraPosition
                actionCameraGameObject.transform.position = actionCameraPoition;
                //rotates the camera to face the target position + height
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                ShowActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

}
