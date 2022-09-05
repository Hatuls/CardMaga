
using CardMaga.Card;
using CardMaga.UI.Card;


using Battle.Deck;

using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CardMaga.UI;
using Battle;
using Managers;

namespace CardMaga.Battle.UI
{
    public class CardUIManager : MonoSingleton<CardUIManager>, ISequenceOperation<BattleManager>
    {
        #region Field
        private IPlayersManager _players;
        [SerializeField] private CardUIPool _cardPool;
        [SerializeField] private CardUI _enemyCardUI;
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
            card.AssignCard(cardData);
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

   


        public override void Awake()
        {
            base.Awake();
            BattleManager.Register(this, OrderType.After);
            _cardPool.Init();
        }
        
        public void ExecuteTask(ITokenReciever tokenMachine, BattleManager data)
        {
            _players = data.PlayersManager;
            CardExecutionManager.OnEnemyCardExecute += PlayEnemyCard;
            data.TurnHandler.GetCharacterTurn(false).EndTurnOperations.Register((x) => ActivateEnemyCardUI(false));

        }
        private void OnDestroy()
        {
            CardExecutionManager.OnEnemyCardExecute -= PlayEnemyCard;
        }
        #endregion
    }

}