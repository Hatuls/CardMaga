using System;
using DG.Tweening;
using UnityEngine;
using Battle.Data;

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

        public static event Action OnEnterZoomTutorial;
        public static event Action OnZoomInLocation;
        public static event Action OnExitZoomTutorial;

        private Sequence _currentSequence;
        [SerializeField] private bool _isOnDialogue;
        private IDisposable _zoomToken;

        private void Start()
        {
            _inputBehaviour.OnClick += ReturnToHandState;
            _inputBehaviour.OnBeginHold += SetToFollowState;
        }

        private void OnDestroy()
        {
            _inputBehaviour.OnClick -= ReturnToHandState;
            _inputBehaviour.OnBeginHold -= SetToFollowState;
        }

        public override void EnterState(CardUI cardUI)
        {
            base.EnterState(cardUI);
            _clickHelper.LoadObject(true,false,() => ReturnToHandState(cardUI),cardUI.RectTransform);
            MoveToZoomPosition(cardUI);
            if (OnExitZoomTutorial != null)
                OnExitZoomTutorial.Invoke();
        }

        public override void ExitState(CardUI cardUI)
        {
             _clickHelper.Close();
            _zoomToken?.Dispose();
            base.ExitState(cardUI);
            if(OnEnterZoomTutorial != null)
                OnEnterZoomTutorial.Invoke();
        }

        public void ReturnToHandState(CardUI cardUI)
        {
            _handUI.SetToHandState(cardUI);
        }

        private void SetToFollowState(CardUI cardUI)
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

            if (OnZoomInLocation != null)
                OnZoomInLocation.Invoke();
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