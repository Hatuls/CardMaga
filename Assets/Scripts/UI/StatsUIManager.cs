using Battle;
using CardMaga.Battle.Visual;
using CardMaga.SequenceOperation;
using CardMaga.UI.Bars;
using CardMaga.UI.Visuals;
using ReiTools.TokenMachine;
using UnityEngine;
namespace CardMaga.Battle.UI
{
    public class StatsUIManager : MonoBehaviour, ISequenceOperation<IBattleManager>
    {
        [Header("Health Bars:")]
        [SerializeField] private HealthBarUI _leftHealthBarUI;
        [SerializeField] private HealthBarUI _rightHealthBarUI;

        [Header("Shield Bars:")]
        [SerializeField] private TopPartArmorUI _leftShieldUI;
        [SerializeField] private TopPartArmorUI _rightShieldUI;

        public int Priority => 0;

        private TopPartArmorUI Armour(bool isLeft) => isLeft ? _leftShieldUI : _rightShieldUI;
        private HealthBarUI HealthBar(bool isLeft) => isLeft ? _leftHealthBarUI : _rightHealthBarUI;

        private void InitStatUI(bool isLeft, VisualStatHandler character)
        {
            HealthBarUI healthBar = HealthBar(isLeft);
            VisualStat health = character.VisualStatsDictionary[Keywords.KeywordTypeEnum.Heal];
            VisualStat maxHealth = character.VisualStatsDictionary[Keywords.KeywordTypeEnum.MaxHealth];
            healthBar.InitHealthBar(health.Amount, maxHealth.Amount);
            health.OnValueChanged += healthBar.ChangeHealth;
            maxHealth.OnValueChanged += healthBar.ChangeMaxHealth;


            VisualStat armour = character.VisualStatsDictionary[Keywords.KeywordTypeEnum.Shield];
            armour.OnValueChanged += Armour(isLeft).SetArmor;
        }
 

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            bool isLeft = true;
            IPlayersManager playerManager = data.PlayersManager;
            InitStatUI(isLeft, playerManager.GetCharacter(isLeft).VisualCharacter.VisualStats);
            InitStatUI(!isLeft, playerManager.GetCharacter(!isLeft).VisualCharacter.VisualStats);

            data.OnBattleManagerDestroyed += BeforeDestroy;
        }

        private void BeforeDestroy(IBattleManager obj)
        {
            IPlayersManager playerManager = obj.PlayersManager;
            bool isLeftPlayer = true;
            UnSubscribe(isLeftPlayer);
            UnSubscribe(!isLeftPlayer);
            void UnSubscribe(bool isLeft)
            {
                var stats = playerManager.GetCharacter(isLeft).VisualCharacter.VisualStats;
                if (stats == null)
                    return;
                HealthBarUI healthBar = HealthBar(isLeft);
                VisualStat health = stats.VisualStatsDictionary[Keywords.KeywordTypeEnum.Heal];
                VisualStat maxHealth = stats.VisualStatsDictionary[Keywords.KeywordTypeEnum.MaxHealth];
                VisualStat armour = stats.VisualStatsDictionary[Keywords.KeywordTypeEnum.Shield];
                health.OnValueChanged -= healthBar.ChangeHealth;
                maxHealth.OnValueChanged -= healthBar.ChangeMaxHealth;
                armour.OnValueChanged -= Armour(isLeft).SetArmor;
            }
        }
    }
}
