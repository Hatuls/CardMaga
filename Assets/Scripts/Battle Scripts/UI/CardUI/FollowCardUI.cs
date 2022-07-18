using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Battles;
using Battles.UI;
using DG.Tweening;
using ReiTools.TokenMachine;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class FollowCardUI : MonoBehaviour
{
    public event Action<CardUI> OnCardExecut;

    [SerializeField] private HandUI _handUI;
    [SerializeField] private ZoomCardUI _zoomCardUI;
    [SerializeField] private TransitionPackSO _followHand;
    [SerializeField] private RectTransform _executionBoundry;

    private CardUI _selectCardUI;
    private float _executionBoundry_Y;

    private Sequence _currentSequence;
    private IDisposable _token;
    
    private Vector2 _mousePosition;
    
    public void Start()
    {
        _executionBoundry_Y = _executionBoundry.position.y;
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

        if (cardUI.transform.position.y > _executionBoundry_Y && CardExecutionManager.Instance.TryExecuteCard(_selectCardUI))
        {
            OnCardExecut?.Invoke(_selectCardUI);
        }
        else
        {
            _handUI.ReturnCardUIToHand(_selectCardUI);
        }
        
        _selectCardUI = null;
        
        Debug.Log("Release " + cardUI.name + " Form " + name);
    }

    public void ForceReleaseCard()
    {
        if (_selectCardUI == null)
            return;
        
        _selectCardUI.Inputs.OnHold -= FollowHand;
        _selectCardUI.Inputs.OnPointUp -= ReleaseCard;
        _handUI.ForceReturnCardUIToHand(_selectCardUI);
        _selectCardUI = null;
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
