using ReiTools.TokenMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CardMaga.UI.Card;

public class MaskInstruction : MonoBehaviour
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
        ZoomCardUI.OnZoomInTutorial += CloseCanvas;
        ZoomCardUI.OnZoomOutTutorial += CloseCanvas;
        DisplayCanvas();
    }

    private void DisplayCanvas()
    {
        _maskGameobject.SetActive(true);
        //_clickHelper.LoadObject(true, false, null, _maskTransform);
    }

    private void CloseCanvas()
    {
        _maskGameobject.SetActive(false);
        ReleaseToken();
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
        ZoomCardUI.OnZoomInTutorial -= CloseCanvas;
        ZoomCardUI.OnZoomOutTutorial -= CloseCanvas;
    }
}
