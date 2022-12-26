using System.Collections.Generic;
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
        [SerializeField] private DeckEditingDataManager _collectionData;
        [SerializeField] private DeckContinaerUIHandler _deckContinaer;
        [SerializeField] private MetaCollectionHandler _metaCollectionHandler;
       
        [Header("Title")]
        [SerializeField] private InputFieldHandler _deckName;
        
        private DeckBuilder _deckBuilder;
        
        private AccountDataCollectionHelper _accountDataCollectionHelper;

        private MetaAccountData _metaAccountData;

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
            _collectionData = data.MetaDataManager.DeckEditingDataManager;
            _metaAccountData = data.MetaDataManager.MetaAccountData;
            _accountDataCollectionHelper = data.MetaDataManager.AccountDataCollectionHelper;

            _metaCardUis = VisualRequesterManager.Instance.GetMetaCardUIs(8).ToArray();
            _metaComboUis = VisualRequesterManager.Instance.GetMetaComboUIs(3).ToArray();

            _deckContinaer.Init(_metaCardUis,_metaComboUis);
            _metaCollectionHandler.Init();

            _deckName.OnValueChange += _deckBuilder.TryEditDeckName;

            _collectionData.OnSuccessUpdateDeck += ExitDeckEditing;
            _collectionData.OnFailedUpdateDeck += _clickHelper.Open;
            
            _deckBuilder.OnSuccessCardAdd += _deckContinaer.AddCardUI;
            _deckBuilder.OnSuccessCardRemove += _deckContinaer.RemoveCardUI;
            _deckBuilder.OnSuccessComboAdd += _deckContinaer.AddComboUI;
            _deckBuilder.OnSuccessComboRemove += _deckContinaer.RemoveComboUI;
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
            
            _metaCollectionCardUIs = VisualRequesterManager.Instance.GetMetaCollectionCardUI(_accountDataCollectionHelper.CurrentCollectionCardDatas);
            _metaComboCollectionUIs = VisualRequesterManager.Instance.GetMetaCollectionComboUis(_accountDataCollectionHelper.CurrentCollectionComboDatas);

            _metaCollectionHandler.LoadObjects(_metaCollectionCardUIs,_metaComboCollectionUIs);

            if (metaDeckData.IsNewDeck)
                return;
            
            foreach (var cardData in metaDeckData.Cards)
                _deckContinaer.AddCardUI(cardData);
            
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
            _deckBuilder.OnSuccessCardAdd -= _deckContinaer.AddCardUI;
            _deckBuilder.OnSuccessCardRemove -= _deckContinaer.RemoveCardUI;
            _deckBuilder.OnSuccessComboAdd -= _deckContinaer.AddComboUI;
            _deckBuilder.OnSuccessComboRemove -= _deckContinaer.RemoveComboUI;
            
            _collectionData.OnSuccessUpdateDeck -= ExitDeckEditing;
            _collectionData.OnFailedUpdateDeck += _clickHelper.Open;
        }
    }
}

