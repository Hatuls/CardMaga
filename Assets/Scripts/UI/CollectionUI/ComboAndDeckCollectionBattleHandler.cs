using Battle;
using CardMaga.SequenceOperation;
using CardMaga.UI.ScrollPanel;
using Managers;
using ReiTools.TokenMachine;
using UnityEngine;

public class ComboAndDeckCollectionBattleHandler : MonoBehaviour , ISequenceOperation<IBattleManager>
{
    [SerializeField] private ComboAndDeckCollectionMainHandler _collection;

    private void Awake()
    {
        BattleManager.Register(this,OrderType.After);
    }
    
    public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
    {
        IPlayer player = data.PlayersManager.RightCharacter;
        
        _collection.Init(player.DeckHandler,player.Combos);        
    }

    public int Priority
    {
        get => 0;
    }
}
