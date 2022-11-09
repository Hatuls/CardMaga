using CardMaga.Meta.AccountMetaData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaData.DeckBuilder;
using CardMaga.UI.MetaUI;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

public class MetaCollectionManager : MonoBehaviour
{
    [SerializeField] private AccountDataAccess _accountDataAccess;
    [SerializeField] private MetaComboUIScrollHandler _comboScrollPanelHandler;
    [SerializeField] private MetaCardUIScrollHandler _cardScrollPanelHandler;
    [SerializeField] private MetaCardUIPool _cardUIPool;
    [SerializeField] private MetaComboUIPool _comboUIPool;
    private AccountDataHelper _accountDataHelper;
    private DeckBuilder _deckBuilder;
    private MetaCollectionDeckUIHandler _metaCollectionDeck;
    
    void Start()
    {
        _accountDataHelper = new AccountDataHelper(_accountDataAccess);
        _deckBuilder = new DeckBuilder(_accountDataHelper);
        _metaCollectionDeck = new MetaCollectionDeckUIHandler();
        _deckBuilder.AssingDeckToEdit(_accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0]);
        _cardScrollPanelHandler.Init();
        _comboScrollPanelHandler.Init();
        _cardUIPool.Init();
        _comboUIPool.Init();
        LoadObjects();
    }

    public void LoadObjects()
    {
        _metaCollectionDeck.AddComboToSlot(_comboUIPool.PullObjects(_accountDataHelper.MetaComboDatas));
        _metaCollectionDeck.AddCardToSlot(_cardUIPool.PullObjects(_accountDataHelper.DeckData));
        _cardScrollPanelHandler.AddObjectToPanel(_accountDataHelper.CollectionCardDatas);
        _comboScrollPanelHandler.AddObjectToPanel(_accountDataAccess.AccountData.AccountCombos);//need to move from start
    }
    
}
