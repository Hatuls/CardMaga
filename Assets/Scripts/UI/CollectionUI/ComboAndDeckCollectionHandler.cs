using Battle.Combo;
using Battle.Deck;
using CardMaga.Card;
using CardMaga.Collection;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class ComboAndDeckCollectionHandler : MonoBehaviour
    {
        [Header("Scripts Reference")]
        [SerializeField] private ComboUIScrollPanelHandler _comboUIScroll;
        [SerializeField] private CardUIScrollPanelManager _cardUIScroll;
        [SerializeField] private CardDataFilterSystem _cardDataFilter;

        private CardDataSort _cardDataSort;
        private ComboDataSort _comboDataSort;
        
        private IGetCollection<CardData> _cardDatas;
        private IGetCollection<ComboData> _comboDatas;

        private void ShowCombo()
        {
            _comboUIScroll.RemoveAllObjectsFromPanel();
            _comboUIScroll.AddObjectToPanel(_comboDataSort.SortComboData(_comboDatas.GetCollection));
        }

        private void ShowCard()
        {
            _cardUIScroll.RemoveAllObjectsFromPanel();
            _cardUIScroll.AddObjectToPanel(_cardDataSort.SortCardData(_cardDataFilter.Filter(_cardDatas.GetCollection)));
        }

        private void OnDestroy()
        {
            _cardDataFilter.OnCycleFilter -= ShowCard;
        }

        public void AssignCardData(IGetCollection<CardData> cardDatas)
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
            _cardDataSort = new CardDataSort();
            _comboDataSort = new ComboDataSort();
            _cardDataFilter.OnCycleFilter += ShowCard;
            
            _cardUIScroll.Init();
            _comboUIScroll.Init();
        }
    }
}
