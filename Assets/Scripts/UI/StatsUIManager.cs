using System;
using UnityEngine.Events;
using UnityEngine;
namespace Battles.UI
{
    public class StatsUIManager : MonoBehaviour
    {
        [SerializeField] UIBar _playerHealthBar, _enemyHealthBar;
        [SerializeField] UIBar _playerArmorBar, _enemyArmorBar;

        private static StatsUIManager _instance;
        public static StatsUIManager GetInstance
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
        =>(isPlayer ? _playerHealthBar : _enemyHealthBar).SetValueBar(health);
        
        public void UpdateMaxHealthBar(bool isPlayer , int maxHealth)
            => (isPlayer ? _playerHealthBar : _enemyHealthBar).SetMaxValue(maxHealth);

        public void UpdateShieldBar(bool isPlayer, int shield)
        => (isPlayer ? _playerArmorBar : _enemyArmorBar).SetValueBar(shield);

        public void UpdateMaxShieldBar(bool isplayer, int maxShield)
            => (isplayer ? _playerArmorBar : _enemyArmorBar).SetMaxValue(maxShield);
    }
}
