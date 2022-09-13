using Battle;
using Battle.Deck;
using CardMaga.UI;
using CardMaga.UI.Carfting;
using Managers;
using ReiTools.TokenMachine;
using UnityEngine;

public class CraftingSlotsUIManager_V4 : MonoBehaviour , ISequenceOperation<BattleManager>
{
    [SerializeField] private CraftingSlotHandler_V4 _playerSlots;
    [SerializeField] private CraftingSlotHandler_V4 _enemySlots;


    public int Priority => 0;
    private IPlayersManager _playersManager;
    public void ExecuteTask(ITokenReciever tokenMachine, BattleManager data)
    {
        _playersManager = data.PlayersManager;
        _playersManager.GetCharacter(true).CraftingHandler.OnCraftingSlotsReset += _playerSlots.RestCraftingSlots;
        _playersManager.GetCharacter(false).CraftingHandler.OnCraftingSlotsReset += _enemySlots.RestCraftingSlots;
    }

    public void Awake()
    {
        HandUI.OnCardSelect += _playerSlots.LoadCraftingSlot;
        HandUI.OnCardReturnToHand += _playerSlots.CancelLoadSlot;
        CardExecutionManager.OnPlayerCardExecute += _playerSlots.ApplySlot;
        BattleManager.Register(this, OrderType.Default);

        CardExecutionManager.OnEnemyCardExecute += _enemySlots.ApplyEnemySlot;
    }

    public void OnDestroy()
    {
        HandUI.OnCardSelect -= _playerSlots.LoadCraftingSlot;
        HandUI.OnCardReturnToHand -= _playerSlots.CancelLoadSlot;
        CardExecutionManager.OnPlayerCardExecute -= _playerSlots.ApplySlot;

        _playersManager.GetCharacter(true).CraftingHandler.OnCraftingSlotsReset  -= _playerSlots.RestCraftingSlots;
        _playersManager.GetCharacter(false).CraftingHandler.OnCraftingSlotsReset -= _enemySlots.RestCraftingSlots;

        CardExecutionManager.OnEnemyCardExecute -= _enemySlots.ApplyEnemySlot;
    }


}
