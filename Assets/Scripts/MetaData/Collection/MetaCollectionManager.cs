using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaData.DeckBuilder;
using UnityEngine;

namespace CardMaga.MetaData.Collection
{
    public class MetaCollectionManager : MonoBehaviour
    {
        [SerializeField] private AccountDataAccess _accountDataAccess;
        
        [SerializeField] private MetaCardUIPool _cardUIPool;
        [SerializeField] private MetaComboUIPool _comboUIPool;
        private AccountDataCollectionHelper _accountDataCollectionHelper;
        private DeckBuilder _deckBuilder;
        
    
        void Start()
        {
            _accountDataCollectionHelper = new AccountDataCollectionHelper(_accountDataAccess);
            _deckBuilder = new DeckBuilder(_accountDataCollectionHelper);
            _metaCollectionDeck = new MetaCollectionDeckUIHandler();
            _deckBuilder.AssingDeckToEdit(_accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0]);
            _cardCollectionPanelHandler.Init();
            _comboCollectionPanelHandler.Init();
            _cardUIPool.Init();
            _comboUIPool.Init();
            LoadObjects();
        }

        public void LoadObjects()
        {
            _metaCollectionDeck.AddComboToSlot(_comboUIPool.PullObjects(_accountDataCollectionHelper.MetaComboDatas));
            _metaCollectionDeck.AddCardToSlot(_cardUIPool.PullObjects(_accountDataCollectionHelper.DeckData));
            _cardCollectionPanelHandler.AddObjectToPanel(_accountDataCollectionHelper.CollectionCardDatas);
            _comboCollectionPanelHandler.AddObjectToPanel(_accountDataAccess.AccountData.AccountCombos);//need to move from start
        }
    
    }
}

