using System.Collections.Generic;
using Account.GeneralData;

namespace CardMaga.Meta.AccountMetaData
{
    public class MetaCharactersHandler
    {
        #region Fields

        private Dictionary<int, MetaCharacterData> _characterDatas;

        private MetaCharacterData _mainMetaCharacterData;

        #endregion

        public MetaCharactersHandler(IReadOnlyList<Character> characters)
        {
            foreach (var character in characters)
            {
                MetaCharacterData data = new MetaCharacterData(character);
                _characterDatas.Add(character.Id,data);
            }
        }
    }
}