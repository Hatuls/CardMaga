
using CardMaga.Card;
using CardMaga.UI.Card;


using Battle.Deck;

using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CardMaga.UI;

namespace CardMaga.Battle.UI
{
    public class CardUIManager : MonoSingleton<CardUIManager>
    {
        #region Field

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


        internal void UpdateHand(bool isPlayer)
        {
            GetCardsUI(DeckManager.Instance.GetCardsFromDeck(isPlayer, DeckEnum.Hand));
        }

        internal void PlayEnemyCard(CardData card)
        {
            if (_enemyCardUI.gameObject.activeSelf == false)
                ActivateEnemyCardUI(true);
            AssignDataToCardUI(_enemyCardUI, card);

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

   

        public void ExecuteCardUI(CardUI card)
        {
            if (card == null)
                return;
        }

        public override void Init(ITokenReciever token)
        {
            _cardPool.Init(token);
        }

        #endregion
    }

}