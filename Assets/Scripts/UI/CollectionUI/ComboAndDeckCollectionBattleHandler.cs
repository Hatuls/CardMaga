
using Battle.Deck;
using CardMaga.Battle.Players;
using CardMaga.Battle.UI;
using CardMaga.Input;
using CardMaga.SequenceOperation;
using CardMaga.UI.Card;
using CardMaga.UI.Collections.Zoom;
using CardMaga.UI.Combos;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.UI.Collections
{
    public class ComboAndDeckCollectionBattleHandler : MonoBehaviour, ISequenceOperation<IBattleUIManager>
    {
        [SerializeField] private ComboAndDeckCollectionHandler _collection;
        [SerializeField] private RectTransform _cardZoomPositionRectTransform;
        private DeckHandler _deckHandler;
        private PlayerComboContainer _playerComboContainer;
        private BattleCollectionZoomHandler _battleCollectionZoomHandler;
        private bool _isFirstTimeOpeningCards = true;


        private InputBehaviour<BattleCardUI> _cardInputBehaviour;
        private InputBehaviour<BattleComboUI> _comboInputBehaviour;

        public int Priority => 0;
        public ComboAndDeckCollectionHandler ComboAndDeckCollectionHandler => _collection;
        public InputBehaviour<BattleCardUI> CardInputBehaviour 
        {
            get => _cardInputBehaviour;
            private set => _cardInputBehaviour = value; 
        }
        public InputBehaviour<BattleComboUI> ComboInputBehaviour 
        {
            get => _comboInputBehaviour; 
            private set => _comboInputBehaviour = value; 
        }


        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager data)
        {
            IPlayer player = data.BattleDataManager.PlayersManager.LeftCharacter;

            _deckHandler = player.DeckHandler;
            _playerComboContainer = player.Combos;
            CardInputBehaviour = new InputBehaviour<BattleCardUI>();
            ComboInputBehaviour = new InputBehaviour<BattleComboUI>();

            _battleCollectionZoomHandler = new BattleCollectionZoomHandler(this, _cardZoomPositionRectTransform);

            _collection.Init();
            _collection.AssignComboData(_playerComboContainer);
            SetPlayerDeck();


        }

        public void SetExhaustDeck()
        {
            _collection.AssignCardData(_deckHandler[DeckEnum.Exhaust]);
        }

        public void SetPlayerDeck()
        {
            _collection.AssignCardData(_deckHandler[DeckEnum.PlayerDeck]);
        }

        public void SetDiscardDeck()
        {
            _collection.AssignCardData(_deckHandler[DeckEnum.Discard]);
        }
        
        public void AllCards()
        {
            _collection.AssignCardData(_deckHandler);
        }
    

        public void FirstOpenCardPanelCheck()
        {
            if (_isFirstTimeOpeningCards)
            {
                _isFirstTimeOpeningCards = false;
                SetPlayerDeck();
            }
        }
    }
}