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

        public void SetSelectCardUI(CardUI cardUI)
        {
            if (_selectCardUI != null)
                return;

            _selectCardUI = cardUI;
            MoveToZoomPosition(_selectCardUI);
        }

        private void InitZoom()
        {
            if (_selectCardUI == null)
                return;
            
            _zoomToken = _selectCardUI.CardVisuals.CardZoomHandler.ZoomTokenMachine.GetToken();
        }

        public void SetToFollow(CardUI cardUI)
        {
            if (!ReferenceEquals(cardUI, _selectCardUI))
                return;

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
                _currentSequence = cardUI.RectTransform.Transition(_zoomPosition, _zoomCard, InitZoom);
            }
        }

        public void ReturnCardToHand(CardUI cardUI)
        {
            if (!ReferenceEquals(cardUI, _selectCardUI))
                return;

            _zoomToken.Dispose();

            _handUI.ReturnCardUIToHand(_selectCardUI);

            _selectCardUI = null;
        }

        public void ForceReleaseCard()
        {
            if (_selectCardUI == null)
                return;
            
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