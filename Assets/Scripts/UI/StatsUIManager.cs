using CardMaga.UI.Bars;
using CardMaga.UI.Visuals;
using Characters.Stats;
using UnityEngine;
namespace Battle.UI
{
    public class StatsUIManager : MonoBehaviour
    {
        [Header("Health Bars:")]
        [SerializeField] private HealthBarUI _leftHealthBarUI;
        [SerializeField] private HealthBarUI _rightHealthBarUI;

        [Header("Shield Bars:")]
        [SerializeField]private TopPartArmorUI _leftShieldUI;
        [SerializeField]private TopPartArmorUI _rightShieldUI;

        private void Awake()
        {
            CharacterStatsHandler.OnStatAssigned += InitStatUI;

        }

        private void InitStatUI(bool isPlayer, CharacterStatsHandler character)
        {
            HealthBarUI healthBar = HealthBar(isPlayer);
            BaseStat health = character.GetStat(Keywords.KeywordTypeEnum.Heal);
            BaseStat maxHealth = character.GetStat(Keywords.KeywordTypeEnum.MaxHealth);
            healthBar.InitHealthBar(health.Amount, maxHealth.Amount);
            health.OnValueChanged += healthBar.ChangeHealth;
            maxHealth.OnValueChanged += healthBar.ChangeMaxHealth;


            BaseStat armour = character.GetStat(Keywords.KeywordTypeEnum.Shield);
            armour.OnValueChanged += Armour(isPlayer).SetArmor;
        }
        private TopPartArmorUI Armour(bool isPlayer) => isPlayer ? _leftShieldUI : _rightShieldUI;
        private HealthBarUI HealthBar(bool isPlayer) => isPlayer ? _leftHealthBarUI : _rightHealthBarUI;

        private void OnDestroy()
        {
            UnSubscribe(true);
            UnSubscribe(false);

            CharacterStatsHandler.OnStatAssigned -= InitStatUI;

            void UnSubscribe(bool isPlayer)
            {
                var stats = BattleManager.Instance.PlayersManager.GetCharacter(isPlayer).StatsHandler;
                if (stats == null)
                    return;
                HealthBarUI healthBar = HealthBar(isPlayer);
                BaseStat health = stats.GetStat(Keywords.KeywordTypeEnum.Heal);
                BaseStat maxHealth = stats.GetStat(Keywords.KeywordTypeEnum.MaxHealth);
                BaseStat armour = stats.GetStat(Keywords.KeywordTypeEnum.Shield);
                health.OnValueChanged -= healthBar.ChangeHealth;
                maxHealth.OnValueChanged -= healthBar.ChangeMaxHealth;
                armour.OnValueChanged -= Armour(isPlayer).SetArmor;
            }
        }
    }
}
