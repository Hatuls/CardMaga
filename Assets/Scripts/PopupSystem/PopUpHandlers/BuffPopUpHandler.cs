using CardMaga.UI.Buff;
using CardMaga.UI.PopUp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPopUpHandler : MonoBehaviour
{
    [SerializeField] private BuffVisualHandler _buffVisualHandler;

    [SerializeField] private PopUpSO _popUpSO; //The ID of the popUp
    [SerializeField] private TransitionPackSO[] _enterPackSO;
    [SerializeField] private TransitionPackSO[] _exitPackSO;
    private TransitionData[] _enterTransitionData;
    private TransitionData[] _exitTransitionData;

    private BasePopUp _currentBasePopUp;

    private void Awake()
    {

    }

    private void OnDestroy()
    {
    }

    private void InitBuffTransitionData()
    {
        _enterTransitionData = new TransitionData[_enterPackSO.Length];
        _enterTransitionData[0] = new TransitionData(_enterPackSO[0], transform.position);
        Vector2 buffLocation = _currentBasePopUp.RectTransform.GetLocalPosition();
        for (int i = 1; i < _enterTransitionData.Length; i++)
        {
            if (buffLocation.x > 0)
            {
            _enterTransitionData[i] = new TransitionData(_enterPackSO[i], new Vector2(buffLocation.x, transform.position.y));
            }

            else
                _enterTransitionData[i] = new TransitionData(_enterPackSO[i], new Vector2(transform.position.x-20, transform.position.y));
        }

        for (int i = 0; i < _enterTransitionData.Length; i++)
        {
            _currentBasePopUp.PopUpTransitionHandler.AddTransitionData(true, _enterTransitionData[i]);
        }



        _exitTransitionData = new TransitionData[_exitPackSO.Length];
        for (int i = 0; i < _exitTransitionData.Length; i++)
        {
            _exitTransitionData[i] = new TransitionData(_exitPackSO[i], transform.position);
        }

        for (int i = 0; i < _exitTransitionData.Length; i++)
        {
            _currentBasePopUp.PopUpTransitionHandler.AddTransitionData(false, _exitTransitionData[i]);
        }
    }

    public void ShowBuffPopUp()
    {
        if (_currentBasePopUp == null)
        {
            _currentBasePopUp = PopUpManager.Instance.PoolHandler.Pull(_popUpSO);
            _currentBasePopUp.OnDisposed += ResertPopUp;
            InitBuffTransitionData();
            _currentBasePopUp.Enter();
        }
    }

    public void HideBuffPopUp()
    {
        _currentBasePopUp.StartExitTransition();
    }

    private void ResertPopUp(BasePopUp basePopUp)
    {
        basePopUp.OnDisposed -= ResertPopUp;
        _currentBasePopUp = null;
    }
}
