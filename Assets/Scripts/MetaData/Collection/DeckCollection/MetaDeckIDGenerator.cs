using CardMaga.MetaData.AccoutData;

namespace CardMaga.MetaData.Collection.DeckCollection
{
    public class MetaDeckIDGenerator
    {
        private int _deckIdCount = 0;
        public int GetNewDeckID(MetaDeckData[] deckDatas)
        {
            foreach (var metaDeckData in deckDatas)
            {
                if (metaDeckData.DeckId == _deckIdCount)
                {
                    _deckIdCount++;
                    return GetNewDeckID(deckDatas);
                }
            }

            int cache = _deckIdCount;
            _deckIdCount = 0;
            return cache;
        }
    }
}