using Battle.Combo;
using CardMaga.Card;
using CardMaga.Collection;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

namespace CardMaga.UI.Collections
{
    public class ComboAndDeckCollectionHandler : MonoBehaviour
    {
        [Header("Scripts Reference")]
        [SerializeField] private BattleComboUIScrollPanelHandler battleComboUIScroll;
        [SerializeField] private BattleCardUIScrollPanelManager battleCardUIScroll;
        [SerializeField] private CardDataFilterSystem _cardDataFilter;

        private BattleCardDataSort _battleCardDataSort;
        private ComboDataSort _comboDataSort;
        
        private IGetCollection<BattleCardData> _cardDatas;
        private IGetCollection<ComboData> _comboDatas;

        private void ShowCombo()
        {
            battleComboUIScroll.RemoveAllObjectsFromPanel();
            battleComboUIScroll.AddObjectToPanel(_comboDataSort.SortComboData(_comboDatas.GetCollection));
        }

        private void ShowCard()
        {
            battleCardUIScroll.RemoveAllObjectsFromPanel();
            battleCardUIScroll.AddObjectToPanel(_battleCardDataSort.SortCardData(_cardDataFilter.Filter(_cardDatas.GetCollection)));
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
        
        public void AssignComboData(IGetCollection<ComboData> comboDatas)
        {
            _comboDatas = comboDatas;
            ShowCombo();
        }
        
        public void Init()
        {
            _battleCardDataSort = new BattleCardDataSort();
            _comboDataSort = new ComboDataSort();
            _cardDataFilter.OnCycleFilter += ShowCard;
            
            battleCardUIScroll.Init();
            battleComboUIScroll.Init();
        }
    }
}
