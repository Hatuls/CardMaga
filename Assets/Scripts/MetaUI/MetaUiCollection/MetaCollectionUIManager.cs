using System.Collections.Generic;
using CardMaga.InventorySystem;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.ObjectPool;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaCollectionUIManager : MonoBehaviour , ISequenceOperation<MetaUIManager>
    {
        [SerializeField] private MetaCardUI _cardPrefabRef;
        [SerializeField] private MetaComboUI _comboPrefabRef;
        
        [SerializeField] private MetaCardUICollectionScrollPanel metaCardUICollectionScrollPanel;
        [SerializeField] private MetaComboUICollectionScrollPanel metaComboUICollectionScrollPanel;
        [SerializeField] private MetaCardUIContainer _metaCardUIContainer;
        [SerializeField] private MetaComboUiContainer _metaComboUiContainer;
        
        private VisualRequester<MetaComboUI, MetaComboData> _comboVisualRequester;
        private VisualRequester<MetaCardUI, MetaCardData> _cardVisualRequester;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
        {
            AccountDataCollectionHelper accountDataCollectionHelper = data.MetaDataManager.AccountDataCollectionHelper;
            MetaAccountData metaAccountData = data.MetaDataManager.MetaAccountData;
            
            _comboVisualRequester = new VisualRequester<MetaComboUI, MetaComboData>(_comboPrefabRef);
            _cardVisualRequester = new VisualRequester<MetaCardUI, MetaCardData>(_cardPrefabRef);

            List<MetaComboUI> metaComboUis =  _comboVisualRequester.GetVisual(metaAccountData.CharacterDatas.CharacterData.Decks[0].Combos);
            List<MetaCardUI> metaCardUis = _cardVisualRequester.GetVisual(metaAccountData.CharacterDatas.CharacterData.Decks[0].Cards);
            
            metaCardUICollectionScrollPanel.Init();
            metaComboUICollectionScrollPanel.Init();
            metaCardUICollectionScrollPanel.AddObjectToPanel(accountDataCollectionHelper.CollectionCardDatas);
            metaComboUICollectionScrollPanel.AddObjectToPanel(accountDataCollectionHelper.CollectionComboDatas);
            
            
            _metaComboUiContainer.InitializeSlots(metaComboUis.ToArray());
            _metaCardUIContainer.InitializeSlots(metaCardUis.ToArray());
        }

        public int Priority => 1;

        public void AddCardUI(MetaCardData metaCardData)
        {
            if (!_metaCardUIContainer.TryGetEmptySlot(out var cardSlot))
                return;

            var cache = cardSlot.InventoryObject; 
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
            if (!_metaComboUiContainer.TryGetEmptySlot(out var comboSlot))
                return;

            var cache = comboSlot.InventoryObject;   
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
            MetaCardUI[] metaCardUis = _metaCardUIContainer.AllInventoryObject;
            
            foreach (var metaCardUi in metaCardUis)
            {
                if (metaCardData.CardInstance.ID == metaCardUi.CardInstance.ID)
                {
                    return metaCardUi;
                }
            }

            return null;
        }

        private MetaComboUI FindComboUI(MetaComboData metaComboData)
        {
            MetaComboUI[] metaComboUis = _metaComboUiContainer.AllInventoryObject;
            
            foreach (var metaComboUI in metaComboUis)
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

