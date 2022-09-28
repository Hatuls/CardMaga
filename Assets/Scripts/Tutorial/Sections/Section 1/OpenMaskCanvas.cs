using ReiTools.TokenMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OpenMaskCanvas : MonoBehaviour
{
    [SerializeField] OperationManager _operationManager;
    [SerializeField] private GameObject _maskGameobject;
    [SerializeField] private float _seconds = 5f;
    private IDisposable _token;

    public void DisplayCanvas(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        _maskGameobject.SetActive(true);
        Debug.Log("Canvas Started!");
        StartCoroutine(WaitSeconds(_seconds, CloseCanvas));
        Debug.Log("Canvas Ended!");
    }


    IEnumerator WaitSeconds(float num, Action OnComplete)
    {
        yield return new WaitForSeconds(num);
        OnComplete.Invoke();
    }

    private void CloseCanvas()
    {
        _maskGameobject.SetActive(false);
        if (_token != null)
            _token.Dispose();

        else
            Debug.LogError("No token to release");
    }
}
