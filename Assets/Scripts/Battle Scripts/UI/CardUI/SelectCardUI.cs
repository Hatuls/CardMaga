using System;
using System.Collections;
using System.Collections.Generic;
using Battles.UI;
using DG.Tweening;
using ReiTools.TokenMachine;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SelectCardUI : MonoBehaviour, ILockabel
{
    public event Action OnSelectCard;
    public event Action<CardUI> OnDisposeCard;

    [SerializeField] private ZoomCardUI _zoomCardUI;
    [SerializeField] private TransitionPackSO _followHand;
    [SerializeField] private TransitionPackSO _resetCardPosition;

    private TokenMachine _selectLockTokenMachine;

    private Sequence _currentSequence;
    private CardUI _selectCard;
    private Vector2 _mousePosition;
    
    public ITokenReciever SelectLockTokenReceiver
    {
        get { return _selectLockTokenMachine; }
    }
    
    public void Start()
    {
        _selectLockTokenMachine = new TokenMachine(UnLockInput,LockInput);
        InputReciever.OnTouchDetected += GetMousePos;
    }

    public void OnDestroy()
    {
        InputReciever.OnTouchDetected -= GetMousePos;
        KillTween();
    }
    
    
    public void SetSelectCardUI(CardUI cardUI)
    {
        OnSelectCard?.Invoke();
        cardUI.Inputs.OnBeginHold += SetToFollowState;
        cardUI.Inputs.OnClick += SetCardToZoomState;
        //_selectCard.Inputs.OnBeginHold
        //_selectCard.Inputs.OnEndHold
        _selectCard = cardUI;
        Debug.Log("Card Select is " + cardUI);
    }

    private void SetCardToZoomState(CardUI cardUI)
    {
        cardUI.Inputs.OnBeginHold -= SetToFollowState;
        _zoomCardUI.SetZoomCard(cardUI);
    }

    private void SetToFollowState(CardUI cardUI)
    {
        cardUI = cardUI;
        cardUI.Inputs.OnClick -= SetCardToZoomState;
        cardUI.Inputs.OnHold += FollowHand;
        cardUI.Inputs.OnPointUp += ReleaseCard;
    }

    public void ReleaseCard(CardUI cardUI)
    {
        cardUI.Inputs.OnBeginHold -= SetToFollowState;
        cardUI.Inputs.OnClick -= SetCardToZoomState;
        cardUI.Inputs.OnPointUp -= ReleaseCard;
        ResetCard(cardUI);
        OnDisposeCard?.Invoke(cardUI);
        Debug.Log("Release " + cardUI.name + " Form " + name);
    }

    public void ResetCard(CardUI cardUI)
    {
        KillTween();
        _currentSequence = cardUI.CardTransitionManager.Move(cardUI.HandPos, _resetCardPosition);
    }

    public void FollowHand(CardUI cardUI)
    {
        if (cardUI != null)
        {
            KillTween();
            _currentSequence = cardUI.CardTransitionManager.Move(_mousePosition, _followHand);
        }
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

    public void LockInput()
    {
        _selectCard.Inputs.ForceChangeState(false);
    }

    public void UnLockInput()
    {
        _selectCard.Inputs.ForceChangeState(true);
    }
}
