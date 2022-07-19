using System;
using DG.Tweening;
using UnityEngine;
namespace CardMaga.UI.Card
{


    public class ZoomCardUI : MonoBehaviour
    {
        [SerializeField] private HandUI _handUI;
        [SerializeField] private FollowCardUI _followCard;
        [SerializeField] private RectTransform _zoomPosition;
        [SerializeField] private TransitionPackSO _zoomCard;
        [SerializeField] private TransitionPackSO _resetZoomCard;

        private Sequence _currentSequence;

        private CardUI _selectCardUI;

        private IDisposable _zoomToken;

        public void SetZoomCard(CardUI cardUI)
        {
            if (_selectCardUI != null)
                return;

            _selectCardUI = cardUI;
            _selectCardUI.Inputs.OnBeginHold -= _followCard.SetSelectCardUI;
            _selectCardUI.Inputs.OnClick -= SetZoomCard;
            MoveToZoomPosition(_selectCardUI);
        }

        private void InitZoom()
        {
            if (_selectCardUI == null)
                return;
            
            _zoomToken = _selectCardUI.CardVisuals.CardZoomHandler.ZoomTokenMachine.GetToken();
            _selectCardUI.Inputs.OnBeginHold += SetToFollow;
            _selectCardUI.Inputs.OnClick += ReturnCardToHand;
        }

        private void SetToFollow(CardUI cardUI)
        {
            if (!ReferenceEquals(cardUI, _selectCardUI))
                return;

            _selectCardUI.Inputs.OnClick -= ReturnCardToHand;
            _selectCardUI.Inputs.OnBeginHold -= SetToFollow;

            if (_zoomToken != null)
                _zoomToken.Dispose();

            _followCard.SetSelectCardUI(_selectCardUI);
            _selectCardUI = null;
        }

        private void MoveToZoomPosition(CardUI cardUI)
        {
            if (_selectCardUI != null)
            {
                KillTween();
                _currentSequence = cardUI.CardTransitionManager.Transition(_zoomPosition, _zoomCard, InitZoom);
            }
        }

        private void ReturnCardToHand(CardUI cardUI)
        {
            if (!ReferenceEquals(cardUI, _selectCardUI))
                return;

            _selectCardUI.Inputs.OnBeginHold -= SetToFollow;
            _selectCardUI.Inputs.OnClick -= ReturnCardToHand;

            _zoomToken.Dispose();

            _handUI.ReturnCardUIToHand(_selectCardUI);

            _selectCardUI = null;
        }

        public void ForceReleaseCard()
        {
            if (_selectCardUI == null)
                return;

            _selectCardUI.Inputs.OnBeginHold -= SetToFollow;
            _selectCardUI.Inputs.OnClick -= ReturnCardToHand;
            _zoomToken.Dispose();

            _handUI.ForceReturnCardUIToHand(_selectCardUI);

            _selectCardUI = null;
        }

        private void KillTween()
        {
            if (_currentSequence != null) _currentSequence.Kill();
        }
    }
}