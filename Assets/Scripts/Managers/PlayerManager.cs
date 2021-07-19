using UnityEngine;


namespace Managers
{



    public class PlayerManager : MonoSingleton<PlayerManager> , Battles.IBattleHandler
    {
        #region Fields

        [Tooltip("Player Stats: ")]
        [SerializeField] Characters.Stats.CharacterStats _playerStat;
        [SerializeField] AnimatorController _playerAnimatorController;
        [SerializeField] Battles.CharacterSO _playerCards;

        #endregion
        public ref Characters.Stats.CharacterStats GetCharacterStats =>ref  _playerStat;
        public AnimatorController PlayerAnimatorController => _playerAnimatorController;
        public override void Init()
        {
            _playerStat = Battles.BattleManager.GetDictionary(typeof(PlayerManager)).GetCharacter(Battles.CharactersEnum.Player).GetCharacterStats;
            Battles.UI.StatsUIManager.GetInstance.UpdateMaxHealthBar(true, _playerStat.MaxHealth);
         //   Battles.UI.StatsUIManager.GetInstance.UpdateHealthBar(true, _playerStat.Health);

            CardManager.Instance.AssignPlayerCardDict(_playerCards.GetCharacterCards);
            Characters.Stats.StaminaHandler.SetMaxStamina = _playerCards.GetMaxStamina;
        }
        private void Start()
        {
                        Battles.UI.StatsUIManager.GetInstance.InitHealthBar(true, _playerStat.Health);

            Battles.UI.StatsUIManager.GetInstance.UpdateShieldBar(true, _playerStat.Shield);
        }
        public void OnEndBattle()
        {
         
        }

        public void OnStartBattle()
        {
         
        }
    }









    //public abstract class AnimationState
    //{
    //    public string _animationName;
    //    public AnimationState(string animName)
    //    {
    //        _animationName = animName;
    //    }
    //}

    //public class NormalAttack : AnimationState
    //{
    //    public Keywords.KeywordData keyword;
    //}

    //public class ComboAttack : AnimationState
    //{
    //    public Keywords.KeywordData[] keyword;
    //    Relics.RelicSO combo;
    //}
}
