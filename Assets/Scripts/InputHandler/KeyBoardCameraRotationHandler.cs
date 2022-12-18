using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardCameraRotationHandler : MonoBehaviour,ICameraRotationHandler
{
    public event Action<RotationEvent> OnCameraRotateEvent;


    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            OnCameraRotateEvent?.Invoke(RotationEvent.YawNegative);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            OnCameraRotateEvent?.Invoke(RotationEvent.YawPositive);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            OnCameraRotateEvent?.Invoke(RotationEvent.RollPositive);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            OnCameraRotateEvent?.Invoke(RotationEvent.RollNegative);
        }
    }
}



