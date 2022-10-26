using Battle.Combo;
using CardMaga.Card;
using CardMaga.Collection;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class ComboAndDeckCollectionMainHandler : MonoBehaviour
    {
        [Header("Scripts Reference")]
        [SerializeField] private ComboUIScrollPanelManager _comboUIScroll;
        [SerializeField] private CardUIScrollPanelManager _cardUIScroll;
        [SerializeField] private CardDataFilterSystem _cardDataFilter;

        private CardDataSort _cardDataSort;
        private ComboDataSort _comboDataSort;
        
        private IGetSourceCollection<CardData> _cardDatas;
        private IGetSourceCollection<ComboData> _comboDatas;

        private void Awake()
        {
            _cardDataSort = new CardDataSort();
            _comboDataSort = new ComboDataSort();
        }

        private void Start()
        {
            _cardDataFilter.OnCycleFilter += ShowCard;
            
            ShowCombo();
            ShowCard();
        }
        
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

        public void Init(IGetSourceCollection<CardData> cardDatas, IGetSourceCollection<ComboData> comboDatas)
        {
            _cardDatas = cardDatas;
            _comboDatas = comboDatas;
            _cardUIScroll.Init();
            _comboUIScroll.Init();
        }
    }
}
