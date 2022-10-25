using Battle;
using CardMaga.SequenceOperation;
using CardMaga.UI;
using Keywords;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.Battle.UI
{

    public class BattleUiManager : MonoBehaviour, ISequenceOperation<IBattleManager>
    {
        #region Fields
        [SerializeField]
        private HandUI _handUI;

        [SerializeField]
        private CardUIManager _cardUIManager;

        [SerializeField]
        private EndTurnButton _endTurnButton;

        [SerializeField]
        private StatsUIManager _statsUIManager;

        [SerializeField]
        private ComboUIManager _comboUIManager;

        [SerializeField]
        private BuffIconManager _buffIconManager;

        [SerializeField]
        private CraftingSlotsUIManager_V4 _craftingSlotsUIManager;

        
        private VisualKeywordsHandler _visualKeywordsHandler;
        #endregion


        #region Events


        #endregion

        #region Properties

        public IEnumerable<ISequenceOperation<IBattleManager>> VisualInitializers
        {
            get
            {
                yield return _statsUIManager;
                yield return _visualKeywordsHandler;
                yield return _cardUIManager;
                yield return _handUI;
                yield return _comboUIManager;
                yield return _endTurnButton;
                yield return _buffIconManager;
                yield return _craftingSlotsUIManager;
            }
        }
        public int Priority => 0;

        public HandUI HandUI { get => _handUI; }
        public CardUIManager CardUIManager { get => _cardUIManager; }
        public EndTurnButton EndTurnButton { get => _endTurnButton; }
        public StatsUIManager StatsUIManager { get => _statsUIManager; }
        public ComboUIManager ComboUIManager { get => _comboUIManager; }
        public BuffIconManager BuffIconManager { get => _buffIconManager; }
        public CraftingSlotsUIManager_V4 CraftingSlotsUIManager { get => _craftingSlotsUIManager; }
        #endregion

        #region UnityCallBacks
        private void Awake()
        {
            _visualKeywordsHandler = new VisualKeywordsHandler();
            BattleManager.Register(this, OrderType.After);
        }
        #endregion
        #region Functions
        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            IDisposable token = tokenMachine.GetToken();

            foreach (var uiElement in VisualInitializers)
                uiElement.ExecuteTask(tokenMachine, data);

            token.Dispose();
        }
        #endregion



        #region Editor
#if UNITY_EDITOR
        [ContextMenu("Find Values And Assign")]
        private void AssignFields()
        {
            _handUI = FindObjectOfType<HandUI>();
            _comboUIManager = FindObjectOfType<ComboUIManager>();
            _cardUIManager = FindObjectOfType<CardUIManager>();
            _endTurnButton = FindObjectOfType<EndTurnButton>();
            _statsUIManager = FindObjectOfType<StatsUIManager>();
            _craftingSlotsUIManager = FindObjectOfType<CraftingSlotsUIManager_V4>();
            _buffIconManager.AssignBuffsFields();
        }
#endif
        #endregion
    }
}

