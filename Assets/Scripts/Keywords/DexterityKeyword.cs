using CardMaga.Battle;

namespace CardMaga.Keywords
{
    public class DexterityKeyword : BaseKeywordLogic
    {
        public DexterityKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }


        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + target.ToString() + " recieved " + KeywordSO.GetKeywordType.ToString() + " with Amount " + amount);


            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
                InvokeKeywordVisualEffect(currentPlayer);
            }
            if (target == TargetEnum.All || target == TargetEnum.Opponent){
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
                InvokeKeywordVisualEffect(!currentPlayer);
            }
            KeywordSO.SoundEventSO.PlaySound();
        }



        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {

            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);
        }
    }
}