using Battle;
using Battle.Deck;
using CardMaga.UI;
using CardMaga.UI.Carfting;
using UnityEngine;

public class CraftingSlotsUIManager_V4 : MonoBehaviour
{
    [SerializeField] private CraftingSlotHandler_V4 _playerSlots;
    [SerializeField] private CraftingSlotHandler_V4 _enemySlots;
    [SerializeField] private DeckManager _deckManager;

    private void Awake()
    {
        HandUI.OnCardSelect += _playerSlots.LoadCraftingSlot;
        HandUI.OnCardReturnToHand += _playerSlots.CancelLoadSlot;
        CardExecutionManager.OnPlayerCardExecute += _playerSlots.ApplySlot;

        PlayerCraftingSlots.OnResetCraftingSlot += ResetCraftingSlots;

        CardExecutionManager.OnEnemyCardExecute += _enemySlots.ApplyEnemySlot;
    }

    private void OnDestroy()
    {
        HandUI.OnCardSelect -= _playerSlots.LoadCraftingSlot;
        HandUI.OnCardReturnToHand -= _playerSlots.CancelLoadSlot;
        CardExecutionManager.OnPlayerCardExecute -= _playerSlots.ApplySlot;

        PlayerCraftingSlots.OnResetCraftingSlot -= ResetCraftingSlots;

        CardExecutionManager.OnEnemyCardExecute -= _enemySlots.ApplyEnemySlot;
    }

    private void ResetCraftingSlots(bool isPlayer)
    {
        if (isPlayer)
        {
            _playerSlots.RestCraftingSlots();
        }
        else
        {
            _enemySlots.RestCraftingSlots();
        }
    }
}
