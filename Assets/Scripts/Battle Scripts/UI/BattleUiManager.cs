using UnityEngine;
using Unity.Events;
using Keywords;
namespace Battles.UI

{
    public class UpdateUiStats : UnityEngine.Events.UnityEvent<bool,int, KeywordTypeEnum> { }
    public class BattleUiManager : MonoSingleton<BattleUiManager>
    {
        #region Fields
        [SerializeField]
        BuffIconsHandler _playerBuffHandler;
        [SerializeField]
        BuffIconsHandler _enemyBuffHandler;

        #endregion


        #region Events
        [SerializeField] TextPopUpEvent _textEvent;
        [SerializeField] VoidEvent _endTurn;
 
        #endregion


        public void EndTurn() {

            _endTurn?.Raise();
        }



        public void SetBuffUI(bool isPlayer, KeywordTypeEnum icon,int amount)
        {
            if(_playerBuffHandler == null || _enemyBuffHandler == null)
            {
                Debug.LogError("Error in SetBuffUI");
                return;
            }

           


            if(isPlayer)
            {
                _playerBuffHandler.SetBuffIcon(icon , amount);
            }
            else
            {
                _enemyBuffHandler.SetBuffIcon(icon , amount);
            }
        }
        public void RemoveBuffUI(bool isPlayer, KeywordTypeEnum icon)
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


        public void UpdateUiStats(bool isPlayer,int Amount, KeywordTypeEnum actionTypeEnum)
        {
            switch (actionTypeEnum)
            {
                case KeywordTypeEnum.Attack:

                //    _textEvent?.Raise(TextType.NormalDMG, TextPopUpHandler.TextPosition(isPlayer), Amount.ToString());
                    StatsUIManager.GetInstance.UpdateHealthBar(isPlayer, Amount);
                    StatsUIManager.GetInstance.UpdateShieldBar(isPlayer, Amount);
                    break;

                case KeywordTypeEnum.Shield:
        
                 //   _textEvent?.Raise(TextType.Shield, TextPopUpHandler.TextPosition(isPlayer), Amount.ToString());
                    StatsUIManager.GetInstance.UpdateShieldBar(isPlayer, Amount);

                    break;

                case KeywordTypeEnum.Heal:
               
                //    _textEvent?.Raise(TextType.Healing, TextPopUpHandler.TextPosition(isPlayer), Amount.ToString());
                    StatsUIManager.GetInstance.UpdateHealthBar(isPlayer, Amount);
                    break;

                case KeywordTypeEnum.Strength:
                    SetBuffUI(isPlayer, KeywordTypeEnum.Strength, Amount);
                    break;

                case KeywordTypeEnum.Bleed:
                    SetBuffUI(isPlayer, KeywordTypeEnum.Bleed , Amount);
                   // StatsUIManager.GetInstance.UpdateHealthBar(isPlayer, Amount);
                    break;

                case KeywordTypeEnum.MaxHealth:
                    StatsUIManager.GetInstance.UpdateMaxHealthBar(isPlayer, Amount);
                    break;
                default:
                    break;
            }
        }
    }
}



