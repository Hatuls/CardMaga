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

        public override void EnterState(BattleCardUI battleCardUI)
        {
            base.EnterState(battleCardUI);
            _clickHelper.LoadObject(true,false,() => ReturnToHandState(battleCardUI),battleCardUI.RectTransform);
            MoveToZoomPosition(battleCardUI);
            if (OnExitZoomTutorial != null)
                OnExitZoomTutorial.Invoke();
        }

        public override void ExitState(BattleCardUI battleCardUI)
        {
             _clickHelper.Close();
            _zoomToken?.Dispose();
            base.ExitState(battleCardUI);
            if(OnEnterZoomTutorial != null)
                OnEnterZoomTutorial.Invoke();
        }

        public void ReturnToHandState(BattleCardUI battleCardUI)
        {
            _handUI.SetToHandState(battleCardUI);
        }

        private void SetToFollowState(BattleCardUI battleCardUI)
        {
            _handUI.SetToFollowState(battleCardUI);
        }

        public override BattleCardUI ForceExitState()
        {
            if (_zoomToken != null)
                _zoomToken.Dispose();
            
            return base.ForceExitState();
        }

        private void InitZoom()
        {
            if (SelectedBattleCardUI == null)
                return;

            if (OnZoomInLocation != null)
                OnZoomInLocation.Invoke();
            _zoomToken = SelectedBattleCardUI.CardVisuals.CardZoomHandler.ZoomTokenMachine.GetToken();
        }
        
        private void MoveToZoomPosition(BattleCardUI battleCardUI)
        {
            if (SelectedBattleCardUI != null)
            {
                SelectedBattleCardUI.KillTween(false);
                SelectedBattleCardUI.CurrentSequence = battleCardUI.RectTransform.Transition(_zoomPosition, _zoomCard, InitZoom);
            }
        }

   

    }
}