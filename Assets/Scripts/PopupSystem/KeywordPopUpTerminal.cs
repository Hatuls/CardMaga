using CardMaga.UI.Visuals;
using UnityEngine;
namespace CardMaga.UI.PopUp
{
    public class KeywordPopUpTerminal : BasePopUpTerminal
    {
        private Transform _currentPosition;
        [SerializeField]
        private TransitionBuilder[] _transitionIn;
        [SerializeField]
        private TransitionBuilder[] _transitionOut;

        private IPopUpTransition<TransitionData> _popUpTransitionIn;
        private IPopUpTransition<TransitionData> _popUpTransitionOut;
        private IPopUpTransition<AlphaData> _popUpAlphaTransitionIn;

        public override IPopUpTransition<AlphaData> TransitionAlphaIn => _popUpAlphaTransitionIn;

        public override IPopUpTransition<AlphaData> TransitionAlphaOut => null;

        public override IPopUpTransition<TransitionData> TransitionIn => _popUpTransitionIn;

        public override IPopUpTransition<TransitionData> TransitionOut => _popUpTransitionOut;

        protected override Vector2 GetStartLocation() => _currentPosition.position;

        private void Awake()
        {
            KeywordTextAssigner.OnKeywordPointerUp += HideComboPopup;
            KeywordTextAssigner.OnKeywordPointerDown += ShowComboPopup;
            _popUpTransitionIn = new BasicTransition(GenerateTransitionData(_transitionIn));
            _popUpTransitionOut = new BasicTransition(GenerateTransitionData(_transitionOut));
            _popUpAlphaTransitionIn = new AlphaTransition(GenerateAlphaTransitionData(_transitionIn));
        }
        private void OnDestroy()
        {
            KeywordTextAssigner.OnKeywordPointerUp   -= HideComboPopup;
            KeywordTextAssigner.OnKeywordPointerDown -= ShowComboPopup;
        }
        private void HideComboPopup()
        {
            HidePopUp();
            _currentActivePopUp?.Dispose();
        }

        private void ShowComboPopup(KeywordTextAssigner keywordTextAssigner)
        {
            _currentPosition = keywordTextAssigner.transform;
            base.ShowPopUp();
            _currentActivePopUp.GetComponent<KeywordPopUpAssigner>().SetVisual(keywordTextAssigner);
        }

    }
}