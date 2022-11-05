using CardMaga.Battle;
using CardMaga.Battle.UI;
using CardMaga.Battle.Visual;
using CardMaga.Keywords;
using Characters.Stats;

namespace CardMaga.Rules
{
    public class CharactersDiedListener : BaseEndGameRule
    {
        private BaseStat _leftPlayerHeal;
        private BaseStat _rightPlayerHeal;
        private VisualStat _leftVisualStat;
        private VisualStat _rightVisualStat;

        public CharactersDiedListener(float delayToEndGame) : base(delayToEndGame)
        {
        }
        public override void InitRuleListener(IBattleManager battleManager, BaseRuleLogic<bool>[] ruleLogics)
        {
            base.InitRuleListener(battleManager, ruleLogics);
            //Data
            _leftPlayerHeal = battleManager.PlayersManager.GetCharacter(true).StatsHandler.GetStat(KeywordType.Heal);
            _rightPlayerHeal = battleManager.PlayersManager.GetCharacter(false).StatsHandler.GetStat(KeywordType.Heal);

            //Visual
            VisualCharactersManager visualCharactersManager = battleManager.BattleUIManager.VisualCharactersManager;
            _leftVisualStat = visualCharactersManager.GetVisualCharacter(true).VisualStats.GetStat(KeywordType.Heal);
            _rightVisualStat = visualCharactersManager.GetVisualCharacter(false).VisualStats.GetStat(KeywordType.Heal);


            _leftVisualStat.OnValueChanged += CheckLeftPlayerCondition;
            _rightVisualStat.OnValueChanged += CheckRightPlayerCondition;
        }

        private void CheckLeftPlayerCondition(int playerHp)
        {
            if (playerHp <= 0 && _leftPlayerHeal.IsEmpty)
            {
                Active(false);
            }
        }

        private void CheckRightPlayerCondition(int playerHp)
        {
            if (playerHp <= 0 && _rightPlayerHeal.IsEmpty)
            {
                Active(true);
            }
        }

        public override void Dispose()
        {

            _leftVisualStat.OnValueChanged  -= CheckLeftPlayerCondition;
            _rightVisualStat.OnValueChanged -= CheckRightPlayerCondition;
        }

      
    }
}