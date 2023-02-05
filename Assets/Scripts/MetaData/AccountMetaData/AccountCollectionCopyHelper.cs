using CardMaga.MetaData.Collection;

namespace CardMaga.MetaData.AccoutData
{
    public class AccountCollectionCopyHelper
    {
        private MetaDeckData _metaDeckData;
        private CardsCollectionDataHandler _cardsCollectionDataHandler;
        private ComboCollectionDataHandler _comboCollectionDataHandler;

        public MetaDeckData MetaDeckData => _metaDeckData;

        public CardsCollectionDataHandler CardsCollectionDataHandler => _cardsCollectionDataHandler;

        public ComboCollectionDataHandler ComboCollectionDataHandler => _comboCollectionDataHandler;

        public void SetData(MetaDeckData metaDeckData, CardsCollectionDataHandler cardsCollectionDataHandler,
            ComboCollectionDataHandler comboCollectionDataHandler)
        {
            _metaDeckData = metaDeckData;
            _cardsCollectionDataHandler = cardsCollectionDataHandler;
            _comboCollectionDataHandler = comboCollectionDataHandler;
        }
        
        public CardsCollectionDataHandler GetAllUnAssingeCard()
        {
            CardsCollectionDataHandler output = new CardsCollectionDataHandler();
            
            bool Condition(MetaCardInstanceInfo metaCardInstanceInfo)
            {
                return !metaCardInstanceInfo.InDeck; 
            }

            foreach (var metaCollectionCardData in _cardsCollectionDataHandler.CollectionCardDatas.Values)
            {
                if (metaCollectionCardData.TryGetMetaCardInstanceInfo(Condition, out MetaCardInstanceInfo[] metaCardInstanceInfos))
                {
                    output.AddCardInstance(metaCardInstanceInfos);
                }
            }

            return output;
        }
    }
}