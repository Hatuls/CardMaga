using Cinemachine;
using UnityEngine;

[System.Serializable]
public class CameraDetails
{
    public CameraIdentification[] LeftCamera;
    public CameraIdentification[] RightCamera;
    public CinemachineBlenderSettings[] CinemachineBlenderSettings;

    public TransitionCamera GetTransitionCamera(bool IsPlayer)
    {
        TransitionCamera transitionCamera = null;

        if (IsPlayer)
            transitionCamera = new TransitionCamera(CameraIndex(LeftCamera), BlenderIndex(CinemachineBlenderSettings));

        else
            transitionCamera = new TransitionCamera(CameraIndex(RightCamera), BlenderIndex(CinemachineBlenderSettings));

        return transitionCamera;
    }

    private CameraIdentification CameraIndex(CameraIdentification[] cameraSide)
    {
        int camIndex = Random.Range(0, cameraSide.Length);
        return cameraSide[camIndex];
    }

    private CinemachineBlenderSettings BlenderIndex(CinemachineBlenderSettings[] blenders)
    {
        int blenderIndex = Random.Range(0, blenders.Length);
        return blenders[blenderIndex];
    }
    public bool CheckCameraDetails(bool isPlayer)
    {
        bool isValid = true;

        if (isPlayer)       //Left Camera Check
        {
            if (LeftCamera == null || LeftCamera.Length == 0)
                isValid = false;

        }
        else
        {
            //Right Camera Check
            if (RightCamera == null || RightCamera.Length == 0)
                isValid = false;
        }



        //CinemachineBlenderSettings
        if (isValid && (CinemachineBlenderSettings == null || CinemachineBlenderSettings.Length == 0))
        {
            isValid = false;
        }
        return isValid;
    }
    static public bool CheckCameraDetails(CameraDetails cameraDetails)
    {
        bool noErrors = true;

        if (cameraDetails == null)
        {
            Debug.LogError("The CameraDetails is null!");
            if (noErrors)
                noErrors = false;

            return noErrors;
        }

        //Left Camera Check
        if (cameraDetails.LeftCamera == null)
        {
            Debug.LogError("The LeftCamera Array is null!");
            if (noErrors)
                noErrors = false;
        }

        else if (cameraDetails.LeftCamera.Length == 0)
            Debug.LogError("The RightCamera Array Length is 0!");


        //Right Camera Check
        if (cameraDetails.RightCamera == null)
        {
            Debug.LogError("The RightCamera Array is null!");
            if (noErrors)
                noErrors = false;
        }

        else if (cameraDetails.RightCamera.Length == 0)
            Debug.LogError("The RightCamera Array Length is 0!");


        //CinemachineBlenderSettings
        if (cameraDetails.CinemachineBlenderSettings == null)
        {
            Debug.LogError("The RightCamera Array is null!");
            if (noErrors)
                noErrors = false;
        }

        else if (cameraDetails.CinemachineBlenderSettings.Length == 0)
        {
            Debug.LogError("The CinemachineBlenderSettings Array Length is 0!");
            if (noErrors)
                noErrors = false;
        }

        return noErrors;
    }
}
