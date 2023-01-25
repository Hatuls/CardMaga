using Account;
using CardMaga.Battle.Players;
using CardMaga.ValidatorSystem;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    public class ValidationPopUpTerminal : BasePopUpTerminal
    {

        [SerializeField]
        private TransitionBuilder[] _transitionIn;
        [SerializeField]
        private TransitionBuilder[] _transitionOut;
        private IPopUpTransition<TransitionData> _popUpTransitionIn;
        private IPopUpTransition<TransitionData> _popUpTransitionOut;
        private IPopUpTransition<AlphaData> _popUpAlphaTransitionIn;


        [SerializeField]
        private LocationTagSO _middleScreenPosition;

        public override IPopUpTransition<AlphaData> TransitionAlphaIn => _popUpAlphaTransitionIn;

        public override IPopUpTransition<AlphaData> TransitionAlphaOut =>null;

        public override IPopUpTransition<TransitionData> TransitionIn => _popUpTransitionIn;

        public override IPopUpTransition<TransitionData> TransitionOut => _popUpTransitionOut;

        protected override Vector2 GetStartLocation() => PopUpManager.Instance.GetPosition(_middleScreenPosition);


        protected override void Awake()
        {
            if (PopUpManager.Instance == null) return;


            base.Awake();
            Validator.OnCriticalError += ShowPopUp;

            _popUpTransitionIn = new BasicTransition(GenerateTransitionData(_transitionIn));
            _popUpTransitionOut = new BasicTransition(GenerateTransitionData(_transitionOut));
            _popUpAlphaTransitionIn = new AlphaTransition(GenerateAlphaTransitionData(_transitionIn));
        }
        private void OnDestroy()
        {
            Validator.OnCriticalError -= ShowPopUp;
        }
        private void ShowPopUp(IValidFailedInfo valid)
        {
            HidePopUp();
            ShowPopUp();
            _currentActivePopUp.GetComponent<ValidationPopUpHandler>().AssignVisuals(valid);
        }

        private void ClosePopUp()
        {
            HidePopUp();
            _currentActivePopUp.Dispose();
        }

    }


}