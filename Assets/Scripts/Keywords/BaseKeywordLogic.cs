using Battle;


namespace Keywords
{

    public abstract class BaseKeywordLogic : IKeyword
    {
        public abstract KeywordTypeEnum Keyword { get; }
        public abstract void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager);
        public abstract void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager);
    }


    public interface IKeyword
    {
        void UnProcessOnTarget(bool isFromPlayer, KeywordData keywordData, IPlayersManager playersManager);
        void ProcessOnTarget(bool isFromPlayer, KeywordData keywordData, IPlayersManager playersManager);
        KeywordTypeEnum Keyword { get; }
    }

    
}