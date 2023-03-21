using CardMaga.Battle;
using CardMaga.Keywords;
using Characters.Stats;

namespace Keywords
{
    public class AttackKeywordLogic : BaseKeywordLogic
    {
        public AttackKeywordLogic(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override int Priority => 0;

        //public void ApplyDamage(CharacterStatsHandler reciever, int amount)
        //=> reciever.GetStat(KeywordType.Heal).Reduce(amount);

        public override void ProcessOnTarget(bool currentPlayer, TargetEnum targetEnum, int amount)
        {
            CharacterStatsHandler reciver;
            CharacterStatsHandler applier;
            if (targetEnum == TargetEnum.MySelf || targetEnum == TargetEnum.All)
            {
                reciver = _playersManager.GetCharacter(currentPlayer).StatsHandler;
                applier = _playersManager.GetCharacter(!currentPlayer).StatsHandler;
                int finalDamage = CalculateDamage(amount, reciver, applier);
                ApplyDamageToShield(reciver, finalDamage);
            }

            if (targetEnum == TargetEnum.Opponent || targetEnum == TargetEnum.All)
            {

                reciver = _playersManager.GetCharacter(!currentPlayer).StatsHandler;
                applier = _playersManager.GetCharacter(currentPlayer).StatsHandler;
                int finalDamage = CalculateDamage(amount, reciver, applier);

                ApplyDamageToShield(reciver, finalDamage);

            }
            KeywordSO.SoundEventSO.PlaySound();
        }
        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {

                // damage + opponent strength + my Vulnerable -  opponent weakend  
                // damage + applier strenght + reciever Vulnerable - applier weakend

                var reciver = _playersManager.GetCharacter(currentPlayer).StatsHandler;
                var applier = _playersManager.GetCharacter(!currentPlayer).StatsHandler;
                int finalDamage = CalculateDamage(amount, reciver, applier);


                ApplyDamageToShield(reciver, -finalDamage);
            }


            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {


                var reciver = _playersManager.GetCharacter(!currentPlayer).StatsHandler;
                var applier = _playersManager.GetCharacter(currentPlayer).StatsHandler;
                int finalDamage = CalculateDamage(amount, reciver, applier);
                ApplyDamageToShield(reciver, -finalDamage);
            }
        }


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
    }
}