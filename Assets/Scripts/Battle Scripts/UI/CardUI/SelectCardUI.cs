using System;
using System.Collections;
using System.Collections.Generic;
using Battles.UI;
using DG.Tweening;
using FMOD;
using UnityEngine;
using Debug = UnityEngine.Debug;

[Serializable]
public class SelectCardUI
{
    [SerializeField] private RectTransform _zoomPosition;
    [SerializeField] private TransitionPackSO _followHand;
    [SerializeField] private TransitionPackSO _resetCardPosition;
    [SerializeField] private TransitionPackSO _zoomCard;
    [SerializeField] private float _zoomScaleMultiply;

    private Sequence _currentSequence;
    private CardUI _selectCard;
    private Vector2 _mousePosition;

    public void InitSelectCardUI()
    {
        InputReciever.OnTouchDetectd += GetMousePos;
    }

    public void DisableSelectCardUI()
    {
        InputReciever.OnTouchDetectd -= GetMousePos;
        KillTween();
    }
    
    public CardUI Card
    {
        get { return _selectCard; }
    }
    
    public void SetSelectCardUI(CardUI cardUI)
    {
        _selectCard = cardUI;
        Debug.Log("Card Select is " + _selectCard);
    }

    public void ReleaseCard(CardUI cardUI)
    {
        _selectCard = null;
    }
    
    public void DisposeCard(Action onDispose = null)
    {
        if (onDispose != null)
        {
            onDispose?.Invoke();
        }
    }

    public void ZoomCard(CardUI cardUI)
    {
        KillTween();
        _currentSequence = _selectCard.CardTransitionManager.Move(_zoomPosition.position, _zoomCard)
            .Join(_selectCard.CardTransitionManager.Scale(_zoomScaleMultiply, _zoomCard));
    }
    
    public void ResetCard(CardUI cardUI)
    {
        if (_selectCard == cardUI)
        {
            KillTween();
            _currentSequence = _selectCard.CardTransitionManager.Move(_selectCard.HandPos, _resetCardPosition)
                .OnComplete(KillTween);
        }
    }
    
    public void FollowHand(CardUI cardUI)
    {
        if (cardUI == _selectCard)
        {
            KillTween();
            _currentSequence = _selectCard.CardTransitionManager.Move(_mousePosition, _followHand);
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
}
