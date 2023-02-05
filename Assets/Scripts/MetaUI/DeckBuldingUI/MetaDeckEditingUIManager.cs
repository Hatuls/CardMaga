using CardMaga.Input;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.ObjectPool;
using CardMaga.SequenceOperation;
using CardMaga.UI;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using Account.GeneralData;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaDeckEditingUIManager : BaseUIScreen, ISequenceOperation<MetaUIManager>
    {
        public event Action<MetaDeckEditingUIManager> OnDeckBuildingInitiate;

        [SerializeField] private UnityEvent OnExitDeckEditing;

        [Header("Scrips Ref")]
        [SerializeField] private ClickHelper _defaultDeckClickHelper;
        [SerializeField] private ClickHelper _deckClickHelper;
        [SerializeField] private MetaDeckEditingDataManager _dataManager;
        [SerializeField] private DeckContianerUIHandler _deckContinaer;
        [SerializeField] private MetaCollectionUIHandler _metaCollectionHandler;

        [Header("Title")]
        [SerializeField] private InputFieldHandler _deckName;

        private DeckBuilder _deckBuilder;
        private bool _isFirstTime;

        private List<MetaCardUI> _metaCardUis;
        private List<MetaComboUI> _metaComboUis;
        private List<MetaCollectionCardUI> _metaCollectionCardUIs;
        private List<MetaCollectionComboUI> _metaComboCollectionUIs;
        
        public int Priority => 1;
        public IReadOnlyList<MetaComboUI> InDeckCombosUI => _metaComboUis;
        public IReadOnlyList<MetaCardUI> InDeckCardsUI => _metaCardUis;
        public IReadOnlyList<MetaCollectionCardUI> InCollectionCardsUI => _metaCollectionCardUIs;
        public IReadOnlyList<MetaCollectionComboUI> InCollectionCombosUI => _metaComboCollectionUIs;

        public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
        {
            _isFirstTime = true;

            _deckBuilder = data.MetaDataManager.DeckBuilder;
            _dataManager = data.MetaDataManager.MetaDeckEditingDataManager;

            _metaCardUis = new List<MetaCardUI>();
            _metaComboUis = new List<MetaComboUI>();
            
            _metaCollectionHandler.Init();
            _deckContinaer.Init();

            _deckName.OnValueChange += _deckBuilder.TryEditDeckName;

            _dataManager.OnFailedUpdateDeck += FailedToUpdateDeck;
            _dataManager.OnSuccessUpdateDeck += ExitAndDiscardDeck;

            _deckBuilder.OnSuccessfulCardAdd += AddCardUIToContainer;
            _deckBuilder.OnSuccessfulCardRemove += RemoveCardUIFromContainer;
            _deckBuilder.OnSuccessfulComboAdd += AddComboUiToContainer;
            _deckBuilder.OnSuccessfulComboRemove += RemoveComboUIFromContainer;
        }

        public override void Show()//plaster 10.01.23
        {
            if (_isFirstTime)
                _isFirstTime = false;
            else
                DiscardDeck();
            
            _dataManager.AssignDeckDataToEdit();
            SetDeckToEdit(_dataManager.MetaDeckData);
            
            base.Show();
        }

        private void SetDeckToEdit(MetaDeckData metaDeckData)
        {
            _deckName.SetText(metaDeckData.DeckName);

            _metaCollectionCardUIs = VisualRequesterManager.Instance.GetMetaCollectionCardUI(_dataManager.CardCollectionDataHandler.CollectionCardDatas.Values.ToList());
            _metaComboCollectionUIs = VisualRequesterManager.Instance.GetMetaCollectionComboUis(_dataManager.ComboCollectionDataHandler.CollectionComboDatas);
            
            _metaCollectionHandler.LoadObjects(_metaCollectionCardUIs, _metaComboCollectionUIs);

            if (metaDeckData.IsNewDeck)
                return;

            _metaCardUis = VisualRequesterManager.Instance.GetMetaCardUIs(metaDeckData.Cards);
            _metaComboUis = VisualRequesterManager.Instance.GetMetaComboUIs(metaDeckData.Combos);
            
            _deckContinaer.Init(_metaCardUis, _metaComboUis);
            
            ResetInputs();

            OnDeckBuildingInitiate?.Invoke(this);
        }

        #region ContinerManagers

        private void AddCardUIToContainer(CardInstance cardInstance)
        {
            var metaCardUIs = VisualRequesterManager.Instance.GetMetaCardUIs(cardInstance);
            _metaCardUis.Add(metaCardUIs);
            _deckContinaer.AddCardUI(metaCardUIs);
        }
        
        private void AddComboUiToContainer(ComboInstance comboInstance)
        {
            var metaComboUIs = VisualRequesterManager.Instance.GetMetaComboUIs(comboInstance);
            _metaComboUis.Add(metaComboUIs);
            _deckContinaer.AddComboUI(metaComboUIs);
        }

        private void RemoveCardUIFromContainer(CardInstance cardInstance)
        {
            var metaCardUi = FindCardUI(cardInstance);

            _metaCardUis.Remove(metaCardUi);
            
            _deckContinaer.RemoveCardUI(metaCardUi);
        }
        
        private void RemoveComboUIFromContainer(ComboInstance comboInstance)
        {
            var metaComboUI = FindComboUI(comboInstance);

            _metaComboUis.Remove(metaComboUI);
            
            _deckContinaer.RemoveComboUI(metaComboUI);
        }

        #endregion
        
        private void ResetInputs()
        {
            foreach (var input in CardsInputs())
                input.ForceResetInputBehaviour();


            IEnumerable<CardUIInputHandler> CardsInputs()
            {
                foreach (var card in InDeckCardsUI)
                    yield return card.CardUI.Inputs;

                foreach (var card in InCollectionCardsUI)
                    yield return card.CardUI.Inputs;
            }
        }

        private void DiscardDeck()
        {
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
        
        public void TryExitDeckEditing()
        {
            _dataManager.ExitDeckEditing();
        }

        public void ExitAndUpdateDeck()
        {
            _dataManager.UpdateDeck();
        }

        public void ExitAndDiscardDeck()
        {
            _dataManager.DiscardDeck();
            DiscardDeck();
            CloseScreen();
        }

        private void FailedToUpdateDeck(bool isDefaultDeck)
        {
            if (isDefaultDeck)
            {
                _defaultDeckClickHelper.Open();
            }
            else
            {
                _deckClickHelper.Open();
            }
        }

        private void OnDestroy()
        {
            _dataManager.Dispose();
            _deckName.OnValueChange -= _deckBuilder.TryEditDeckName;//paster
            _deckBuilder.OnSuccessfulCardAdd -= AddCardUIToContainer;
            _deckBuilder.OnSuccessfulCardRemove -= RemoveCardUIFromContainer;
            _deckBuilder.OnSuccessfulComboAdd -= AddComboUiToContainer;
            _deckBuilder.OnSuccessfulComboRemove -= RemoveComboUIFromContainer;
            
            _dataManager.OnSuccessUpdateDeck -= ExitAndDiscardDeck;
            _dataManager.OnFailedUpdateDeck -= FailedToUpdateDeck;
        }
        
        private MetaComboUI FindComboUI(ComboInstance comboData) => _metaComboUis.TakeWhile(metaComboUI => !ReferenceEquals(metaComboUI, null)).FirstOrDefault(metaComboUI => comboData.CoreID == metaComboUI.ComboData.CoreID);
        private MetaCardUI FindCardUI(CardInstance cardData) => _metaCardUis.TakeWhile(metaCardUi => !ReferenceEquals(metaCardUi, null)).FirstOrDefault(metaCardUi => cardData.CoreID == metaCardUi.CardInstance.CoreID);

    }
}

