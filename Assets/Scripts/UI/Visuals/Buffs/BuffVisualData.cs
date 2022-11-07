using CardMaga.Keywords;
using Sirenix.OdinInspector;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class BuffVisualData
    {
        public KeywordType _keywordType;
        public int _buffCurrentAmount;

        public KeywordType KeywordType { get => _keywordType;set => _keywordType = value; }
        public int BuffCurrentAmount { get => _buffCurrentAmount;set => _buffCurrentAmount = value; }
    }
}