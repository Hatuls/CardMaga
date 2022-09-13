using Battle;
using Keywords;
namespace Characters.Stats
{
    public class HealthRegenerationStat : BaseStat
    {
        public HealthRegenerationStat(bool isPlayer,  int amount) : base(isPlayer,  amount)
        {
        }

        public override void Reduce(int amount)
        {
            if(Amount - amount >=0)
            base.Reduce(amount);
        }
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Regeneration;
    }


    public class HealthRegenerationKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Regeneration;

    
        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            if (data == null)
                throw new System.Exception("HealthRegen data is null!!");

            var target = data.GetTarget;


            if (target == TargetEnum.MySelf || target == TargetEnum.All)
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);
            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);
            data.KeywordSO.SoundEventSO.PlaySound();
        }
    }
}