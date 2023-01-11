using System;
using System.Collections.Generic;
using Account.GeneralData;
using CardMaga.MetaData.Collection;
using UnityEngine;

namespace CardMaga.MetaData.Dismantle
{
    public class DismantleHandler
    {
        public event Action<CardInstance> OnAddCardToDismantleList; 
        public event Action<CardInstance> OnRemoveCardFromDismantleList; 

        private List<CardInstance> _dismantleCards;

        private CardsCollectionDataHandler _originalCardCollection;

        public DismantleHandler(CardsCollectionDataHandler originalCardCollection)
        {
            _dismantleCards = new List<CardInstance>();
            _originalCardCollection = originalCardCollection;
        }

        public void AddCardToDismantleList(CardInstance cardInstance)
        {
            _dismantleCards.Add(cardInstance);
            OnAddCardToDismantleList?.Invoke(cardInstance);
        }

        public CardInstance RemoveCardFromDismantleList(CardCore cardCore)
        {
            if (FindCardInstanceInDismantelList(cardCore.CoreID,out CardInstance cardInstance))
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

        public void ResetDismantleList()
        {
            //need to return card to collection
            _dismantleCards.Clear();
        }

        public List<CardInstance> ConfirmDismantleList()//plaster 10.1.23
        {
            foreach (var dismantleCard in _dismantleCards)
            {
                _originalCardCollection.TryRemoveCardInstance(dismantleCard.InstanceID,true);
            }

            var cache = _dismantleCards;
            //_dismantleCards.Clear();
            return cache;
        }
    }
}