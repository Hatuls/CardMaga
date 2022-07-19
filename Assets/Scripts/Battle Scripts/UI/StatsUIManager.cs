using Characters.Stats;
using UnityEngine;
namespace Battle.UI
{
    public class StatsUIManager : MonoBehaviour
    {
        [SerializeField] UIBar _playerHealthBar, _enemyHealthBar;


        [SerializeField] BuffIconsHandler _playerBuffIconHandler;
        [SerializeField] BuffIconsHandler _opponentBuffIconHandler;
        private static StatsUIManager _instance;
        public static StatsUIManager Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("StatsUIManager is null!");

                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }


        public void UpdateHealthBar(bool isPlayer, int health)
        {
            (isPlayer ? _playerHealthBar : _enemyHealthBar).SetValueBar(health);
            if (isPlayer) 
                PlaySound( health);
        }
        float playerMaxHealth = 0;
        private async void PlaySound(float val)
        {
            if (playerMaxHealth == 0)
            {
                await System.Threading.Tasks.Task.Yield();
                playerMaxHealth = CharacterStatsManager.GetCharacterStatsHandler(true).GetStats(Keywords.KeywordTypeEnum.MaxHealth).Amount;
            }

            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Hp Parameter", val / playerMaxHealth);
        }
        public void InitHealthBar(bool isPlayer, int health)
            => (isPlayer ? _playerHealthBar : _enemyHealthBar).InitValueBar(health);
        public void UpdateMaxHealthBar(bool isPlayer, int maxHealth)
            => (isPlayer ? _playerHealthBar : _enemyHealthBar).SetMaxValue(maxHealth);


        public void UpdateShieldBar(bool isPlayer, int shield)
        {
            (isPlayer ? _playerBuffIconHandler : _opponentBuffIconHandler)?.UpdateArmour(shield);
        }

    }
}
