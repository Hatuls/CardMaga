using System;
using System.Collections.Generic;
using System.Linq;
using CardMaga.InventorySystem;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaData.DeckBuilding;
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
        [SerializeField] private InputFieldHandler _deckName;
        
        private VisualRequester<MetaComboUI, MetaComboData> _comboVisualRequester;
        private VisualRequester<MetaCardUI, MetaCardData> _cardVisualRequester;
        
        private DeckBuilder _deckBuilder;

        private MetaCollectionDataManager _collectionData;

        private AccountDataCollectionHelper _accountDataCollectionHelper;

        private MetaAccountData _metaAccountData;

        private MetaCardUI[] _metaCardUis;
        private MetaComboUI[] _metaComboUis;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
        {
            _deckBuilder = data.MetaDataManager.DeckBuilder;
            _collectionData = data.MetaDataManager.MetaCollectionDataManager;
            _metaAccountData = data.MetaDataManager.MetaAccountData;
            _accountDataCollectionHelper = data.MetaDataManager.AccountDataCollectionHelper;
            
            _comboVisualRequester = new VisualRequester<MetaComboUI, MetaComboData>(_comboPrefabRef);
            _cardVisualRequester = new VisualRequester<MetaCardUI, MetaCardData>(_cardPrefabRef);
            
            _metaComboUis = _comboVisualRequester.GetVisual(_metaAccountData.CharacterDatas.CharacterData.MainDeck.Combos)
                .ToArray();
            _metaCardUis = _cardVisualRequester.GetVisual(_metaAccountData.CharacterDatas.CharacterData.MainDeck.Cards)
                .ToArray();
            
            _metaComboUiContainer.Init();
            _metaCardUIContainer.Init();
            metaCardUICollectionScrollPanel.Init();
            metaComboUICollectionScrollPanel.Init();
            
            _metaComboUiContainer.InitializeSlots(_metaComboUis);
            _metaCardUIContainer.InitializeSlots(_metaCardUis);

            _deckName.OnValueChange += _deckBuilder.TryEditDeckName;
            
            _deckBuilder.OnSuccessCardAdd += AddCardUI;
            _deckBuilder.OnSuccessCardRemove += RemoveCardUI;
            _deckBuilder.OnSuccessComboAdd += AddComboUI;
            _deckBuilder.OnSuccessComboRemove += RemoveComboUI;
        }

        private void OnEnable()
        {
            SetDeckToEdit(_metaAccountData.CharacterDatas.CharacterData.MainDeck);
        }

        private void SetDeckToEdit(MetaDeckData metaDeckData)
        {
            _accountDataCollectionHelper.UpdateCollection();
            _collectionData.AssingDeckDataToEdit();

            _deckName.SetText(metaDeckData.DeckName);//all plaster
            
            metaCardUICollectionScrollPanel.RemoveAllObjectsFromPanel();
            metaComboUICollectionScrollPanel.RemoveAllObjectsFromPanel();
            
            _metaComboUiContainer.Reset();
            _metaCardUIContainer.Reset();
            
            metaCardUICollectionScrollPanel.AddObjectToPanel(_accountDataCollectionHelper.CollectionCardDatas);
            metaComboUICollectionScrollPanel.AddObjectToPanel(_accountDataCollectionHelper.CollectionComboDatas);
            
            if (metaDeckData.IsNewDeck)
                return;
            
            foreach (var cardData in metaDeckData.Cards)
                AddCardUI(cardData);
            
            foreach (var comboData in metaDeckData.Combos)
                AddComboUI(comboData);
        }

        public int Priority => 1;

        private void AddCardUI(MetaCardData metaCardData)
        {
            if (!_metaCardUIContainer.TryGetEmptySlot(out var cardSlot))
                return;

            var cache = FindEmptyCard();
            cache.AssignVisual(metaCardData);
            cardSlot.AssignValue(cache);   
            cache.Show();
        }

        private void RemoveCardUI(MetaCardData metaCardData)
        {
            _metaCardUIContainer.RemoveObject(FindCardUI(metaCardData));
        }

        private void AddComboUI(MetaComboData metaComboData)
        {
            if (!_metaComboUiContainer.TryGetEmptySlot(out var comboSlot))
                return;

            var cache = FindEmptyCombo();
            cache.AssignVisual(metaComboData);
            comboSlot.AssignValue(cache);   
            cache.Show();
        }

        private void RemoveComboUI(MetaComboData metaComboData)
        {
            _metaComboUiContainer.RemoveObject(FindComboUI(metaComboData));
        }

        private MetaCardUI FindCardUI(MetaCardData metaCardData)
        {
            return _metaCardUis.TakeWhile(metaCardUi => !ReferenceEquals(metaCardUi, null)).FirstOrDefault(metaCardUi => metaCardData.CardInstance.ID == metaCardUi.CardInstance.ID && !metaCardUi.IsEmpty);
        }

        private MetaCardUI FindEmptyCard()
        {
            return _metaCardUis.TakeWhile(metaCardUi => !ReferenceEquals(metaCardUi, null)).FirstOrDefault(metaCardUi => metaCardUi.IsEmpty);
        }

        private MetaComboUI FindComboUI(MetaComboData metaComboData)
        {
            return _metaComboUis.TakeWhile(metaComboUI => !ReferenceEquals(metaComboUI, null)).FirstOrDefault(metaComboUI => metaComboData.ID == metaComboUI.MetaComboData.ID && !metaComboUI.IsEmpty);
        }

        private MetaComboUI FindEmptyCombo()
        {
            return _metaComboUis.TakeWhile(metaComboUI => !ReferenceEquals(metaComboUI, null)).FirstOrDefault(metaComboUI => metaComboUI.IsEmpty);
        }

        public void ExitDeckEditing()
        {
            _collectionData.ExitDeckEditing();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _collectionData.Dispose();
            _deckName.OnValueChange -= _deckBuilder.TryEditDeckName;//paster
            _deckBuilder.OnSuccessCardAdd -= AddCardUI;
            _deckBuilder.OnSuccessCardRemove -= RemoveCardUI;
            _deckBuilder.OnSuccessComboAdd -= AddComboUI;
            _deckBuilder.OnSuccessComboRemove -= RemoveComboUI;
        }
    }
}

