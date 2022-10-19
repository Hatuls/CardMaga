using CardMaga.Card;
using CardMaga.UI.Card;
using CardMaga.SequenceOperation;
using Battle.Deck;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Battle;
using CardMaga.UI;

namespace CardMaga.Battle.UI
{
    public class CardUIManager : MonoSingleton<CardUIManager>, ISequenceOperation<IBattleManager>
    {
        #region Field
        private IPlayersManager _players;
        [SerializeField] private CardUIPool _cardPool;
        [SerializeField] private CardUI _enemyCardUI;
        [SerializeField] private HandUI _handUI;
        #endregion

        #region Events
        [SerializeField]
        UnityEvent OnPlayerRemoveHand;
        [SerializeField]
        UnityEvent OnDrawCard;

        #endregion

        #region Properties
        public int Priority => 0;


        internal void UpdateHand(bool isPlayer)
        {
            GetCardsUI(_players.GetCharacter(isPlayer).DeckHandler.GetCardsFromDeck( DeckEnum.Hand));
        }

        internal void PlayEnemyCard(CardData card)
        {
            AssignDataToCardUI(_enemyCardUI, card);
                ActivateEnemyCardUI(true);

        }

        public void ActivateEnemyCardUI(bool state)
            => _enemyCardUI.gameObject.SetActive(state);


        #endregion

        #region Monobehaviour Callbacks 
  

        #endregion

        #region Private Methods



        public void AssignDataToCardUI(CardUI card, CardData cardData)
        {
            card.AssingVisual(cardData);
        }

        #endregion

        #region Public Methods

        public CardUI[] GetCardsUI(params CardData[] cardData)
        {
            if (cardData == null)
            {
                throw new Exception(name + " CardData is null");
            }

            List<CardUI> tempCardUI = new List<CardUI>();

            for (int i = 0; i < cardData.Length; i++)
            {
                if (cardData[i] == null)
                {
                    Debug.LogError(name + " CardData in index " + i + " in null");
                }

                CardUI cache = _cardPool.Pull();

                AssignDataToCardUI(cache, cardData[i]);

                tempCardUI.Add(cache);
            }

            return tempCardUI.ToArray();
        }


        public void RemoveHands()
        {
            //_handUI.DiscardHand();
            OnPlayerRemoveHand?.Invoke();
        }

        public IReadOnlyList<CardUI>  GetCardUiFromHand()
        {
            return _handUI.GetCardUIFromHand();
        }


        public override void Awake()
        {
            base.Awake();
            BattleManager.Register(this, OrderType.After);
            _cardPool.Init();
        }
        
        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            _players = data.PlayersManager;
            data.CardExecutionManager.OnEnemyCardExecute += PlayEnemyCard;
            data.TurnHandler.GetCharacterTurn(false).EndTurnOperations.Register((x) => ActivateEnemyCardUI(false));
            data.OnBattleManagerDestroyed += BeforeBattleManagerDestroyed;
        }
        private void BeforeBattleManagerDestroyed(IBattleManager bm)
        { 
          bm.CardExecutionManager.OnEnemyCardExecute -= PlayEnemyCard;
        }
        #endregion
    }

}