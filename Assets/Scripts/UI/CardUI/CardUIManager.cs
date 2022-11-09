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
    public class CardUIManager : MonoSingleton<CardUIManager>, ISequenceOperation<IBattleUIManager>
    {
        #region Field
        private IPlayersManager _players;
        [SerializeField] private CardUIPool _cardPool;
        [SerializeField] private BattleCardUI enemyBattleCardUI;
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
        public HandUI HandUI { get => _handUI; }

        internal void UpdateHand(bool isPlayer)
        {
            GetCardsUI(_players.GetCharacter(isPlayer).DeckHandler.GetCardsFromDeck( DeckEnum.Hand));
        }

        internal void PlayEnemyCard(BattleCardData battleCard)
        {
            AssignDataToCardUI(enemyBattleCardUI, battleCard);
                ActivateEnemyCardUI(true);

        }

        public void ActivateEnemyCardUI(bool state)
            => enemyBattleCardUI.gameObject.SetActive(state);


        #endregion

        #region Monobehaviour Callbacks 
  

        #endregion

        #region Private Methods



        public void AssignDataToCardUI(BattleCardUI battleCard, BattleCardData battleCardData)
        {
            battleCard.AssingVisual(battleCardData);
        }

        #endregion

        #region Public Methods

        public BattleCardUI[] GetCardsUI(params BattleCardData[] cardData)
        {
            if (cardData == null)
            {
                throw new Exception(name + " BattleCardData is null");
            }

            List<BattleCardUI> tempCardUI = new List<BattleCardUI>();

            for (int i = 0; i < cardData.Length; i++)
            {
                if (cardData[i] == null)
                {
                    Debug.LogError(name + " BattleCardData in index " + i + " in null");
                }

                BattleCardUI cache = _cardPool.Pull();

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

        public IReadOnlyList<BattleCardUI>  GetCardUiFromHand() => _handUI.GetCardUIFromHand();
        


        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager battleUIManager)
        {
            var data = battleUIManager.BattleDataManager;
            _players = data.PlayersManager;
            _cardPool.Init();
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