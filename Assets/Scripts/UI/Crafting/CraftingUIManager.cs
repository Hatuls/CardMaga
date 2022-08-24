using Battle.Deck;
using Managers;
using ReiTools.TokenMachine;
using UnityEngine;

namespace Battle.UI
{
    public class CraftingUIManager : MonoBehaviour, ISequenceOperation<BattleManager>
    {
        [SerializeField] private CraftingSlotUI[] _playerCraftingSlotsUI;
        [SerializeField] private RectTransform _playersfirstSlotTransform;
        [SerializeField] private CraftingSlotUI _fadeOutCraftingSlots;

        [SerializeField] private CraftingSlotUI[] _opponentCraftingSlotsUI;
        [SerializeField] private RectTransform _opponentfirstSlotTransform;
        [SerializeField] private CraftingSlotUI _opponentfadeOutCraftingSlots;

        [SerializeField] private float moveLeanTweenTime;
        private CraftingUIHandler _opponentCraftingUIHandler;
        private CraftingUIHandler _playerCraftingUIHandler;



        const int ORDER = 4;
        public int Priority => ORDER;
       



        public CraftingUIHandler GetCharacterUIHandler(bool players)
        {
           // Init();
            return players ? _playerCraftingUIHandler : _opponentCraftingUIHandler;
        }


   

        #region Monobehaviour Callbacks
        public void Awake()
        {
            BattleManager.Register(this, OrderType.Before);
        }

        public void ExecuteTask(ITokenReciever tokenMachine,BattleManager battleManager)
        {
            using (tokenMachine.GetToken())
            {
                Init();
            }
        }

        private void Init()
        {
            if (_playerCraftingUIHandler == null)
            {
                _playerCraftingUIHandler = new CraftingUIHandler(_playerCraftingSlotsUI, _fadeOutCraftingSlots,
                    _playersfirstSlotTransform, moveLeanTweenTime, true);
                _playerCraftingUIHandler.ResetAllSlots();
            }

            if (_opponentCraftingUIHandler == null)
            {
                _opponentCraftingUIHandler = new CraftingUIHandler(_opponentCraftingSlotsUI,
                    _opponentfadeOutCraftingSlots, _opponentfirstSlotTransform, moveLeanTweenTime, false);
                _opponentCraftingUIHandler.ResetAllSlots();
            }
        }

        #endregion
    }
}