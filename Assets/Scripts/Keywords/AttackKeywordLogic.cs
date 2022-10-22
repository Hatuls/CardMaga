using Battle;
using Characters.Stats;

namespace Keywords
{
    public class AttackKeywordLogic : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Attack;

        private void ApplyDamage(CharacterStatsHandler reciever, int amount)
        =>  reciever.GetStats(KeywordTypeEnum.Shield).Reduce(amount);
        private int CalculateDamage(KeywordData data, CharacterStatsHandler reciver, CharacterStatsHandler applier)
        {
            int finalDamage;
            int recieverVulnerable = reciver.GetStats(KeywordTypeEnum.Vulnerable).Amount;
            int applierStrength = applier.GetStats(KeywordTypeEnum.Strength).Amount;
            int applierWeakend = applier.GetStats(KeywordTypeEnum.Weak).Amount;


            finalDamage = data.GetAmountToApply + applierStrength + recieverVulnerable - applierWeakend;
            if (finalDamage < 0)
                finalDamage = 0;
            return finalDamage;
        }
        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {

            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {

                // damage + opponent strength + my Vulnerable -  opponent weakend  
                // damage + applier strenght + reciever Vulnerable - applier weakend

                CharacterStatsHandler reciver = playersManager.GetCharacter(currentPlayer).StatsHandler;
                CharacterStatsHandler applier = playersManager.GetCharacter(!currentPlayer).StatsHandler;
                int finalDamage = CalculateDamage(data, reciver, applier);
                ApplyDamage(reciver, finalDamage);
            }


            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {

                var reciver = playersManager.GetCharacter(!currentPlayer).StatsHandler;
                var applier = playersManager.GetCharacter(currentPlayer).StatsHandler;
                int finalDamage = CalculateDamage(data, reciver, applier);

                ApplyDamage(reciver, finalDamage);

            }
            data.KeywordSO.SoundEventSO.PlaySound();

         
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {

            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {

                // damage + opponent strength + my Vulnerable -  opponent weakend  
                // damage + applier strenght + reciever Vulnerable - applier weakend

                var reciver = playersManager.GetCharacter(currentPlayer).StatsHandler;
                var applier = playersManager.GetCharacter(!currentPlayer).StatsHandler;
                int finalDamage = CalculateDamage(data, reciver, applier);


                ApplyDamage(reciver, -finalDamage);
            }


            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {

                 
                var reciver = playersManager.GetCharacter(!currentPlayer).StatsHandler;
                var applier = playersManager.GetCharacter(currentPlayer).StatsHandler;
                int finalDamage = CalculateDamage(data, reciver, applier);
                ApplyDamage(reciver, -finalDamage);
            }
        }
    }
}