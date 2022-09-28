using System;
using DG.Tweening;
using UnityEngine;

namespace CardMaga.UI.Card
{
    public class ZoomCardUI : BaseHandUIState
    {
        [Header("Scripts Reference")] 
        [SerializeField] private HandUI _handUI;
        [SerializeField] private ClickHelper _clickHelper;
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
            _inputBehaviour.OnBeginHold += SetToFallowState;
        }

        private void OnDestroy()
        {
            _inputBehaviour.OnClick -= ReturnToHandState;
            _inputBehaviour.OnBeginHold -= SetToFallowState;
        }

        public override void EnterState(CardUI cardUI)
        {
            base.EnterState(cardUI);
            _clickHelper.LoadObject(true,false,() => ReturnToHandState(cardUI),cardUI.RectTransform);
            MoveToZoomPosition(cardUI);
        }

        public override void ExitState(CardUI cardUI)
        {
            _clickHelper.Close();
            _zoomToken?.Dispose();
            base.ExitState(cardUI);
        }

        private void ReturnToHandState(CardUI cardUI)
        {
            _handUI.SetToHandState(cardUI);
        }

        private void SetToFallowState(CardUI cardUI)
        {
            _handUI.SetToFollowState(cardUI);
        }

        public override CardUI ForceExitState()
        {
            if (_zoomToken != null)
                _zoomToken.Dispose();
            
            return base.ForceExitState();
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