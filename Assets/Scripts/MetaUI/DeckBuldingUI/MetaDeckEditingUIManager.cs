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
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaDeckEditingUIManager : BaseUIScreen, ISequenceOperation<MetaUIManager>
    {
        public event Action<MetaDeckEditingUIManager> OnDeckBuildingInitiate;

        [SerializeField] private UnityEvent OnExitDeckEditing;

        [Header("Scrips Ref")]
        [SerializeField] private ClickHelper _clickHelper;
        [SerializeField] private MetaDeckEditingDataManager _dataManager;
        [SerializeField] private DeckContianerUIHandler _deckContinaer;
        [SerializeField] private MetaCollectionUIHandler _metaCollectionHandler;

        [Header("Title")]
        [SerializeField] private InputFieldHandler _deckName;

        private DeckBuilder _deckBuilder;
        private bool _isFirstTime;

        private MetaCardUI[] _metaCardUis;
        private MetaComboUI[] _metaComboUis;
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

            _metaCardUis = VisualRequesterManager.Instance.GetMetaCardUIs(8).ToArray();
            _metaComboUis = VisualRequesterManager.Instance.GetMetaComboUIs(3).ToArray();

            _deckContinaer.Init(_metaCardUis, _metaComboUis);
            _metaCollectionHandler.Init();

            _deckName.OnValueChange += _deckBuilder.TryEditDeckName;

            //   _dataManager.OnSuccessUpdateDeck += CloseScreen;
            _dataManager.OnFailedUpdateDeck += _clickHelper.Open;

            _deckBuilder.OnSuccessfulCardAdd += _deckContinaer.AddOnsuccessfulCardUI;
            _deckBuilder.OnSuccessfulCardRemove += _deckContinaer.RemoveCardUI;
            _deckBuilder.OnSuccessfulComboAdd += _deckContinaer.AddComboUI;
            _deckBuilder.OnSuccessfulComboRemove += _deckContinaer.RemoveComboUI;
        }

        public override void Show()//plaster 10.01.23
        {

            if (_isFirstTime)
                _isFirstTime = false;
            else
                DiscardDeck();

            _dataManager.AssingDeckDataToEdit();
            SetDeckToEdit(_dataManager.MetaDeckData);
            
            base.Show();
        }

        private void SetDeckToEdit(MetaDeckData metaDeckData)
        {
            _deckName.SetText(metaDeckData.DeckName);//all plaster

            _metaCollectionCardUIs = VisualRequesterManager.Instance.GetMetaCollectionCardUI(_dataManager.CardCollectionDataHandler.CollectionCardDatas);
            _metaComboCollectionUIs = VisualRequesterManager.Instance.GetMetaCollectionComboUis(_dataManager.ComboCollectionDataHandler.CollectionComboDatas);

            _metaCollectionHandler.LoadObjects(_metaCollectionCardUIs, _metaComboCollectionUIs);

            if (metaDeckData.IsNewDeck)
                return;

            foreach (var cardData in metaDeckData.Cards)
                _deckContinaer.AddOnsuccessfulCardUI(cardData);

            foreach (var comboData in metaDeckData.Combos)
                _deckContinaer.AddComboUI(comboData);

            ResetInputs();

            OnDeckBuildingInitiate?.Invoke(this);
        }

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
            if (_dataManager.ExitDeckEditing())
                CloseScreen();
        }
      
        public void ExitAndDiscardDeck()
        {
            _dataManager.DiscardDeck();
            DiscardDeck();
            CloseScreen();
        }

        private void OnDestroy()
        {
            _dataManager.Dispose();
            _deckName.OnValueChange -= _deckBuilder.TryEditDeckName;//paster
            _deckBuilder.OnSuccessfulCardAdd -= _deckContinaer.AddOnsuccessfulCardUI;
            _deckBuilder.OnSuccessfulCardRemove -= _deckContinaer.RemoveCardUI;
            _deckBuilder.OnSuccessfulComboAdd -= _deckContinaer.AddComboUI;
            _deckBuilder.OnSuccessfulComboRemove -= _deckContinaer.RemoveComboUI;

   //         _dataManager.OnSuccessUpdateDeck -= CloseScreen;
            _dataManager.OnFailedUpdateDeck -= _clickHelper.Open;
        }
    }
}

