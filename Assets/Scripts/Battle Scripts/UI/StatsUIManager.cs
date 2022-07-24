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
            StatAbst health = character.GetStats(Keywords.KeywordTypeEnum.Heal);
            StatAbst maxHealth = character.GetStats(Keywords.KeywordTypeEnum.MaxHealth);
            healthBar.InitHealthBar(health.Amount, maxHealth.Amount);
            health.OnValueChanged += healthBar.ChangeHealth;
            maxHealth.OnValueChanged += healthBar.ChangeMaxHealth;


            StatAbst armour = character.GetStats(Keywords.KeywordTypeEnum.Shield);
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
                var stats = CharacterStatsManager.GetCharacterStatsHandler(isPlayer);
                HealthBarUI healthBar = HealthBar(isPlayer);
                StatAbst health = stats.GetStats(Keywords.KeywordTypeEnum.Heal);
                StatAbst maxHealth = stats.GetStats(Keywords.KeywordTypeEnum.MaxHealth);
                StatAbst armour = stats.GetStats(Keywords.KeywordTypeEnum.Shield);
                health.OnValueChanged -= healthBar.ChangeHealth;
                maxHealth.OnValueChanged -= healthBar.ChangeMaxHealth;
                armour.OnValueChanged -= Armour(isPlayer).SetArmor;
            }
        }
    }
}
