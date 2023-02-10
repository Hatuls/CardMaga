using Battle;
using CardMaga.Battle.Visual;
using CardMaga.SequenceOperation;
using CardMaga.UI.Bars;
using CardMaga.UI.Visuals;
using ReiTools.TokenMachine;
using UnityEngine;
namespace CardMaga.Battle.UI
{
    public class StatsUIManager : MonoBehaviour, ISequenceOperation<IBattleUIManager>
    {
        [Header("Health Bars:")]
        [SerializeField] private HealthBarUI _leftHealthBarUI;
        [SerializeField] private HealthBarUI _rightHealthBarUI;

        [Header("Shield Bars:")]
        [SerializeField] private TopPartArmorUI _leftShieldUI;
        [SerializeField] private TopPartArmorUI _rightShieldUI;
       private VisualCharactersManager _visualCharactersManager;
        public int Priority => 0;

        private TopPartArmorUI Armour(bool isLeft) => isLeft ? _leftShieldUI : _rightShieldUI;
        private HealthBarUI HealthBar(bool isLeft) => isLeft ? _leftHealthBarUI : _rightHealthBarUI;

        private void InitStatUI(bool isLeft, VisualStatHandler character)
        {
            HealthBarUI healthBar = HealthBar(isLeft);
            VisualStat health = character.VisualStatsDictionary[Keywords.KeywordType.Heal];
            VisualStat maxHealth = character.VisualStatsDictionary[Keywords.KeywordType.MaxHealth];
            healthBar.InitHealthBar(health.Amount, maxHealth.Amount);
            health.OnValueChanged += healthBar.ChangeHealth;
            maxHealth.OnValueChanged += healthBar.ChangeMaxHealth;


            VisualStat armour = character.VisualStatsDictionary[Keywords.KeywordType.Shield];
            armour.OnValueChanged += Armour(isLeft).SetArmor;
        }


        public void ExecuteTask(ITokenReceiver tokenMachine, IBattleUIManager data)
        {
            bool isLeft = true;
            _visualCharactersManager = data.VisualCharactersManager;
            InitStatUI(isLeft,  _visualCharactersManager.GetVisualCharacter(isLeft).VisualStats);
            InitStatUI(!isLeft, _visualCharactersManager.GetVisualCharacter(!isLeft).VisualStats);

            data.BattleDataManager.OnBattleManagerDestroyed += BeforeDestroy;
        }

        private void BeforeDestroy(IBattleManager obj)
        {
            IPlayersManager playerManager = obj.PlayersManager;
            bool isLeftPlayer = true;
            UnSubscribe(isLeftPlayer);
            UnSubscribe(!isLeftPlayer);
            void UnSubscribe(bool isLeft)
            {
                var stats = _visualCharactersManager.GetVisualCharacter(isLeft).VisualStats;
                if (stats == null)
                    return;
                HealthBarUI healthBar = HealthBar(isLeft);
                VisualStat health = stats.VisualStatsDictionary[Keywords.KeywordType.Heal];
                VisualStat maxHealth = stats.VisualStatsDictionary[Keywords.KeywordType.MaxHealth];
                VisualStat armour = stats.VisualStatsDictionary[Keywords.KeywordType.Shield];
                health.OnValueChanged -= healthBar.ChangeHealth;
                maxHealth.OnValueChanged -= healthBar.ChangeMaxHealth;
                armour.OnValueChanged -= Armour(isLeft).SetArmor;
            }
        }
    }
}
