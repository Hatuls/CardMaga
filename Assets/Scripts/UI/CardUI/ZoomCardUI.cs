using System;
using DG.Tweening;
using UnityEngine;
namespace CardMaga.UI.Card
{
    public class ZoomCardUI : BaseHandUIState
    {
        [Header("Scripts Reference")]
        [SerializeField] private CardUiInputBehaviourHandler _behaviourHandler;
        [Header("TransitionPackSO")]
        [SerializeField] private TransitionPackSO _zoomCard;
        [SerializeField] private TransitionPackSO _resetZoomCard;
        [Header("RectTransforms")]
        [SerializeField] private RectTransform _zoomPosition;

        private Sequence _currentSequence;

        private IDisposable _zoomToken;

        private void Start()
        {
            _inputBehaviour.OnClick += ReturnToHandState;
        }

        private void OnDestroy()
        {
            _inputBehaviour.OnClick -= ReturnToHandState;
        }

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

        private void ReturnToHandState(CardUI cardUI)
        {
            _behaviourHandler.SetState(CardUiInputBehaviourHandler.HandState.Hand,cardUI);
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