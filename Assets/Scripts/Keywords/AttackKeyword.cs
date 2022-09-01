using Battle;
using Characters.Stats;

namespace Keywords
{
    public class AttackKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Attack;

     
        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
         
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {

                // damage + opponent strength + my Vulnerable -  opponent weakend  
                // damage + applier strenght + reciever Vulnerable - applier weakend

                int finalDamage = 0;
                var reciver = CharacterStatsManager.GetCharacterStatsHandler(currentPlayer);
                var applier = CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer);

                var recieverVulnerable = reciver.GetStats(KeywordTypeEnum.Vulnerable).Amount;
                var applierStrength = applier.GetStats(KeywordTypeEnum.Strength).Amount;
                var applierWeakend = applier.GetStats(KeywordTypeEnum.Weak).Amount;


                finalDamage = data.GetAmountToApply + applierStrength + recieverVulnerable - applierWeakend;
                if (finalDamage < 0)
                    finalDamage = 0;
                reciver.RecieveDamage(finalDamage);
            }
         

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {

                int finalDamage = 0;
                var reciver = CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer);
                var applier = CharacterStatsManager.GetCharacterStatsHandler(currentPlayer);

                var recieverVulnerable = reciver.GetStats(KeywordTypeEnum.Vulnerable).Amount;
                var applierStrength = applier.GetStats(KeywordTypeEnum.Strength).Amount;
                var applierWeakend = applier.GetStats(KeywordTypeEnum.Weak).Amount;
                finalDamage = data.GetAmountToApply + applierStrength + recieverVulnerable - applierWeakend;
                if (finalDamage < 0)
                    finalDamage = 0;
                reciver.RecieveDamage(finalDamage);

                //var reciver = CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer);
                //var applierStrength = CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(KeywordTypeEnum.Strength).Amount;

                //reciver.RecieveDamage(applierStrength + data.GetAmountToApply);

            }
            data.KeywordSO.SoundEventSO.PlaySound();
        }
    }
}