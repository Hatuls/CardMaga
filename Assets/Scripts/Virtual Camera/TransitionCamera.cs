using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[Serializable]
public class TransitionCamera
{
    [SerializeField]
    private CameraIdentification _nextCamID;
    [SerializeField]
    private CinemachineBlenderSettings _cinemachineBlenderSettings;

    public TransitionCamera(CameraIdentification nextCamID, CinemachineBlenderSettings cinemachineBlenderSettings)
    {
        _nextCamID = nextCamID;
        _cinemachineBlenderSettings = cinemachineBlenderSettings;
    }

    public CameraIdentification NextCamID
    {
        get { return _nextCamID; }
    }

    public CinemachineBlenderSettings CustomBlend
    {
        get { return _cinemachineBlenderSettings; }
    }
}
