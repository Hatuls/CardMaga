using CardMaga.Trackers;
using ReiTools.TokenMachine;
using System;
using UnityEngine;

public class TutorialObjectActivationHandler : MonoBehaviour
{
    [SerializeField] TrackerID _tracker;
    private IDisposable _token;

    public void DisableObject(ITokenReciever tokenReciever)
    {
        gameObject.SetActive(true);
        _token = tokenReciever.GetToken();
        TrackerHandler.GetTracker(_tracker).gameObject.SetActive(false);
        ReleaseToken();
    }

    public void ActivateObject(ITokenReciever tokenReciever)
    {
        gameObject.SetActive(true);
        _token = tokenReciever.GetToken();
        TrackerHandler.GetTracker(_tracker).gameObject.SetActive(true);
        ReleaseToken();
    }

    private void ReleaseToken()
    {
        if (_token != null)
            _token.Dispose();

        else
            Debug.LogError("No token to release");
    }
}
