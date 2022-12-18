using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Transform _lookAtTarget;
    [SerializeField] GameObject _cameraRoationInputHandlerGo;

    private ICameraRotationHandler _cameraRotationHandler;
    private float _speed = 50f;
    private Vector3 _lookAtTargetPos;


    private void Awake()
    {
        _cameraRotationHandler = _cameraRoationInputHandlerGo.GetComponent<ICameraRotationHandler>();
        _cameraRotationHandler.OnCameraRotateEvent += HandleInput;
        _lookAtTargetPos = _lookAtTarget.position;
    }
    /// <summary>
    /// Manage Input data and perform camera rotation
    /// </summary>
    /// <param name="rotationEvent"></param>
    private void HandleInput(RotationEvent rotationEvent)
    {
        switch (rotationEvent)
        {
            case RotationEvent.YawPositive:
                transform.RotateAround(_lookAtTarget.position, -Vector3.up, _speed * Time.deltaTime);
                break;
            case RotationEvent.YawNegative:
                transform.RotateAround(_lookAtTarget.position, Vector3.up, _speed * Time.deltaTime);
                break;
            case RotationEvent.RollPositive:
                transform.RotateAround(_lookAtTarget.position, transform.right, _speed * Time.deltaTime);
                break;
            case RotationEvent.RollNegative:
                transform.RotateAround(_lookAtTarget.position, -transform.right, _speed * Time.deltaTime);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (_lookAtTargetPos != _lookAtTarget.position)
        {
            transform.LookAt(_lookAtTarget);
            _lookAtTargetPos = _lookAtTarget.position;
        }
    }
}
