using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimerFactory;
using UnityEngine.UI;
using TMPro;

public class LoadManager : MonoBehaviour
{


    [SerializeField] Transform _recordTarget;
    [SerializeField] Transform _lookAtTarget;
    [SerializeField] TMP_Dropdown _dropDown;

    [Header("Gizmos")]
    [Range(1, 200)]
    [SerializeField] int GizmosInterval = 3;

    private List<Vector3> _trajectoryPosList = new List<Vector3>();
    private List<Quaternion> _trajectoryRotList = new List<Quaternion>();
    private string _currentOption;
    private string[] _fileNames;
    private int _timeInterval = Global.RecordTimerInterval;
    private int _trajectoryIndex = 0;
    

    private void Awake()
    {
        SetSaveDataListToUI();
    }

    private void SetSaveDataListToUI()
    {
        List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
        _fileNames = FileIO.GetDataListInfo(Global.SaveDataFolder, Global.SaveDataExtension);
        for (int i = 0; i < _fileNames.Length; i++)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(_fileNames[i]);
            optionDatas.Add(optionData);
        }
        _dropDown.AddOptions(optionDatas);
        _currentOption = _fileNames[_dropDown.value];
        _dropDown.onValueChanged.AddListener(SelectFile);
    }

    public void LoadFromFile()
    {
        Debug.Log("Loading: " + _currentOption);
        CameraStateData[] trajectoryArray = FileIO.LoadDataFromFile(Global.SaveDataFolder, _currentOption, Global.SaveDataExtension);
        _trajectoryPosList.Clear();
        _trajectoryRotList.Clear();
        foreach (CameraStateData point in trajectoryArray)
        {
            Vector3 trajectroyPos = new Vector3(point.Vector[0], point.Vector[1], point.Vector[2]);
            _trajectoryPosList.Add(trajectroyPos);

            Quaternion trajectroyRot = new Quaternion(point.Quaternion[0], point.Quaternion[1], point.Quaternion[2], point.Quaternion[3]);
            _trajectoryRotList.Add(trajectroyRot);
        }
    }

    public void Play()
    {
        Timer _replayTimer = TimerFactory.Instance.CreateLoopTimer(_timeInterval, Replay, true);
        _replayTimer.Trigger();
    }

    private void Replay(Timer attachedTimer)
    {
        if (_trajectoryIndex == _trajectoryPosList.Count - 1)
        {
            Debug.Log("<color=#FFFF00>" + "End play" + "</color>");
            attachedTimer.Clear();
            _trajectoryIndex = 0;
            return;
        }
        Debug.Log("play ...");
        _recordTarget.position = _trajectoryPosList[_trajectoryIndex];
        _recordTarget.rotation = _trajectoryRotList[_trajectoryIndex];
        _trajectoryIndex++;
    }

    private void SelectFile(int option)
    {
        _currentOption = _fileNames[_dropDown.value];
    }
    private void OnDrawGizmos()
    {
        if (_trajectoryPosList.Count != 0)
        {
            Vector3 from = _trajectoryPosList[0];
            int GizmoIndex = 0;
            Color currentColor = Color.red;
            while(GizmoIndex< _trajectoryPosList.Count- GizmosInterval)
            {
                GizmoIndex += GizmosInterval;
                Vector3 to = _trajectoryPosList[GizmoIndex];
                currentColor = Color.Lerp(currentColor, Color.green,0.01f);
                Gizmos.color = currentColor;
                Gizmos.DrawLine(from, to);
                from = to;
            }
        }
    }
}
