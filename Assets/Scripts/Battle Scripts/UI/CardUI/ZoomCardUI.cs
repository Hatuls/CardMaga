using Battles.UI;
using DG.Tweening;
using UnityEngine;

public class ZoomCardUI : MonoBehaviour
{
    [SerializeField] private HandUI _handUI;
    [SerializeField] private SelectCardUI _selectCard;
    [SerializeField] private RectTransform _zoomPosition;
    [SerializeField] private TransitionPackSO _zoomCard;
    [SerializeField] private TransitionPackSO _resetZoomCard;
    
    private Sequence _currentSequence;

    public void SetZoomCard(CardUI cardUI)
    {
        cardUI.Inputs.OnBeginHold -= _selectCard.SetSelectCardUI;
        cardUI.Inputs.OnClick -= SetZoomCard;
        cardUI.Inputs.OnBeginHold += SetToFollow;
        cardUI.Inputs.OnClick += ReturnCardToHand;
        ZoomCard(cardUI);
    }

    private void SetToFollow(CardUI cardUI)
    {
        cardUI.Inputs.OnClick -= ReturnCardToHand;
        cardUI.Inputs.OnBeginHold -= SetToFollow;
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

    private void ReturnCardToHand(CardUI cardUI)
    {
        cardUI.Inputs.OnBeginHold -= SetToFollow;
        cardUI.Inputs.OnPointDown -= ReturnCardToHand;
        _handUI.AddCardUIToHand(cardUI);
    }
    
    private void KillTween()
    {
        if (_currentSequence != null)
        {
            _currentSequence.Kill();
        }
    }
}
