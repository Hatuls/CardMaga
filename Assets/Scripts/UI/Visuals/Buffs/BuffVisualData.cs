using CardMaga.Keywords;

namespace CardMaga.UI.Visuals
{
    public class BuffVisualData
    {
        KeywordType _keywordType;
        int _buffCurrentAmount;

        public KeywordType KeywordType { get => _keywordType;set => _keywordType = value; }
        public int BuffCurrentAmount { get => _buffCurrentAmount;set => _buffCurrentAmount = value; }
    }
}