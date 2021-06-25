using UnityEngine;
namespace Battles.UI

{
    public class UpdateUiStats : UnityEngine.Events.UnityEvent<bool, int, Keywords.KeywordTypeEnum> { }
    public class BattleUiManager : MonoSingleton<BattleUiManager>
    {
        #region Fields
        [SerializeField]
        BuffIconsHandler _playerBuffHandler;
        [SerializeField]
        BuffIconsHandler _enemyBuffHandler;
        #endregion
        public void SetBuffUI(bool isPlayer, BuffIcons icon)
        {
            if(_playerBuffHandler == null || _enemyBuffHandler == null)
            {
                Debug.LogError("Error in SetBuffUI");
                return;
            }
            if(isPlayer)
            {
                _playerBuffHandler.SetBuffIcon(icon);
            }
            else
            {
                _enemyBuffHandler.SetBuffIcon(icon);
            }
        }
        public void RemoveBuffUI(bool isPlayer, BuffIcons icon)
        {
            if (_playerBuffHandler == null || _enemyBuffHandler == null)
            {
                Debug.LogError("Error in RemoveBuffUI");
                return;
            }
            if (isPlayer)
            {
                _playerBuffHandler.RemoveBuffIcon(icon);
            }
            else
            {
                _enemyBuffHandler.RemoveBuffIcon(icon);
            }
        }
        public override void Init()
        {
           
        }


        public void UpdateUiStats(bool isPlayer, int Amount, Keywords.KeywordTypeEnum actionTypeEnum)
        {
             var characterStats = Characters.Stats.StatsHandler.GetInstance.GetCharacterStats(isPlayer);
            switch (actionTypeEnum)
            {
                case Keywords.KeywordTypeEnum.Attack:
                    TextPopUpHandler.GetInstance.CreatePopUpText(TextType.NormalDMG, isPlayer, Amount.ToString());
                    StatsUIManager.GetInstance.UpdateHealthBar(isPlayer, characterStats.Health);
                    StatsUIManager.GetInstance.UpdateShieldBar(isPlayer, characterStats.Shield);
                    break;

                case Keywords.KeywordTypeEnum.Defense:
                    TextPopUpHandler.GetInstance.CreatePopUpText(TextType.Shield, isPlayer, Amount.ToString());
                    StatsUIManager.GetInstance.UpdateShieldBar(isPlayer, characterStats.Shield);
                    break;

                case Keywords.KeywordTypeEnum.Heal:
                    TextPopUpHandler.GetInstance.CreatePopUpText(TextType.Healing, isPlayer, Amount.ToString());
                    StatsUIManager.GetInstance.UpdateHealthBar(isPlayer, characterStats.Health);
                    break;

                case Keywords.KeywordTypeEnum.Strength:
                    SetBuffUI(isPlayer, BuffIcons.Strength);
                    break;

                case Keywords.KeywordTypeEnum.Bleed:
                    SetBuffUI(isPlayer, BuffIcons.Bleed);
                    StatsUIManager.GetInstance.UpdateHealthBar(isPlayer, characterStats.Health);
                    break;

                case Keywords.KeywordTypeEnum.MaxHealth:
                    StatsUIManager.GetInstance.UpdateMaxHealthBar(isPlayer, characterStats.MaxHealth);
                    break;
                default:
                    break;
            }

        }
    }
}
