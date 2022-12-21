using System;
using System.Collections.Generic;
using System.Linq;
using CardMaga.InventorySystem;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.ObjectPool;
using CardMaga.SequenceOperation;
using CardMaga.UI;
using CardMaga.UI.PopUp;
using ReiTools.TokenMachine;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaDeckCollectionUIManager : MonoBehaviour , ISequenceOperation<MetaUIManager>
    {
        [SerializeField] private UnityEvent OnExitDeckEditing;
        
        [SerializeField] private MetaCardUI _cardPrefabRef;
        [SerializeField] private MetaComboUI _comboPrefabRef;
        [SerializeField] private MetaCollectionCardUI _metaCollectionCardPrefabRef;
        [SerializeField] private MetaCollectionUICombo _collectionUIComboPrefabRef;
        
        [SerializeField] private ClickHelper _clickHelper;
        
        [SerializeField] private MetaCollectionDataManager _collectionData;
        
        [SerializeField] private MetaCardUICollectionScrollPanel metaCardUICollectionScrollPanel;
        [SerializeField] private MetaComboUICollectionScrollPanel metaComboUICollectionScrollPanel;
        [SerializeField] private MetaCardUIContainer _metaCardUIContainer;
        [SerializeField] private MetaComboUiContainer _metaComboUiContainer;
        [SerializeField] private InputFieldHandler _deckName;
        
        private VisualRequester<MetaComboUI, MetaComboData> _comboVisualRequester;
        private VisualRequester<MetaCardUI, MetaCardData> _cardVisualRequester;
        private VisualRequester<MetaCollectionCardUI, MetaCollectionCardData> _cardCollectionVisualRequester;
        private VisualRequester<MetaCollectionUICombo, MetaCollectionComboData> _comboCollectionVisualRequester;
        
        private DeckBuilder _deckBuilder;


        private AccountDataCollectionHelper _accountDataCollectionHelper;

        private MetaAccountData _metaAccountData;

        private bool _isFirstTime;

        private MetaCardUI[] _metaCardUis;
        private MetaComboUI[] _metaComboUis;
        private List<MetaCollectionCardUI> _metaCollectionCardUIs;
        private List<MetaCollectionUICombo> _metaComboCollectionUIs;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
        {
            _isFirstTime = true;
            
            _deckBuilder = data.MetaDataManager.DeckBuilder;
            _collectionData = data.MetaDataManager.MetaCollectionDataManager;
            _metaAccountData = data.MetaDataManager.MetaAccountData;
            _accountDataCollectionHelper = data.MetaDataManager.AccountDataCollectionHelper;
            
            _comboVisualRequester = new VisualRequester<MetaComboUI, MetaComboData>(_comboPrefabRef);
            _cardVisualRequester = new VisualRequester<MetaCardUI, MetaCardData>(_cardPrefabRef);
            _cardCollectionVisualRequester =
                new VisualRequester<MetaCollectionCardUI, MetaCollectionCardData>(_metaCollectionCardPrefabRef);
            _comboCollectionVisualRequester = new VisualRequester<MetaCollectionUICombo, MetaCollectionComboData>(_collectionUIComboPrefabRef);
            
            _metaComboUis = _comboVisualRequester.GetVisual(3).ToArray();
            _metaCardUis = _cardVisualRequester.GetVisual(8).ToArray();
            
            
            _metaComboUiContainer.InitializeSlots(_metaComboUis);
            _metaCardUIContainer.InitializeSlots(_metaCardUis);
            metaCardUICollectionScrollPanel.Init();
            metaComboUICollectionScrollPanel.Init();
            
            _metaComboUiContainer.Init();
            _metaCardUIContainer.Init();

            _deckName.OnValueChange += _deckBuilder.TryEditDeckName;

            _collectionData.OnSuccessUpdateDeck += ExitDeckEditing;
            _collectionData.OnFailedUpdateDeck += _clickHelper.Open;
            
            _deckBuilder.OnSuccessCardAdd += AddCardUI;
            _deckBuilder.OnSuccessCardRemove += RemoveCardUI;
            _deckBuilder.OnSuccessComboAdd += AddComboUI;
            _deckBuilder.OnSuccessComboRemove += RemoveComboUI;
        }

        private void OnEnable()
        {
            DiscardDeck();
            SetDeckToEdit(_metaAccountData.CharacterDatas.CharacterData.MainDeck);
        }

        private void Start()
        {
            if (_isFirstTime)
                _isFirstTime = false;
        }

        private void SetDeckToEdit(MetaDeckData metaDeckData)
        {
            _accountDataCollectionHelper.UpdateCollection();
            _collectionData.AssingDeckDataToEdit();

            _deckName.SetText(metaDeckData.DeckName);//all plaster
            
            _metaCollectionCardUIs =  _cardCollectionVisualRequester.GetVisual(_accountDataCollectionHelper.CollectionCardDatas);
            _metaComboCollectionUIs =
                _comboCollectionVisualRequester.GetVisual(_accountDataCollectionHelper.CollectionComboDatas);
            
            var cardUIElement = _metaCollectionCardUIs.ConvertAll(x => (IUIElement) x);
            var comboUIElement = _metaComboCollectionUIs.ConvertAll(x => (IUIElement) x);
            
            metaCardUICollectionScrollPanel.AddObjectToPanel(cardUIElement);
            metaComboUICollectionScrollPanel.AddObjectToPanel(comboUIElement);
            
            if (metaDeckData.IsNewDeck)
                return;
            
            foreach (var cardData in metaDeckData.Cards)
                AddCardUI(cardData);
            
            foreach (var comboData in metaDeckData.Combos)
                AddComboUI(comboData);
        }
        
        private void DiscardDeck()
        {
            if (_isFirstTime)
                return;

            metaCardUICollectionScrollPanel.RemoveAllObjectsFromPanel();
            metaComboUICollectionScrollPanel.RemoveAllObjectsFromPanel();
            
            _metaComboUiContainer.Reset();
            _metaCardUIContainer.Reset();
            
            foreach (var collectionCardUI in _metaCollectionCardUIs)
            {
                collectionCardUI.Dispose();
            }

            foreach (var collectionUICombo in _metaComboCollectionUIs)
            {
                collectionUICombo.Dispose();
            }
        }

        public int Priority => 1;

        private void AddCardUI(MetaCardData metaCardData)
        {
            if (!_metaCardUIContainer.TryGetEmptySlot(out var cardSlot))
                return;

            var cache = FindEmptyCard();
            _metaCardUIContainer.TryAddObject(cache);
            cache.AssignVisual(metaCardData);
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
            _metaComboUiContainer.TryAddObject(cache);  
            cache.Show();
        }

        private void RemoveComboUI(MetaComboData metaComboData)
        {
            _metaComboUiContainer.RemoveObject(FindComboUI(metaComboData));
        }

        private MetaCardUI FindCardUI(MetaCardData metaCardData) => _metaCardUis.TakeWhile(metaCardUi => !ReferenceEquals(metaCardUi, null)).FirstOrDefault(metaCardUi => metaCardData.CardInstance.ID == metaCardUi.CardInstance.ID && !metaCardUi.IsEmpty);
        private MetaCardUI FindEmptyCard() => _metaCardUis.TakeWhile(metaCardUi => !ReferenceEquals(metaCardUi, null)).FirstOrDefault(metaCardUi => metaCardUi.IsEmpty);
        private MetaComboUI FindComboUI(MetaComboData metaComboData) => _metaComboUis.TakeWhile(metaComboUI => !ReferenceEquals(metaComboUI, null)).FirstOrDefault(metaComboUI => metaComboData.ID == metaComboUI.MetaComboData.ID && !metaComboUI.IsEmpty);
        private MetaComboUI FindEmptyCombo() => _metaComboUis.TakeWhile(metaComboUI => !ReferenceEquals(metaComboUI, null)).FirstOrDefault(metaComboUI => metaComboUI.IsEmpty);
        
        public void TryExitDeckEditing() => _collectionData.ExitDeckEditing();
        public void ExitDeckEditing() => OnExitDeckEditing.Invoke();

        public void ExitAndDiscardDeck()
        {
            _collectionData.DiscardDeck();
            DiscardDeck();
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
            
            _collectionData.OnSuccessUpdateDeck -= ExitDeckEditing;
            _collectionData.OnFailedUpdateDeck += _clickHelper.Open;
        }
    }
}

