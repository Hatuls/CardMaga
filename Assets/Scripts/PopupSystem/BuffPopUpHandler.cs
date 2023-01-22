using CardMaga.Battle.Players;
using CardMaga.UI.Buff;
using CardMaga.UI.Visuals;
using Keywords;
using UnityEngine;
namespace CardMaga.UI.PopUp
{
    public class BuffPopUpHandler : BasePopUpTerminal
    {
        [SerializeField]
        private RectTransform _popUpLocation;
        [SerializeField]
        private KeywordsCollectionSO AllKeywords;



        [SerializeField]
        private TransitionBuilder[] _transitionIn;
        [SerializeField]
        private TransitionBuilder[] _transitionOut;
        private IPopUpTransition<TransitionData> _popUpTransitionIn;
        private IPopUpTransition<TransitionData> _popUpTransitionOut;
        private IPopUpTransition<AlphaData> _popUpAlphaTransitionIn;
        private RectTransform _buffRectTransform;

        public override IPopUpTransition<AlphaData> TransitionAlphaIn => _popUpAlphaTransitionIn;

        public override IPopUpTransition<AlphaData> TransitionAlphaOut => null;

        public override IPopUpTransition<TransitionData> TransitionIn => _popUpTransitionIn;

        public override IPopUpTransition<TransitionData> TransitionOut => _popUpTransitionOut;

        protected override void Awake()
        {
            if (PopUpManager.Instance == null) return;
            
            
            base.Awake();
            BuffVisualHandler.OnBuffPointerDown += ShowPopUp;
            BuffVisualHandler.OnBuffPointerUp += ClosePopUp;
            _popUpTransitionIn = new BasicTransition(GenerateTransitionData(_transitionIn));
            _popUpTransitionOut = new BasicTransition(GenerateTransitionData(_transitionOut));
            _popUpAlphaTransitionIn = new AlphaTransition(GenerateAlphaTransitionData(_transitionIn));
        }

        private void OnDestroy()
        {
            BuffVisualHandler.OnBuffPointerDown -= ShowPopUp;
            BuffVisualHandler.OnBuffPointerUp -= ClosePopUp;
        }
        private void ShowPopUp(BuffVisualData buffVisualData, RectTransform transform)
        {
            HidePopUp();
            _buffRectTransform = transform;
            ShowPopUp();
            _currentActivePopUp.GetComponent<BuffPopUpVisual>().SetVisual(AllKeywords.GetKeywordSO(buffVisualData.KeywordType), buffVisualData.BuffCurrentAmount);
        }
        
        private void ClosePopUp(BuffVisualData buffVisualData, RectTransform transform)
        {
            HidePopUp();
            _currentActivePopUp.Dispose();
        }
        protected override Vector2 GetStartLocation() => _buffRectTransform.position;


      
        private Vector2 PopUpDestination() => _popUpLocation.position;
    }
}