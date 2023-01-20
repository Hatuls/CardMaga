using System.Collections;
using UnityEngine;


namespace CardMaga.UI.PopUp
{

    public class NoStaminaPopUpHandler : BasePopUpTerminal
    {
        [SerializeField]
        private HandUI _handUI;
        [SerializeField, Range(0, 10f)]
        private float _duration;
        private WaitForSeconds _waitForSeconds;

        [SerializeField] TransitionBuilder[] _enterPack;
        [SerializeField] TransitionBuilder[] _exitPack;

        private IPopUpTransition<TransitionData> _enterTransition;
        private IPopUpTransition<TransitionData> _exitTransition;
        private IPopUpTransition<AlphaData> _enterAlphaTransition;
        private IPopUpTransition<AlphaData> _exitAlphaTransition;

        public override IPopUpTransition<TransitionData> TransitionIn => _enterTransition;
        public override IPopUpTransition<TransitionData> TransitionOut => _exitTransition;

        public override IPopUpTransition<AlphaData> TransitionAlphaIn => _enterAlphaTransition;

        public override IPopUpTransition<AlphaData> TransitionAlphaOut => _exitAlphaTransition;

        protected override Vector2 GetStartLocation() => PopUpManager.Instance.GetPosition(_startLocation);


        #region Monobehaviour Callbacks
        private void Awake()
        {
            _handUI.OnCardExecutionFailed += ShowPopUp;
            _handUI.OnCardExecutionSuccess += HidePopUp;

            _waitForSeconds = new WaitForSeconds(_duration);
            _enterAlphaTransition = new AlphaTransition(GenerateAlphaTransitionData(_enterPack));
            _exitAlphaTransition = new AlphaTransition(GenerateAlphaTransitionData(_exitPack));
            _enterTransition = new BasicTransition(GenerateTransitionData(_enterPack));
            _exitTransition = new BasicTransition(GenerateTransitionData(_exitPack));
        }

        private void OnDestroy()
        {
            _handUI.OnCardExecutionFailed -= ShowPopUp;
            _handUI.OnCardExecutionSuccess -= HidePopUp;
        }
        #endregion

        protected override void ShowPopUp()
        {
            base.ShowPopUp();
            _currentActivePopUp.PopUpTransitionHandler.TransitionOut.OnTransitionComplete += _currentActivePopUp.Dispose;
            StopAllCoroutines();
            StartCoroutine(Delay());
        }
        private IEnumerator Delay()
        {
            yield return _waitForSeconds;
            HidePopUp();
        }
        protected override void RemoveFromActiveList(PopUp obj)
        {
            obj.PopUpTransitionHandler.TransitionOut.OnTransitionComplete -= _currentActivePopUp.Dispose;
            base.RemoveFromActiveList(obj);
            StopAllCoroutines();
        }
    }


}