using Battle;
using System;

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
        KeywordTypeEnum Keyword { get; }
        void UnProcessOnTarget(bool isFromPlayer, KeywordData keywordData, IPlayersManager playersManager);
        void ProcessOnTarget(bool isFromPlayer, KeywordData keywordData, IPlayersManager playersManager);
        
    }

    
}