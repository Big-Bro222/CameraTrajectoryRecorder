using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileCameraRotationHandler : MonoBehaviour, ICameraRotationHandler
{
    public event Action<RotationEvent> OnCameraRotateEvent;
    [SerializeField] GameObject _rotationBtnsGroup;
    LongPressButton _upBtn;
    LongPressButton _downBtn;
    LongPressButton _leftBtn;
    LongPressButton _rightBtn;

    private void Awake()
    {
        _upBtn = _rotationBtnsGroup.transform.Find("UpBtn").GetComponent<LongPressButton>();
        _downBtn = _rotationBtnsGroup.transform.Find("DownBtn").GetComponent<LongPressButton>();
        _leftBtn = _rotationBtnsGroup.transform.Find("LeftBtn").GetComponent<LongPressButton>();
        _rightBtn = _rotationBtnsGroup.transform.Find("RightBtn").GetComponent<LongPressButton>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_leftBtn.IsPressing)
        {
            OnCameraRotateEvent?.Invoke(RotationEvent.YawNegative);
        }
        else if (_rightBtn.IsPressing)
        {
            OnCameraRotateEvent?.Invoke(RotationEvent.YawPositive);
        }
        else if (_upBtn.IsPressing)
        {
            OnCameraRotateEvent?.Invoke(RotationEvent.RollPositive);
        }
        else if (_downBtn.IsPressing)
        {
            OnCameraRotateEvent?.Invoke(RotationEvent.RollNegative);
        }
    }
}
