using Characters.Stats;

namespace Keywords
{
    public class AttackKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Attack;

     
        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                var reciver = CharacterStatsManager.GetCharacterStatsHandler(currentPlayer);
                var applierStrength = CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer).GetStats(KeywordTypeEnum.Strength).Amount;

                reciver.RecieveDamage(applierStrength + data.GetAmountToApply);
            }
         

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {
                var reciver = CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer);
                var applierStrength = CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(KeywordTypeEnum.Strength).Amount;

                reciver.RecieveDamage(applierStrength + data.GetAmountToApply);

            }
  
        }
    }
}