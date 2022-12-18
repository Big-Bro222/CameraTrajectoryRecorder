using System;

public interface ICameraRotationHandler
{
    public event Action<RotationEvent> OnCameraRotateEvent;

}
