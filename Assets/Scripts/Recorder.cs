using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static TimerFactory;

[System.Serializable]
public struct CameraStateData
{
    public float[] Vector;
    public float[] Quaternion;
}

public class Recorder : MonoBehaviour
{
    [SerializeField] Transform _recordTarget;
    [SerializeField] Transform _lookAtTarget;
    [SerializeField] long _maxRecordingTime=2*60000;
    private int timeInterval = Global.RecordTimerInterval;
    private int _replayTrajectoryIndex = 0;
    private Timer _dataStoreTimer;
    private List<Vector3> _trajectoryPosList;
    private List<Quaternion> _trajectoryRotList;
    private void Awake()
    {
        _trajectoryPosList = new List<Vector3>(10);
        _trajectoryRotList = new List<Quaternion>(10);
    }

    public void StartRecording()
    {
        if (_trajectoryPosList.Count != 0)
        {
            _trajectoryPosList.Clear();
            _trajectoryRotList.Clear();
        }
        Debug.Log("<color=#FFFF00>" + "StartRecording" + "</color>");
        _dataStoreTimer = TimerFactory.Instance.CreateLoopTimer(timeInterval, SavePos, true);
        _dataStoreTimer.Trigger();
    }

    public void SaveToFile()
    {
        if (_trajectoryPosList.Count != 0)
        {
            CameraStateData[] trajectoryArray = new CameraStateData[_trajectoryPosList.Count];
            for (int i = 0; i < _trajectoryPosList.Count; i++)
            {
                float[] Vector3Array = new float[3] { _trajectoryPosList[i].x, _trajectoryPosList[i].y, _trajectoryPosList[i].z };
                float[] QuaternionArray = new float[4] {  _trajectoryRotList[i].x,_trajectoryRotList[i].y,_trajectoryRotList[i].z , _trajectoryRotList[i].w};
                trajectoryArray[i] = new CameraStateData() { Vector=Vector3Array,Quaternion=QuaternionArray };
            }
            try
            {
                FileIO.SaveDataToFile(Global.SaveDataFolder, DateTime.Now.ToFileTimeUtc().ToString(), Global.SaveDataExtension, trajectoryArray);
                Debug.Log("<color=#FFFF00>" + "Save to file:" + DateTime.Now.ToFileTimeUtc().ToString() + "</color>");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        else
        {
            Debug.LogError("Save failed: no trajectroy points found");
        }
    }

    public void Preview()
    {
        if (_trajectoryPosList.Count == 0)
        {
            Debug.LogError("No existing record");
            return;
        }
        Debug.Log("<color=#FFFF00>" + "StartPreview" + "</color>");
        Timer _replayTimer = TimerFactory.Instance.CreateLoopTimer(timeInterval, Replay, true);
        _replayTimer.Trigger();
    }

    public void EndRecording()
    {
        if (_dataStoreTimer == null)
        {
            Debug.LogError("Recorder not enabled, Press record Button first");
            return;
        }
        Debug.Log("<color=#FFFF00>" + "EndRecording" + "</color>");
        _dataStoreTimer.Clear();
        _dataStoreTimer = null;
    }

    #region private Methods
    private void SavePos(Timer attachedTimer)
    {
        Debug.Log("Saving pos...");
        _trajectoryPosList.Add(_recordTarget.position);
        _trajectoryRotList.Add(_recordTarget.rotation);
        //Auto stop
        if (attachedTimer.executedTime >= _maxRecordingTime)
        {
            EndRecording();
        }
    }

    private void Replay(Timer attachedTimer)
    {
        if (_replayTrajectoryIndex == _trajectoryPosList.Count-1)
        {
            Debug.Log("<color=#FFFF00>" + "End Preview" + "</color>");
            attachedTimer.Clear();
            _replayTrajectoryIndex = 0;
            return;
        }
        Debug.Log("replay ...");
        _recordTarget.position = _trajectoryPosList[_replayTrajectoryIndex];
        _recordTarget.rotation= _trajectoryRotList[_replayTrajectoryIndex];
        _replayTrajectoryIndex++;
    }
    #endregion
}
