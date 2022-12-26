using System;
using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;

namespace CardMaga.MetaData.Dismantle
{
    public class DismantleHandler
    {
        public event Action<List<MetaCardData>> OnConfirmDismantleCard;
        public event Action<MetaCardData> OnAddCardToDismantleList; 
        public event Action<MetaCardData> OnRemoveCardFromDismantleList; 

        private List<MetaCardData> _dismantleCards;

        public DismantleHandler()
        {
            _dismantleCards = new List<MetaCardData>();
        }

        public void AddCardToDismantleList(MetaCardData metaCardData)
        {
            _dismantleCards.Add(metaCardData);
            OnAddCardToDismantleList?.Invoke(metaCardData);
        }

        public void RemoveCardFromDismantleList(MetaCardData metaCardData)
        {
            if (!_dismantleCards.Contains(metaCardData))
                return;

            _dismantleCards.Remove(metaCardData);
            OnRemoveCardFromDismantleList?.Invoke(metaCardData);
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