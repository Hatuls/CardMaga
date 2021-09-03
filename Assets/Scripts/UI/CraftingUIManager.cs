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

        [SerializeField] CraftingSlotUI[] _opponentCraftingSlotsUI;
        [SerializeField] RectTransform _opponentfirstSlotTransform;

        [SerializeField]  GameObject _buttonGlow;

        [SerializeField] TextMeshProUGUI _buttonText;

        [SerializeField] float leanTweenTime;

        public CraftingUIHandler GetCharacterUIHandler(bool players)
        {
            InitCraftingUIHandler();
            return players ? _playerCraftingUIHandler : _opponentCraftingUIHandler;
        }


        private void InitCraftingUIHandler()
        {
            if(_playerCraftingUIHandler == null)
            {
                _playerCraftingUIHandler = new CraftingUIHandler(_playerCraftingSlotsUI, _playersfirstSlotTransform, leanTweenTime, _buttonGlow, _buttonText);
            }

            if (_opponentCraftingUIHandler == null)
            {
                _opponentCraftingUIHandler = new CraftingUIHandler(_opponentCraftingSlotsUI, _opponentfirstSlotTransform, leanTweenTime);
            }
        }



    public override void Init()
    {
        throw new System.NotImplementedException();
    }
}
}