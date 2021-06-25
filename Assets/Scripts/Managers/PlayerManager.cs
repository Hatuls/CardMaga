using UnityEngine;


namespace Managers
{



    public class PlayerManager : MonoSingleton<PlayerManager> , Battles.IBattleHandler
    {
        #region Fields

        [Tooltip("Player Stats: ")]
        [SerializeField] Characters.Stats.CharacterStats _playerStat;
        [SerializeField] AnimatorController _playerAnimatorController;
     
        #endregion
        public ref Characters.Stats.CharacterStats GetCharacterStats =>ref  _playerStat;
        public AnimatorController PlayerAnimatorController => _playerAnimatorController;
        public override void Init()
        {
            _playerStat = Battles.BattleManager.GetDictionary(typeof(PlayerManager)).GetCharacter(Battles.CharactersEnum.Player).GetCharacterStats;
            Battles.UI.StatsUIManager.GetInstance.UpdateMaxHealthBar(true, _playerStat.MaxHealth);
            Battles.UI.StatsUIManager.GetInstance.UpdateHealthBar(true, _playerStat.Health);
            Battles.UI.StatsUIManager.GetInstance.UpdateMaxShieldBar(true, _playerStat.MaxHealth/4);
            Battles.UI.StatsUIManager.GetInstance.UpdateShieldBar(true, _playerStat.Shield);
        }

        public void OnEndBattle()
        {
         
        }

        public void OnStartBattle()
        {
         
        }
    }

}
