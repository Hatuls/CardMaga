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



        private RectTransform _buffRectTransform;


        protected override void Start()
        {
            if (PopUpManager.Instance == null) return;

            base.Start();
            BuffVisualHandler.OnBuffPointerDown += ShowPopUp;
            BuffVisualHandler.OnBuffPointerUp += ClosePopUp;
      
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


      
    }
}