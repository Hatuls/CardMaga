using System.Collections.Generic;
using CardMaga.InventorySystem;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.ObjectPool;
using UnityEngine;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaUICollectionManager : MonoBehaviour
    {
        [SerializeField] private MetaCardUI _cardPrefabRef;
        [SerializeField] private MetaComboUI _comboPrefabRef;
        
        [SerializeField] private MetaCardUICollectionScrollPanel metaCardUICollectionScrollPanel;
        [SerializeField] private MetaComboUICollectionScrollPanel metaComboUICollectionScrollPanel;
        [SerializeField] private MetaCardUIContainer _metaCardUIContainer;
        [SerializeField] private MetaComboUiContainer _metaComboUiContainer;
        
        private VisualRequester<MetaComboUI, MetaComboData> _comboVisualRequester;
        private VisualRequester<MetaCardUI, MetaCardData> _cardVisualRequester;
        
        private List<MetaCardUI> _metaCardUis;
        private List<MetaComboUI> _metaComboUis;

        public void Init(AccountDataCollectionHelper accountData,MetaAccountData metaAccountData)
        {
            _comboVisualRequester = new VisualRequester<MetaComboUI, MetaComboData>(_comboPrefabRef);
            _cardVisualRequester = new VisualRequester<MetaCardUI, MetaCardData>(_cardPrefabRef);

            _metaComboUis =  _comboVisualRequester.GetVisual(metaAccountData.CharacterDatas.CharacterData.Decks[0].Combos);
            _metaCardUis = _cardVisualRequester.GetVisual(metaAccountData.CharacterDatas.CharacterData.Decks[0].Cards);
            
            metaCardUICollectionScrollPanel.Init();
            metaComboUICollectionScrollPanel.Init();
            metaCardUICollectionScrollPanel.AddObjectToPanel(accountData.CollectionCardDatas);
            metaComboUICollectionScrollPanel.AddObjectToPanel(accountData.CollectionComboDatas);
            
            
            _metaComboUiContainer.InitializeSlots(_metaComboUis.ToArray());
            _metaCardUIContainer.InitializeSlots(_metaCardUis.ToArray());
        }

        public void AddCardUI(MetaCardData metaCardData)
        {
            if (!_metaCardUIContainer.GetEmptySlot(out var cardSlot))
                return;

            var cache = FindEmptyCardUI();  
            cache.AssignVisual(metaCardData);
            cardSlot.AssignValue(cache);   
            cache.Show();
        }

        public void RemoveCardUI(MetaCardData metaCardData)
        {
            _metaCardUIContainer.RemoveObject(FindCardUI(metaCardData));
        }

        public void AddComboUI(MetaComboData metaComboData)
        {
            if (!_metaComboUiContainer.GetEmptySlot(out var comboSlot))
                return;
            
            var cache = FindEmptyComboUI();    
            cache.AssignVisual(metaComboData);
            comboSlot.AssignValue(cache);   
            cache.Show();
        }

        public void RemoveComboUI(MetaComboData metaComboData)
        {
            _metaComboUiContainer.RemoveObject(FindComboUI(metaComboData));
        }

        private MetaCardUI FindCardUI(MetaCardData metaCardData)
        {
            foreach (var metaCardUi in _metaCardUis)
            {
                if (metaCardData.CardInstance.ID == metaCardUi.CardInstance.ID)
                {
                    return metaCardUi;
                }
            }

            return null;
        }
        
        private MetaCardUI FindEmptyCardUI()
        {
            foreach (var metaCardUi in _metaCardUis)
            {
                if (metaCardUi.IsEmpty)
                {
                    return metaCardUi;
                }
            }

            return null;
        }
        
        private MetaComboUI FindEmptyComboUI()
        {
            foreach (var metaComboUI in _metaComboUis)
            {
                if (metaComboUI.IsEmpty)
                {
                    return metaComboUI;
                }
            }

            return null;
        }

        private MetaComboUI FindComboUI(MetaComboData metaComboData)
        {
            foreach (var metaComboUI in _metaComboUis)
            {
                if (metaComboData.ID == metaComboUI.MetaComboData.ID)
                {
                    return metaComboUI;
                }
            }

            return null;
        }
    }
}

