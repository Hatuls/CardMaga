using Battle.Deck;
using Managers;
using ReiTools.TokenMachine;
using UnityEngine;

namespace Battle.UI
{
    public class CraftingUIManager : MonoSingleton<CraftingUIManager>
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

    
        public CraftingUIHandler GetCharacterUIHandler(bool players)
        {
            Init(null);
            return players ? _playerCraftingUIHandler : _opponentCraftingUIHandler;
        }


        public override void Init(ITokenReciever token)
        {
            
            using (token?.GetToken())
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
        }

        #region Monobehaviour Callbacks
        public override void Awake()
        {
            base.Awake();
            const int order = 4;
            SceneStarter.Register(new OperationTask(Init, order));
        }

        #endregion
    }
}