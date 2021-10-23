using System;
using UnityEngine.Events;
using UnityEngine;
namespace Battles.UI
{
    public class StatsUIManager : MonoBehaviour
    {
        [SerializeField] UIBar _playerHealthBar, _enemyHealthBar;


        [SerializeField] BuffIconsHandler _playerBuffIconHandler;
        [SerializeField] BuffIconsHandler _opponentBuffIconHandler;
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
        public void InitHealthBar(bool isPlayer,int health)
            => (isPlayer? _playerHealthBar : _enemyHealthBar).InitValueBar(health);
        public void UpdateMaxHealthBar(bool isPlayer , int maxHealth)
            => (isPlayer ? _playerHealthBar : _enemyHealthBar).SetMaxValue(maxHealth);


        public void UpdateShieldBar(bool isPlayer, int shield)
        {
         (isPlayer ? _playerBuffIconHandler : _opponentBuffIconHandler)?.UpdateArmour(shield);
        
        }

    }
}
