using Battles.Deck;
using Battles.UI.CardUIAttributes;
using Cards;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Battles.UI
{
    public class CardUIManager : MonoSingleton<CardUIManager>
    {
        #region Field

       // [SerializeField] private DeckManager _deckManager;
        //[SerializeField] private RectTransform _middleHandPos;
        [SerializeField] private CardUIPool _cardPool;
        
        private List<CardUI> _handCards;
        private HandUI _handUI;
        
        [SerializeField]
        Art.ArtSO _artSO;
        #endregion

        #region Events
        [SerializeField]
        UnityEvent OnPlayerRemoveHand;
        [SerializeField]
        UnityEvent OnDrawCard;
        #endregion

        #region Properties
        
        [SerializeField]
        private CardUI _enemyCardUI;
        internal void UpdateHand()
        {
            GetCardsUI(DeckManager.Instance.GetCardsFromDeck(true, DeckEnum.Hand));
        }

        internal void PlayEnemyCard(Card card)
        {
            if (_enemyCardUI.gameObject.activeSelf == false)
                ActivateEnemyCardUI(true);
            AssignDataToCardUI(_enemyCardUI, card);

        }

        public void ActivateEnemyCardUI(bool state)
            => _enemyCardUI.gameObject.SetActive(state);

        #endregion

 
        #region Monobehaviour Callbacks 
        public override void Awake()
        {
            base.Awake();
            SceneHandler.OnBeforeSceneShown += Init;
        }
        public void OnDestroy()
        {
            SceneHandler.OnBeforeSceneShown -= Init;
        }

        #endregion

        #region Private Methods

        // internal void CraftCardUI(Card addedCard, DeckEnum toDeck)
        // {
        //     if (toDeck == DeckEnum.Hand)
        //         AssignDataToCardUI(_handUI.CurrentlyHolding, addedCard);
        //
        // }

        public void AssignDataToCardUI(CardUI card, Cards.Card cardData)
        {
            card.GFX.SetCardReference(cardData);
        }
        
        #endregion


        #region Public Methods

        public CardUI[] GetCardsUI(params Card[] cardData)
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
                
                AssignDataToCardUI(cache,cardData[i]);
                
                tempCardUI.Add(cache);
            }
            
            // for (int i = 0; i < cardData.Length; i++)
            // {
            //     var card = _handUI.GetHandCardUIFromIndex(i);
            //
            //     if (cardData[i] == null)
            //     {
            //         string cardsDrawn = "";
            //         for (int j = 0; j < cardData.Length; j++)
            //         {
            //             if (cardData[j] != null)
            //                 cardsDrawn += cardData[j].ToString() + " " + cardData[j].CardSO.CardName + "\n";
            //             else
            //                 cardsDrawn += "Card at index " + i + " Is null!\n";
            //         }
            //         Debug.LogError($"Drawn Card is null!!\n {cardsDrawn} ,\n");
            //     }
            // }

            return tempCardUI.ToArray();

        }
        
        
        public void RemoveHands()
        {
            _handUI.DiscardHand();
            OnPlayerRemoveHand?.Invoke();
        }
        
        public override void Init(ITokenReciever token)
        {
            using (token.GetToken())
            {
               //_handUI = new HandUI(_middleHandPos.rect.position);
            }
        }

        public void ExecuteCardUI(CardUI card)
        {
            if (card == null)
                return;
        }

        #endregion
    }
}