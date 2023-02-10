using CardMaga.UI.Visuals;
using UnityEngine;
namespace CardMaga.UI.PopUp
{
    public class KeywordPopUpTerminal : BasePopUpTerminal
    {
        private Transform _currentPosition;
  
        public override IPopUpTransition<AlphaData> TransitionAlphaOut => null;

   

        protected override Vector2 GetStartLocation() => _currentPosition.position;

        protected override void Start()
        {
            if (PopUpManager.Instance == null)
                return;
            KeywordTextAssigner.OnKeywordPointerUp += HideComboPopup;
            KeywordTextAssigner.OnKeywordPointerDown += ShowComboPopup;
            base.Start();
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