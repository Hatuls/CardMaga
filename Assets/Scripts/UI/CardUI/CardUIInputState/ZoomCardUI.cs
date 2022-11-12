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

        public static event Action OnZoomInLocation;
        public static event Action OnEnterZoomTutorial;
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

        public override void EnterState(BattleCardUI cardUI)
        {
            base.EnterState(cardUI);
            _clickHelper.LoadObject(true,false,() => ReturnToHandState(cardUI),cardUI.RectTransform);
            MoveToZoomPosition(cardUI);
        }

        public override void ExitState(BattleCardUI cardUI)
        {
             _clickHelper.Close();
            _zoomToken?.Dispose();
            base.ExitState(cardUI);

            
        }

        public void ReturnToHandState(BattleCardUI cardUI)
        {
            _handUI.SetToHandState(cardUI);
        }

        private void SetToFollowState(BattleCardUI cardUI)
        {
            _handUI.SetToFollowState(cardUI);
        }

        public override BattleCardUI ForceExitState()
        {
            if (_zoomToken != null)
                _zoomToken.Dispose();

            if (OnExitZoomTutorial != null)
                OnExitZoomTutorial.Invoke();

            return base.ForceExitState();
        }

        private void InitZoom()
        {
            if (_selectedCardUI == null)
                return;

            if (OnZoomInLocation != null)
                OnZoomInLocation.Invoke();
            _zoomToken = _selectedCardUI.CardVisuals.CardZoomHandler.ZoomTokenMachine.GetToken();
        }
        
        public void MoveToZoomPosition(BattleCardUI cardUI)
        {
            if (_selectedCardUI == null)
                SetCardUI(cardUI);
                KillTween();
                _currentSequence = cardUI.RectTransform.Transition(_zoomPosition, _zoomCard, InitZoom);
            if (OnEnterZoomTutorial != null)
                OnEnterZoomTutorial.Invoke();
        }

        private void KillTween()
        {
            if (_currentSequence != null) _currentSequence.Kill();
        }

    }
}