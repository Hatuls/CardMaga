using Battle;
using CardMaga.UI;
using CardMaga.UI.Carfting;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using System;
using UnityEngine;

public class CraftingSlotsUIManager_V4 : MonoBehaviour, ISequenceOperation<IBattleManager>
{
    [SerializeField] private CraftingSlotHandler_V4 _playerSlots;
    [SerializeField] private CraftingSlotHandler_V4 _enemySlots;


    public int Priority => 0;
    private IPlayersManager _playersManager;
    public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
    {
        _playersManager = data.PlayersManager;
        var left = _playersManager.GetCharacter(true);
        var right = _playersManager.GetCharacter(false);
        left.CraftingHandler.OnCraftingSlotsReset += _playerSlots.RestCraftingSlots;
        right.CraftingHandler.OnCraftingSlotsReset += _enemySlots.RestCraftingSlots;
        data.CardExecutionManager.OnPlayerCardExecute += _playerSlots.ApplySlot;
        data.CardExecutionManager.OnEnemyCardExecute += _enemySlots.ApplyEnemySlot;
        data.OnBattleManagerDestroyed += BeforeGameFinished;
        HandUI.OnCardSelect += _playerSlots.LoadCraftingSlot;
        HandUI.OnCardSetToHandState += _playerSlots.CancelLoadSlot;
    }

    private void BeforeGameFinished(IBattleManager data)
    {
        data.OnBattleManagerDestroyed -= BeforeGameFinished;
        data.CardExecutionManager.OnPlayerCardExecute -= _playerSlots.ApplySlot;
        data.CardExecutionManager.OnEnemyCardExecute  -= _enemySlots.ApplyEnemySlot;
        HandUI.OnCardSelect -= _playerSlots.LoadCraftingSlot;
        HandUI.OnCardSetToHandState -= _playerSlots.CancelLoadSlot;
        _playersManager.GetCharacter(true).CraftingHandler.OnCraftingSlotsReset -= _playerSlots.RestCraftingSlots;
        _playersManager.GetCharacter(false).CraftingHandler.OnCraftingSlotsReset -= _enemySlots.RestCraftingSlots;
    }

    public void Awake()
    {


        BattleManager.Register(this, OrderType.Default);

    }



}
