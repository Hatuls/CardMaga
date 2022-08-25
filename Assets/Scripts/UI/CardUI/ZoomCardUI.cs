using System;
using DG.Tweening;
using UnityEngine;
namespace CardMaga.UI.Card
{
    public class ZoomCardUI : BaseHandUIState
    {
        [SerializeField] private RectTransform _zoomPosition;
        [SerializeField] private TransitionPackSO _zoomCard;
        [SerializeField] private TransitionPackSO _resetZoomCard;

        private Sequence _currentSequence;

        private IDisposable _zoomToken;

        public override void EnterState(CardUI cardUI)
        {
            base.EnterState(cardUI);
            
            MoveToZoomPosition(SelectedCardUI);
        }

        public override void ExitState(CardUI cardUI)
        {
            base.ExitState(cardUI);
            _zoomToken.Dispose();
        }

        public override void ForceExitState(CardUI cardUI)
        {
            base.ForceExitState(cardUI);
            _zoomToken.Dispose();
        }

        private void InitZoom()
        {
            if (SelectedCardUI == null)
                return;
            
            _zoomToken = SelectedCardUI.CardVisuals.CardZoomHandler.ZoomTokenMachine.GetToken();
        }
        
        private void MoveToZoomPosition(CardUI cardUI)
        {
            if (SelectedCardUI != null)
            {
                KillTween();
                _currentSequence = cardUI.RectTransform.Transition(_zoomPosition, _zoomCard, InitZoom);
            }
        }

        private void KillTween()
        {
            if (_currentSequence != null) _currentSequence.Kill();
        }
    }
}