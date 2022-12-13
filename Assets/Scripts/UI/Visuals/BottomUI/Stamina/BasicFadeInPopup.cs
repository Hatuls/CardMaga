using DG.Tweening;
using System;
using UnityEngine;


namespace CardMaga.UI.PopUp
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BasicFadeInPopup : BasePopUp
    {
        public event Action OnPopUpFinishEntranceTransition;
        public event Action OnPopUpFinishExitTransition;
        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField, Min(0)]
        private float _alphaEntranceDuration;
        [SerializeField, Min(0)]
        private float _alphaExitDuration;

        protected override void ResetParams()
        {
            if (_sequence != null)
                _sequence.Kill(false);
            if (!IsActive())
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
}