using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Battles.UI;
using DG.Tweening;
using ReiTools.TokenMachine;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class FollowCardUI : MonoBehaviour
{

    [SerializeField] private HandUI _handUI;
    [SerializeField] private ZoomCardUI _zoomCardUI;
    [SerializeField] private TransitionPackSO _followHand;

    private CardUI _selectCardUI;
    

    private Sequence _currentSequence;
    private IDisposable _token;
    
    private Vector2 _mousePosition;
    
    public void Start()
    {
        InputReciever.OnTouchDetected += GetMousePos;
    }

    public void OnDestroy()
    {
        InputReciever.OnTouchDetected -= GetMousePos;
        KillTween();
    }
    
    public void SetSelectCardUI(CardUI cardUI)
    {
        if (_selectCardUI != null)
            return;

        _selectCardUI = cardUI;
        _selectCardUI.Inputs.OnClick -= _zoomCardUI.SetZoomCard;
        _selectCardUI.Inputs.OnHold += FollowHand;
        _selectCardUI.Inputs.OnPointUp += ReleaseCard;
        
        Debug.Log("Card Select is " + _selectCardUI);
        
    }

    public void ReleaseCard(CardUI cardUI)
    {
        if (!ReferenceEquals(cardUI,_selectCardUI))
            return;
        
        _selectCardUI.Inputs.OnHold -= FollowHand;
        _selectCardUI.Inputs.OnPointUp -= ReleaseCard;
        _handUI.AddCardUIToHand(_selectCardUI);
        
        _selectCardUI = null;
        
        Debug.Log("Release " + cardUI.name + " Form " + name);
    }

    public void FollowHand(CardUI cardUI)
    {
        KillTween(); 
        _currentSequence = cardUI.CardTransitionManager.Move(_mousePosition, _followHand);
    }
    
    private void GetMousePos(Vector2 mousePos)
    {
        _mousePosition = mousePos;
    }

    private void KillTween()
    {
        if (_currentSequence != null)
        {
            _currentSequence.Kill();
        }
    }
}
