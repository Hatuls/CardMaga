
﻿using ReiTools.TokenMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class BaseMaskInstruction : MonoBehaviour
{
    #region Fields
    [SerializeField] OperationManager _operationManager;
    [SerializeField] private GameObject _maskGameobject;
    [SerializeField] private RectTransform _maskTransform;
    [SerializeField] private bool CloseOnClick;
    private TutorialClickHelper _tutorialClickHelper;
    private IDisposable _token;
    #endregion

    #region Events
    [SerializeField] private UnityEvent OnMaskStart;
    [SerializeField] private UnityEvent OnMaskEnd;
    #endregion

    public void StartInstruction(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        _tutorialClickHelper = TutorialClickHelper.Instance;
        gameObject.SetActive(true);
        SubscribeEvent();
        DisplayCanvas();
        if (OnMaskStart != null)
            OnMaskStart.Invoke();
    }

    private void DisplayCanvas()
    {
        if(CloseOnClick)
                _tutorialClickHelper.LoadObject(true, false, CloseCanvas, _maskTransform);

        else
                _tutorialClickHelper.LoadObject(true, false, ReturnCanvasObjects, _maskTransform);
        
    }

    public void CloseCanvas()
    {
        if (OnMaskEnd != null)
            OnMaskEnd.Invoke();

        _tutorialClickHelper.Close();
        UnsubscribeEvent();
        ReleaseToken();
        _maskGameobject.SetActive(false);
    }

    protected void ReturnCanvasObjects()
    {
            _tutorialClickHelper.ReturnObjects();
        UnsubscribeEvent();
        ReleaseToken();
        _maskGameobject.SetActive(false);
    }

    private void ReleaseToken()
    {
        if (_token != null)
            _token.Dispose();

        else
            Debug.LogError("No token to release");
    }

    protected virtual void UnsubscribeEvent()
    {

    }

    protected virtual void SubscribeEvent()
    {

    }
}