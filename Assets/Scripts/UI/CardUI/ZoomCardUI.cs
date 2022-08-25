using System;
using DG.Tweening;
using UnityEngine;
namespace CardMaga.UI.Card
{


    public class ZoomCardUI : BaseHandUIState
    {
        [SerializeField] private HandUI _handUI;
        [SerializeField] private FollowCardUI _followCard;
        [SerializeField] private RectTransform _zoomPosition;
        [SerializeField] private TransitionPackSO _zoomCard;
        [SerializeField] private TransitionPackSO _resetZoomCard;

        private Sequence _currentSequence;

        private IDisposable _zoomToken;

        protected override void EnterState(CardUI cardUI)
        {
            base.EnterState(cardUI);
            
            MoveToZoomPosition(SelectedCardUI);
        }

        protected override CardUI ExitState()
        {
            return base.ExitState();
        }

        protected override CardUI ForceExitState()
        {
            return base.ForceExitState();
        }

        public void SetSelectCardUI(CardUI cardUI)
        {
        }

        private void InitZoom()
        {
            if (SelectedCardUI == null)
                return;
            
            _zoomToken = SelectedCardUI.CardVisuals.CardZoomHandler.ZoomTokenMachine.GetToken();
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
            if (SelectedCardUI != null)
            {
                KillTween();
                _currentSequence = cardUI.RectTransform.Transition(_zoomPosition, _zoomCard, InitZoom);
            }
        }

        public void ReleaseCard(CardUI cardUI)
        {
            if (!ReferenceEquals(cardUI, SelectedCardUI))
                return;

            _zoomToken.Dispose();

            _handUI.ReturnCardUIToHand(SelectedCardUI);

            SelectedCardUI = null;
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