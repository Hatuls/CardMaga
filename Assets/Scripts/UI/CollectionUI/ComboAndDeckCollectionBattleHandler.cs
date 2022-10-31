using Battle;
using Battle.Deck;
using CardMaga.SequenceOperation;
using CardMaga.UI.ScrollPanel;
using Managers;
using ReiTools.TokenMachine;
using UnityEngine;

public class ComboAndDeckCollectionBattleHandler : MonoBehaviour , ISequenceOperation<IBattleManager>
{
    [SerializeField] private ComboAndDeckCollectionHandler _collection;

    private DeckHandler _deckHandler;
    private PlayerComboContainer _playerComboContainer;

    private void Awake()
    {
        BattleManager.Register(this,OrderType.After);
    }
    
    public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
    {
        IPlayer player = data.PlayersManager.RightCharacter;

        _deckHandler = player.DeckHandler;
        _playerComboContainer = player.Combos;
        _collection.Init();  
        _collection.AssignComboData(_playerComboContainer);
        _collection.AssignCardData(_deckHandler);
    }
    
    public void SetExhaustDeck()
    {
        _collection.AssignCardData(_deckHandler[DeckEnum.Exhaust]);
    }
        
    public void SetPlayerDeck()
    {
        _collection.AssignCardData(_deckHandler[DeckEnum.PlayerDeck]);
    }
        
    public void SetDiscardDeck()
    {
        _collection.AssignCardData(_deckHandler[DeckEnum.Discard]);
    }



    public int Priority
    {
        get => 0;
    }
}
