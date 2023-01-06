using System;
using System.Collections.Generic;
using Account.GeneralData;
using CardMaga.MetaData.Collection;
using UnityEngine;

namespace CardMaga.MetaData.Dismantle
{
    public class DismantleHandler
    {
        public event Action<List<CardInstance>> OnConfirmDismantleCard;
        public event Action<CardInstance> OnAddCardToDismantleList; 
        public event Action<CardInstance> OnRemoveCardFromDismantleList; 

        private List<CardInstance> _dismantleCards;

        public DismantleHandler()
        {
            _dismantleCards = new List<CardInstance>();
        }

        public void AddCardToDismantleList(CardInstance cardInstance)
        {
            _dismantleCards.Add(cardInstance);
            OnAddCardToDismantleList?.Invoke(cardInstance);
        }

        public CardInstance RemoveCardFromDismantleList(CardCore cardCore)
        {
            if (FindCardInstanceInDismantelList(cardCore.CardID,out CardInstance cardInstance))
            {
                _dismantleCards.Remove(cardInstance);
                OnRemoveCardFromDismantleList?.Invoke(cardInstance);
                return cardInstance;
            }

            Debug.LogWarning("Did not find card Instance in discard collection");
            return null;
        }
        
        private bool FindCardInstanceInDismantelList(int coreId ,out CardInstance cardInstance)
        {
            foreach (var card in _dismantleCards)
            {
                if (card.CoreID == coreId)
                {
                    cardInstance = card;
                    return true;
                }
            }

            cardInstance = null;
            return false;
        }

        public void ResetDismantelList()
        {
            _dismantleCards.Clear();
        }

        public void ConfirmDismantleList()
        {
            OnConfirmDismantleCard?.Invoke(_dismantleCards);
        }
    }
}