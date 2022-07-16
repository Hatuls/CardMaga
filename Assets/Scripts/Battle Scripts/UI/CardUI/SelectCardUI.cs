using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Battles.UI;
using DG.Tweening;
using ReiTools.TokenMachine;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SelectCardUI : MonoBehaviour
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
        cardUI.Inputs.OnClick -= _zoomCardUI.SetZoomCard;
        cardUI.Inputs.OnHold += FollowHand;
        cardUI.Inputs.OnPointUp += ReleaseCard;
        
        Debug.Log("Card Select is " + cardUI);
        
    }

    public void ReleaseCard(CardUI cardUI)
    {
        cardUI.Inputs.OnHold -= FollowHand;
        cardUI.Inputs.OnPointUp -= ReleaseCard;
        _selectCardUI = null;
        _handUI.AddCardUIToHand(cardUI);
        
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
