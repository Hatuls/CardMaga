using CardMaga.Battle;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.Keywords;
using Characters.Stats;

namespace Keywords
{
    public class AttackKeywordLogic : BaseKeywordLogic
    {
        public AttackKeywordLogic(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override int Priority =>0;

        public void ApplyDamage(CharacterStatsHandler reciever, int amount)
        => reciever.GetStat(KeywordType.Heal).Reduce(amount);
        
        public void ApplyDamageToShield(CharacterStatsHandler reciever, int amount)
        => reciever.GetStat(KeywordType.Shield).Reduce(amount);
        private int CalculateDamage(int damage, CharacterStatsHandler reciver, CharacterStatsHandler applier)
        {
            // damage + opponent strength + my Vulnerable -  opponent weakend  
            // damage + applier strenght + reciever Vulnerable - applier weakend

            int finalDamage;
            int recieverVulnerable = reciver.GetStat(KeywordType.Vulnerable).Amount;
            int applierStrength = applier.GetStat(KeywordType.Strength).Amount;
            int applierWeakend = applier.GetStat(KeywordType.Weak).Amount;


            finalDamage = damage + applierStrength + recieverVulnerable - applierWeakend;
            if (finalDamage < 0)
                finalDamage = 0;
            return finalDamage;
        }
        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                CharacterStatsHandler reciver = _playersManager.GetCharacter(currentPlayer).StatsHandler;
                CharacterStatsHandler applier = _playersManager.GetCharacter(!currentPlayer).StatsHandler;
                int finalDamage = CalculateDamage(data.GetAmountToApply, reciver, applier);
                ApplyDamageToShield(reciver, finalDamage);
            }


            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {

                CharacterStatsHandler reciver = _playersManager.GetCharacter(!currentPlayer).StatsHandler;
                CharacterStatsHandler applier = _playersManager.GetCharacter(currentPlayer).StatsHandler;
                int finalDamage = CalculateDamage(data.GetAmountToApply, reciver, applier);

                ApplyDamageToShield(reciver, finalDamage);

            }
            data.KeywordSO.SoundEventSO.PlaySound();


        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {

                // damage + opponent strength + my Vulnerable -  opponent weakend  
                // damage + applier strenght + reciever Vulnerable - applier weakend

                var reciver = _playersManager.GetCharacter(currentPlayer).StatsHandler;
                var applier = _playersManager.GetCharacter(!currentPlayer).StatsHandler;
                int finalDamage = CalculateDamage(data.GetAmountToApply, reciver, applier);


                ApplyDamageToShield(reciver, -finalDamage);
            }


            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {


                var reciver = _playersManager.GetCharacter(!currentPlayer).StatsHandler;
                var applier = _playersManager.GetCharacter(currentPlayer).StatsHandler;
                int finalDamage = CalculateDamage(data.GetAmountToApply, reciver, applier);
                ApplyDamageToShield(reciver, -finalDamage);
            }
        }


    }
}