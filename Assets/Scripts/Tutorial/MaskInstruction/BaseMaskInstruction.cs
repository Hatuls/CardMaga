using ReiTools.TokenMachine;
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
    [SerializeField] private bool _loadOnTutorialPanel;
    private ClickHelper _clickHelper;
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
        _clickHelper = ClickHelper.Instance;
        _tutorialClickHelper = TutorialClickHelper.Instance;
        gameObject.SetActive(true);
        if (OnMaskStart != null)
            OnMaskStart.Invoke();
        SubscribeEvent();
        DisplayCanvas();
    }

    private void DisplayCanvas()
    {
        if(CloseOnClick)
        {
            if(_loadOnTutorialPanel)
                _tutorialClickHelper.LoadObject(true, false, CloseCanvas, _maskTransform);

            else
                _clickHelper.LoadObject(true, false, CloseCanvas, _maskTransform);
        }

        else
        {
            if (_loadOnTutorialPanel)
                _tutorialClickHelper.LoadObject(true, false, ReturnCanvasObjects, _maskTransform);

            else
                _clickHelper.LoadObject(true, false, ReturnCanvasObjects, _maskTransform);
        }
        
    }

    public void CloseCanvas()
    {
        if (OnMaskEnd != null)
            OnMaskEnd.Invoke();

        if (_loadOnTutorialPanel)
            _tutorialClickHelper.Close();
        else
           _clickHelper.Close();
        UnsubscribeEvent();
        ReleaseToken();
        _maskGameobject.SetActive(false);
    }

    protected void ReturnCanvasObjects()
    {
        if (_loadOnTutorialPanel)
            _tutorialClickHelper.ReturnObjects();
        else
            _clickHelper.ReturnObjects();
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
