using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction:
                PositionCamera(sender as ShootAction);
                ShowActionCamera();
                break;
        }
    }

    private void PositionCamera(ShootAction shootAction)
    {
        Unit shooterUnit = shootAction.GetUnit();
        Unit targetUnit = shootAction.GetTargetUnit();
        Vector3 shootDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

        float shoulderOffsetAmount = 0.5f;
        Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDirection * shoulderOffsetAmount;
        Vector3 cameraCharacterHeight = Vector3.up * 1.7f;

        Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + 
                                       cameraCharacterHeight + 
                                       shoulderOffset + 
                                       (shootDirection * -1);
        actionCameraGameObject.transform.position = actionCameraPosition;
        actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction: 
                HideActionCamera();
                break;
        }
    }
    
    private void ShowActionCamera()
    {
        actionCameraGameObject.gameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.gameObject.SetActive(false);
    }
}
