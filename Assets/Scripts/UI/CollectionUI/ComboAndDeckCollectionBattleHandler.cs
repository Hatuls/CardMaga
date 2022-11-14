
using Battle.Deck;
using CardMaga.Battle.Players;
using CardMaga.Battle.UI;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.UI.Collections
{
    public class ComboAndDeckCollectionBattleHandler : MonoBehaviour, ISequenceOperation<IBattleUIManager>
    {
        [SerializeField] private ComboAndDeckCollectionHandler _collection;

        private DeckHandler _deckHandler;
        private PlayerComboContainer _playerComboContainer;

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager data)
        {
            IPlayer player = data.BattleDataManager.PlayersManager.LeftCharacter;

            _deckHandler = player.DeckHandler;
            _playerComboContainer = player.Combos;
            _collection.Init();
            _collection.AssignComboData(_playerComboContainer);
            _collection.AssignCardData(_deckHandler);
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
        public int Priority
        {
            get => 0;
        }
    }
}