using System.Collections.Generic;
using Account.GeneralData;
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
        
        private VisualRequester<BattleComboUI, ComboCore> _visualRequesterCombo;
        private VisualRequester<BattleCardUI, CardCore> _visualRequesterCard;

        private void ShowCombo()
        {
            battleComboUIScroll.RemoveAllObjectsFromPanel();

            List<ComboCore> comboCores = new List<ComboCore>();
            List<BattleComboData> battleComboDatas;

            battleComboDatas = _comboDataSort.SortComboData(_comboDatas.GetCollection);

            foreach (var comboData in battleComboDatas)
            {
                comboCores.Add(comboData.ComboCore);
            }
            
            var comboVisual =  _visualRequesterCombo.GetVisual(comboCores);

            var comboUIElement = comboVisual.ConvertAll(x => (IUIElement) x);
            
            battleComboUIScroll.AddObjectToPanel(comboUIElement);
        }

        private void ShowCard()
        {
            battleCardUIScroll.RemoveAllObjectsFromPanel();

            List<CardCore> cardCores = new List<CardCore>();
            List<BattleCardData> battleCardDatas;


            battleCardDatas = _battleCardDataSort.SortCardData(_cardDatas.GetCollection);
            
            foreach (var cardData in battleCardDatas)
            {
                cardCores.Add(cardData.CardInstance.CardCore);
            }
            
            var cardVisual = _visualRequesterCard.GetVisual(cardCores);

            if (cardVisual == null)
                return;
            
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

            _visualRequesterCard = new VisualRequester<BattleCardUI, CardCore>(_battleCardUI);
            _visualRequesterCombo = new VisualRequester<BattleComboUI, ComboCore>(_battleComboUI);

            battleCardUIScroll.Init();
            battleComboUIScroll.Init();
        }
    }
}