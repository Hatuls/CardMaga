using System.Collections;
using System.Collections.Generic;
using Battles.UI;
using DG.Tweening;
using UnityEngine;

public class ZoomCardUI : MonoBehaviour
{
    [SerializeField] private SelectCardUI _selectCard;
    [SerializeField] private RectTransform _zoomPosition;
    [SerializeField] private TransitionPackSO _zoomCard;
    [SerializeField] private TransitionPackSO _resetZoomCard;
    
    private Sequence _currentSequence;

    public void SetZoomCard(CardUI cardUI)
    {
        cardUI.Inputs.OnBeginHold += SetToFollow;
        ZoomCard(cardUI);
    }

    private void SetToFollow(CardUI cardUI)
    {
        cardUI.CardTransitionManager.Scale(_resetZoomCard);
        _selectCard.SetSelectCardUI(cardUI);
    }
    
    private void ZoomCard(CardUI cardUI)
    {
        if (_selectCard != null)
        {
            KillTween();
            _currentSequence = cardUI.CardTransitionManager.Transition(_zoomPosition, _zoomCard);
        }
        
    }
    
    private void KillTween()
    {
        if (_currentSequence != null)
        {
            _currentSequence.Kill();
        }
    }
}
