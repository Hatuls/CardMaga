using System;
using System.Collections.Generic;
using Account.GeneralData;
using CardMaga.MetaData.Collection;
using UnityEngine;

namespace CardMaga.MetaData.Dismantle
{
    public class DismantleHandler
    {
        public event Action<MetaCardInstanceInfo> OnAddCardToDismantleList; 
        public event Action<MetaCardInstanceInfo> OnRemoveCardFromDismantleList; 

        private List<MetaCardInstanceInfo> _dismantleCards;

        private CardsCollectionDataHandler _originalCardCollection;
        public IReadOnlyList<MetaCardInstanceInfo> DismantleCards => _dismantleCards;
        public DismantleHandler(CardsCollectionDataHandler originalCardCollection)
        {
            _dismantleCards = new List<MetaCardInstanceInfo>();
            _originalCardCollection = originalCardCollection;
        }

        public void AddCardToDismantleList(MetaCardInstanceInfo cardInstance)
        {
            _dismantleCards.Add(cardInstance);
            OnAddCardToDismantleList?.Invoke(cardInstance);
        }

        public MetaCardInstanceInfo RemoveCardFromDismantleList(CardCore cardCore)
        {
            if (FindCardInstanceInDismantelList(cardCore.CoreID,out MetaCardInstanceInfo cardInstance))
            {
                _dismantleCards.Remove(cardInstance);
                OnRemoveCardFromDismantleList?.Invoke(cardInstance);
                return cardInstance;
            }

            Debug.LogWarning("Did not find card Instance in discard collection");
            return null;
        }
        
        private bool FindCardInstanceInDismantelList(int coreId ,out MetaCardInstanceInfo cardInstance)
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

        public List<MetaCardInstanceInfo> ConfirmDismantleList()//plaster 10.1.23
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