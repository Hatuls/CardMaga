using CardMaga.Battle;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using Characters.Stats;
using System;

namespace CardMaga.Keywords
{

    public abstract class BaseKeywordLogic : IKeyword, IKeywordTurnEffect
    {
        public event Action<BaseKeywordLogic> OnKeywordActivated;
        public event Action<BaseKeywordLogic> OnKeywordFinished;

        public readonly KeywordSO KeywordSO;
        protected readonly IPlayersManager _playersManager;
        protected BaseKeywordLogic(KeywordSO keywordSO, IPlayersManager playersManager)
        {
            _playersManager = playersManager;
            KeywordSO = keywordSO;
        }

      
        public virtual int Priority => 0;
        public KeywordType KeywordType => KeywordSO.GetKeywordType;

        public bool IsEmpty(CharacterStatsHandler characterStatsHandler) => characterStatsHandler.GetStat(KeywordType).HasValue();

        public abstract void ProcessOnTarget(bool currentPlayer, KeywordData data);
        public abstract void UnProcessOnTarget(bool currentPlayer, KeywordData data);



        public virtual void StartTurnEffect(IPlayer currentCharacterTurn, GameDataCommands gameDataCommands) { }
        public virtual void EndTurnEffect(IPlayer currentCharacterTurn, GameDataCommands gameDataCommands) { }


        protected void InvokeOnKeywordActivated() => OnKeywordActivated?.Invoke(this);
        protected void InvokeOnKeywordFinished() => OnKeywordFinished?.Invoke(this);


        public int CompareTo(BaseKeywordLogic other)
        {
            if (Priority < other.Priority)
                return -1;
            else if (Priority > other.Priority)
                return 1;
            else
                return 0;
        }
    }


    public interface IKeyword 
    {
        KeywordType KeywordType { get; }
        void ProcessOnTarget(bool isFromPlayer, KeywordData keywordData);
        void UnProcessOnTarget(bool isFromPlayer, KeywordData keywordData);
        bool IsEmpty(CharacterStatsHandler characterStatsHandler);

    }

    public interface IKeywordTurnEffect : IComparable<BaseKeywordLogic>
    {
         event Action<BaseKeywordLogic> OnKeywordActivated;
         event Action<BaseKeywordLogic> OnKeywordFinished;
        void StartTurnEffect(IPlayer currentCharacterTurn, GameDataCommands gameDataCommands);
        void EndTurnEffect(IPlayer currentCharacterTurn, GameDataCommands gameDataCommands);
    } 

    // Experimental will need to remake the kewords effect logic
    //public abstract class KeywordEffect
    //{
    //    public event Action<KeywordEffect> OnKeywordEffectAdded;
    //    public event Action<KeywordEffect> OnKeywordEffectFinished;

    //    public readonly KeywordData KeywordData;

    //    public KeywordEffect(KeywordData keywordData)
    //    {
    //        KeywordData = keywordData;
    //    }


    //    public virtual void InitEffect(bool currentPlayer, IPlayersManager playersManager, GameCommands gameCommands)
    //    {

    //    }

    //    public virtual void EffectFinished()
    //    {

    //    }

    //    public abstract void ActivateEffect();
    //    public abstract void StartTurnEffect(bool currentCharacterTurn);
    //    public abstract void EndTurnEffect(bool currentCharacterTurn);
    //}


}