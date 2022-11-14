using CardMaga.Keywords;
using CardMaga.Tools.Pools;
using DG.Tweening;
using System;
using UnityEngine;


namespace CardMaga.UI.PopUp
{

    public class NoStaminaPopUpHandler : BasePopUp
    {
        [SerializeField]
        private HandUI _handUI;
        [SerializeField, Range(0, 10f)]
        private float _duration;


        #region Monobehaviour Callbacks
        private void Awake()
        {
            _handUI
        }

        private void OnDestroy()
        {

        }
        #endregion

        private void ShowPopUp()
        {

        }

    }
    [RequireComponent(typeof(CanvasGroup))]
    public class NoStaminaPopUp : BasePopUp
    {
        public event Action OnPopUpFinishEntranceTransition;
        public event Action OnPopUpFinishExitTransition;
        [SerializeField,Min(0)]
        private float _alphaEntranceDuration;
        [SerializeField,Min(0)]
        private float _alphaExitDuration;

        private CanvasGroup _canvasGroup;

        protected override void ResetParams()
        {
            if (_sequence != null)
                _sequence.Kill(false);
            _canvasGroup.alpha = 0;
        }
        public override void Enter()
        {
            base.Enter();
            _sequence = _canvasGroup.DOFade(1, _alphaEntranceDuration)
                        .OnComplete(PopUpFinishEntranceTransition);
        }

        public override void Close()
        {
            base.Close();
            if (_sequence != null)
                _sequence.Kill(false);
            _sequence = _canvasGroup.DOFade(0, _alphaExitDuration)
                        .OnComplete(PopUpFinishExitTransition);
        }         

        private void PopUpFinishEntranceTransition() => OnPopUpFinishEntranceTransition?.Invoke();
        private void PopUpFinishExitTransition() => OnPopUpFinishExitTransition?.Invoke();
    }

    public abstract class BasePopUp : BaseUIElement , IPoolableMB<BasePopUp>
    {
        public event Action<BasePopUp> OnDisposed;

        [SerializeField]
        private bool _toRememberPreviousScreen = false;
        [SerializeField]
        protected RectTransform _rectTransform;

        protected Tween _sequence;


        protected virtual void ResetParams()
        {
            if (_sequence != null)
                _sequence.Kill(false);
            _rectTransform.SetScale(0);
        }
        public virtual void Enter()
        {
            ResetParams();
            UIHistoryManager.Show(this, _toRememberPreviousScreen);
        }

        public virtual void Close()
        {
            UIHistoryManager.ReturnBack();
        }

        public void Dispose()
        => OnDisposed?.Invoke(this);
    }
}