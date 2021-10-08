using TMPro;
using UnityEngine;

namespace Battles.UI
{
    public class CraftingUIManager : MonoSingleton<CraftingUIManager>
{
        CraftingUIHandler _playerCraftingUIHandler;
        CraftingUIHandler _opponentCraftingUIHandler;

        [SerializeField] CraftingSlotUI[] _playerCraftingSlotsUI;
        [SerializeField] RectTransform _playersfirstSlotTransform;
        [SerializeField] CraftingSlotUI _fadeOutCraftingSlots;

        [SerializeField] CraftingSlotUI[] _opponentCraftingSlotsUI;
        [SerializeField] RectTransform _opponentfirstSlotTransform;
        [SerializeField] CraftingSlotUI _opponentfadeOutCraftingSlots;



        [SerializeField] float moveLeanTweenTime;

        public CraftingUIHandler GetCharacterUIHandler(bool players)
        {
            Init();
            return players ? _playerCraftingUIHandler : _opponentCraftingUIHandler;
        }




        public override void Init()
        {
            if (_playerCraftingUIHandler == null)
            {
                _playerCraftingUIHandler = new CraftingUIHandler(_playerCraftingSlotsUI, _fadeOutCraftingSlots, _playersfirstSlotTransform, moveLeanTweenTime, true );
                _playerCraftingUIHandler.ResetAllSlots();
            }
            if (_opponentCraftingUIHandler == null)
            {
                _opponentCraftingUIHandler = new CraftingUIHandler(_opponentCraftingSlotsUI, _opponentfadeOutCraftingSlots, _opponentfirstSlotTransform, moveLeanTweenTime, false);
                _opponentCraftingUIHandler.ResetAllSlots();
            }
        }
}
}