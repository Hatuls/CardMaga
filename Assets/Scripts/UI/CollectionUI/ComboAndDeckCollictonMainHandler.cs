using System.Collections.Generic;
using Battle.Combo;
using CardMaga.Card;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class ComboAndDeckCollictonMainHandler : MonoBehaviour
    {
        [Header("Scripts Reference")]
        [SerializeField] private ComboUIScrollPanelManager _comboUIScroll;
        [SerializeField] private CardUIScrollPanelManager _cardUIScroll;
        [SerializeField] private CardDataFilterSystem _cardDataFilter;
        [SerializeField] private ComboDataFilterSystem _comboDataFilter;

        [Header("Filter Button")] [SerializeField]
        private ComboAndDeckFilterButton _filterButton;
        
        private CardDataSort _cardDataSort;
        private ComboDataSort _comboDataSort;
        
        private List<CardData> _cardDatas;
        private List<ComboData> _comboDatas;

        private void Awake()
        {
            _cardDataSort = new CardDataSort();
            _comboDataSort = new ComboDataSort();
        }

        private void Start()
        {
            _filterButton.ComboState.OnClick += FilterCombos;
            _filterButton.DeckState.OnClick += FilterCard;
            
            ShowCombo();
            ShowCard();
        }

        private void FilterCombos()
        {
            _comboDataFilter.CycleFilter();
            ShowCombo();
        }

        private void FilterCard()
        {
            _cardDataFilter.CycleFilter();
            ShowCard();
        }

        private void ShowCombo()
        {
            _comboUIScroll.RemoveAllObjectsFromPanel();
            _comboUIScroll.AddObjectToPanel(_comboDataSort.SortComboData(_comboDataFilter.Filter(_comboDatas)));
        }

        private void ShowCard()
        {
            _cardUIScroll.RemoveAllObjectsFromPanel();
            _cardUIScroll.AddObjectToPanel(_cardDataSort.SortCardData(_cardDataFilter.Filter(_cardDatas)));
        }

        private void OnDestroy()
        {
            _filterButton.ComboState.OnClick -= _comboDataFilter.CycleFilter;
            _filterButton.DeckState.OnClick -= _cardDataFilter.CycleFilter;
        }

        public void Init(List<CardData> cardDatas, List<ComboData> comboDatas)
        {
            _cardDatas = cardDatas;
            _comboDatas = comboDatas;
            _cardUIScroll.Init();
            _comboUIScroll.Init();
        }
    }
}
