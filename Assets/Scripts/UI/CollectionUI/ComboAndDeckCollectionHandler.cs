using Battle.Combo;
using CardMaga.Card;
using CardMaga.Collection;
using CardMaga.ObjectPool;
using CardMaga.UI.Card;
using CardMaga.UI.Combos;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

namespace CardMaga.UI.Collections
{
    public class ComboAndDeckCollectionHandler : MonoBehaviour
    {
        [SerializeField] private BattleComboUI _battleComboUI;
        [SerializeField] private BattleCardUI _battleCardUI;
        [Header("Scripts Reference")] [SerializeField]
        private BattleComboUIScrollPanelManager battleComboUIScroll;

        [SerializeField] private BattleCardUIScrollPanelManager battleCardUIScroll;
        [SerializeField] private CardDataFilterSystem _cardDataFilter;

        private BattleCardDataSort _battleCardDataSort;
        private ComboDataSort _comboDataSort;

        private IGetCollection<BattleCardData> _cardDatas;
        private IGetCollection<BattleComboData> _comboDatas;
        
        private VisualRequester<BattleComboUI, BattleComboData> _visualRequesterCombo;
        private VisualRequester<BattleCardUI, BattleCardData> _visualRequesterCard;

        private void ShowCombo()
        {
            battleComboUIScroll.RemoveAllObjectsFromPanel();
            var comboVisual =  _visualRequesterCombo.GetVisual(_comboDataSort.SortComboData(_comboDatas.GetCollection));
            
            var comboUIElement = comboVisual.ConvertAll(x => (IUIElement) x);
            
            battleComboUIScroll.AddObjectToPanel(comboUIElement);
        }

        private void ShowCard()
        {
            battleCardUIScroll.RemoveAllObjectsFromPanel();
            var cardVisual =
                _visualRequesterCard.GetVisual(
                    _battleCardDataSort.SortCardData(_cardDataFilter.Filter(_cardDatas.GetCollection)));
            
            var cardUIElement = cardVisual.ConvertAll(x => (IUIElement) x);
            
            battleCardUIScroll.AddObjectToPanel(cardUIElement);
        }

        private void OnDestroy()
        {
            _cardDataFilter.OnCycleFilter -= ShowCard;
        }

        public void AssignCardData(IGetCollection<BattleCardData> cardDatas)
        {
            _cardDatas = cardDatas;
            ShowCard();
        }

        public void AssignComboData(IGetCollection<BattleComboData> comboDatas)
        {
            _comboDatas = comboDatas;
            ShowCombo();
        }

        public void Init()
        {
            _battleCardDataSort = new BattleCardDataSort();
            _comboDataSort = new ComboDataSort();
            _cardDataFilter.OnCycleFilter += ShowCard;

            _visualRequesterCard = new VisualRequester<BattleCardUI, BattleCardData>(_battleCardUI);
            _visualRequesterCombo = new VisualRequester<BattleComboUI, BattleComboData>(_battleComboUI);

            battleCardUIScroll.Init();
            battleComboUIScroll.Init();
        }
    }
}