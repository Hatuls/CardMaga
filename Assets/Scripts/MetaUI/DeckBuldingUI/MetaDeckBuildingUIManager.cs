﻿using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.ObjectPool;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaDeckBuildingUIManager : MonoBehaviour , ISequenceOperation<MetaUIManager>
    {
        [SerializeField] private UnityEvent OnExitDeckEditing;
        
        [Header("Scrips Ref")]
        [SerializeField] private ClickHelper _clickHelper;
        [SerializeField] private MetaDeckEditingDataManager _dataManager;
        [SerializeField] private DeckContinaerUIHandler _deckContinaer;
        [SerializeField] private MetaCollectionHandler _metaCollectionHandler;
       
        [Header("Title")]
        [SerializeField] private InputFieldHandler _deckName;
        
        private DeckBuilder _deckBuilder;
        private bool _isFirstTime;

        private MetaCardUI[] _metaCardUis;
        private MetaComboUI[] _metaComboUis;
        private List<MetaCollectionCardUI> _metaCollectionCardUIs;
        private List<MetaCollectionComboUI> _metaComboCollectionUIs;
        public int Priority => 1;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
        {
            _isFirstTime = true;
            
            _deckBuilder = data.MetaDataManager.DeckBuilder;
            _dataManager = data.MetaDataManager.MetaDeckEditingDataManager;
            
            _metaCardUis = VisualRequesterManager.Instance.GetMetaCardUIs(8).ToArray();
            _metaComboUis = VisualRequesterManager.Instance.GetMetaComboUIs(3).ToArray();

            _deckContinaer.Init(_metaCardUis,_metaComboUis);
            _metaCollectionHandler.Init();

            _deckName.OnValueChange += _deckBuilder.TryEditDeckName;

            _dataManager.OnSuccessUpdateDeck += ExitDeckEditing;
            _dataManager.OnFailedUpdateDeck += _clickHelper.Open;
            
            _deckBuilder.OnSuccessfulCardAdd += _deckContinaer.AddOnsuccessfulCardUI;
            _deckBuilder.OnSuccessfulCardRemove += _deckContinaer.RemoveCardUI;
            _deckBuilder.OnSuccessfulComboAdd += _deckContinaer.AddComboUI;
            _deckBuilder.OnSuccessfulComboRemove += _deckContinaer.RemoveComboUI;
        }

        private void OnEnable()
        {
            DiscardDeck();
            _dataManager.AssingDeckDataToEdit();
            SetDeckToEdit(_dataManager.MetaDeckData);
        }

        private void Start()
        {
            if (_isFirstTime)
                _isFirstTime = false;
        }

        private void SetDeckToEdit(MetaDeckData metaDeckData)
        {
            _deckName.SetText(metaDeckData.DeckName);//all plaster
            
            _metaCollectionCardUIs = VisualRequesterManager.Instance.GetMetaCollectionCardUI(_dataManager.CardCollectionDataHandler.CollectionCardDatas);
            _metaComboCollectionUIs = VisualRequesterManager.Instance.GetMetaCollectionComboUis(_dataManager.ComboCollectionDataHandler.CollectionComboDatas);

            _metaCollectionHandler.LoadObjects(_metaCollectionCardUIs,_metaComboCollectionUIs);

            if (metaDeckData.IsNewDeck)
                return;
            
            foreach (var cardData in metaDeckData.Cards)
                _deckContinaer.AddOnsuccessfulCardUI(cardData);
            
            foreach (var comboData in metaDeckData.Combos)
                _deckContinaer.AddComboUI(comboData);
        }
        
        private void DiscardDeck()
        {
            if (_isFirstTime)
                return;

            _metaCollectionHandler.UnLoadObjects();
            
            _deckContinaer.UnLoadObjects();
            
            foreach (var collectionCardUI in _metaCollectionCardUIs)
            {
                collectionCardUI.Dispose();
            }

            foreach (var collectionUICombo in _metaComboCollectionUIs)
            {
                collectionUICombo.Dispose();
            }
        }

        
        public void TryExitDeckEditing() => _dataManager.ExitDeckEditing();
        public void ExitDeckEditing() => OnExitDeckEditing.Invoke();

        public void ExitAndDiscardDeck()
        {
            _dataManager.DiscardDeck();
            DiscardDeck();
            gameObject.SetActive(false);
        }
        
        private void OnDestroy()
        {
            _dataManager.Dispose();
            _deckName.OnValueChange -= _deckBuilder.TryEditDeckName;//paster
            _deckBuilder.OnSuccessfulCardAdd -= _deckContinaer.AddOnsuccessfulCardUI;
            _deckBuilder.OnSuccessfulCardRemove -= _deckContinaer.RemoveCardUI;
            _deckBuilder.OnSuccessfulComboAdd -= _deckContinaer.AddComboUI;
            _deckBuilder.OnSuccessfulComboRemove -= _deckContinaer.RemoveComboUI;
            
            _dataManager.OnSuccessUpdateDeck -= ExitDeckEditing;
            _dataManager.OnFailedUpdateDeck += _clickHelper.Open;
        }
    }
}

