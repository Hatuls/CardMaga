using System.Collections.Generic;
using CardMaga.Meta.AccountMetaData;
using UnityEngine;

namespace CardMaga.MetaData.Collection
{
    public class MetaCardDataHelper : MonoBehaviour
    {
        [SerializeField] private AccountDataAccess _accountDataAccess;
        
        private Dictionary<int, int> _sortCardDataIds;

        public Dictionary<int, int> SortCardDataIds => _sortCardDataIds;
        
        private void InitDictionary()
        {
            List<MetaCardData> cardDatas = _accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0].Cards;

            _sortCardDataIds = new Dictionary<int, int>();

            for (int i = 0; i < cardDatas.Count; i++)
            {
                int cache = cardDatas[i].CardInstance.ID;
            
                if (_sortCardDataIds.TryGetValue(cache, out int value))
                {
                    int temp = value++;
                    _sortCardDataIds[cache] = temp;
                }
                else
                {
                    _sortCardDataIds.Add(cache,1);
                }
            }
        }
    }
}