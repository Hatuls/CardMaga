using System.Linq;
using Battle;
using CardMaga.SequenceOperation;
using CardMaga.UI.ScrollPanel;
using Managers;
using ReiTools.TokenMachine;
using UnityEngine;

public class ComboAndDeckCollictionBattleHandler : MonoBehaviour , ISequenceOperation<IBattleManager>
{
    [SerializeField] private ComboAndDeckCollictonMainHandler _collicton;

    private void Awake()
    {
        BattleManager.Register(this,OrderType.After);
    }
    
    public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
    {
        IPlayer player = data.PlayersManager.RightCharacter;
        
        _collicton.Init(player.DeckHandler.GetAllCardData.ToList(),player.Combos.ToList());        
    }

    public int Priority
    {
        get => 0;
    }
}
