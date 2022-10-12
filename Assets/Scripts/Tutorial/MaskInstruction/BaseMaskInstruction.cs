using ReiTools.TokenMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseMaskInstruction : MonoBehaviour
{
    #region Fields
    [SerializeField] OperationManager _operationManager;
    [SerializeField] private GameObject _maskGameobject;
    [SerializeField] private RectTransform _maskTransform;
    private ClickHelper _clickHelper;
    private IDisposable _token;
    #endregion

    #region Events
    #endregion

    public void StartInstruction(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        _clickHelper = ClickHelper.Instance;
        gameObject.SetActive(true);
        SubscribeEvent();
        DisplayCanvas();
    }

    private void DisplayCanvas()
    {
        _maskGameobject.SetActive(true);
        //_clickHelper.LoadObject(true, false, null, _maskTransform);
    }

    protected void CloseCanvas()
    {
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

    private void OnDestroy()
    {
    }

    protected virtual void UnsubscribeEvent()
    {

    }

    protected virtual void SubscribeEvent()
    {

    }
}
