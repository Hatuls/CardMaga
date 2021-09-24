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
        [SerializeField] Animator _playerAnimator;

        [SerializeField] CraftingSlotUI[] _opponentCraftingSlotsUI;
        [SerializeField] RectTransform _opponentfirstSlotTransform;
        [SerializeField] Animator _opponentAnimator;



        [SerializeField] float leanTweenTime;

        public CraftingUIHandler GetCharacterUIHandler(bool players)
        {
            Init();
            return players ? _playerCraftingUIHandler : _opponentCraftingUIHandler;
        }




        public override void Init()
        {
            if (_playerCraftingUIHandler == null)
            {
                _playerCraftingUIHandler = new CraftingUIHandler(_playerCraftingSlotsUI, _playerAnimator, _playersfirstSlotTransform, leanTweenTime, true );
                _playerCraftingUIHandler.ResetAllSlots();
            }
            if (_opponentCraftingUIHandler == null)
            {
                _opponentCraftingUIHandler = new CraftingUIHandler(_opponentCraftingSlotsUI, _opponentAnimator, _opponentfirstSlotTransform, leanTweenTime, false);
                _opponentCraftingUIHandler.ResetAllSlots();
            }
        }
}
}