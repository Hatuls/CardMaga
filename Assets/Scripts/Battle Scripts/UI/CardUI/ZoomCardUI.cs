using Battles.UI;
using DG.Tweening;
using UnityEngine;

public class ZoomCardUI : MonoBehaviour
{
    [SerializeField] private HandUI _handUI;
    [SerializeField] private FollowCardUI _followCard;
    [SerializeField] private RectTransform _zoomPosition;
    [SerializeField] private TransitionPackSO _zoomCard;
    [SerializeField] private TransitionPackSO _resetZoomCard;

    private CardUI _selectCardUI;
    
    private Sequence _currentSequence;

    public void SetZoomCard(CardUI cardUI)
    {
        if (_selectCardUI != null)
            return;

        _selectCardUI = cardUI;
        _selectCardUI.Inputs.OnBeginHold -= _followCard.SetSelectCardUI;
        _selectCardUI.Inputs.OnClick -= SetZoomCard;
        _selectCardUI.Inputs.OnBeginHold += SetToFollow;
        _selectCardUI.Inputs.OnClick += ReturnCardToHand;
        ZoomCard(_selectCardUI);
    }

    private void SetToFollow(CardUI cardUI)
    {
        if (!ReferenceEquals(cardUI,_selectCardUI))
            return;

        _selectCardUI.Inputs.OnClick -= ReturnCardToHand;
        _selectCardUI.Inputs.OnBeginHold -= SetToFollow;
        _selectCardUI.CardTransitionManager.Scale(_resetZoomCard);
        _followCard.SetSelectCardUI(_selectCardUI);
        _selectCardUI = null;
    }
    
    private void ZoomCard(CardUI cardUI)
    {
        if (_selectCardUI != null)
        {
            KillTween();
            _currentSequence = cardUI.CardTransitionManager.Transition(_zoomPosition, _zoomCard);
        }
    }

    private void ReturnCardToHand(CardUI cardUI)
    {
        if (!ReferenceEquals(cardUI,_selectCardUI))
            return;

        _selectCardUI.Inputs.OnBeginHold -= SetToFollow;
        _selectCardUI.Inputs.OnClick -= ReturnCardToHand;
        _handUI.ReturnCardUIToHand(_selectCardUI);
        
        _selectCardUI = null;
    }
    
    private void KillTween()
    {
        if (_currentSequence != null)
        {
            _currentSequence.Kill();
        }
    }
}
