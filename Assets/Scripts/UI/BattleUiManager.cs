﻿using UnityEngine;
using Unity.Events;
namespace Battles.UI

{
    public class UpdateUiStats : UnityEngine.Events.UnityEvent<bool,int, Keywords.KeywordTypeEnum> { }
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



        public void SetBuffUI(bool isPlayer, BuffIcons icon,int amount)
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


        public void UpdateUiStats(bool isPlayer,int Amount, Keywords.KeywordTypeEnum actionTypeEnum)
        {
            switch (actionTypeEnum)
            {
                case Keywords.KeywordTypeEnum.Attack:

                //    _textEvent?.Raise(TextType.NormalDMG, TextPopUpHandler.TextPosition(isPlayer), Amount.ToString());
                    StatsUIManager.GetInstance.UpdateHealthBar(isPlayer, Amount);
                    StatsUIManager.GetInstance.UpdateShieldBar(isPlayer, Amount);
                    break;

                case Keywords.KeywordTypeEnum.Shield:
        
                 //   _textEvent?.Raise(TextType.Shield, TextPopUpHandler.TextPosition(isPlayer), Amount.ToString());
                    StatsUIManager.GetInstance.UpdateShieldBar(isPlayer, Amount);

                    break;

                case Keywords.KeywordTypeEnum.Heal:
               
                //    _textEvent?.Raise(TextType.Healing, TextPopUpHandler.TextPosition(isPlayer), Amount.ToString());
                    StatsUIManager.GetInstance.UpdateHealthBar(isPlayer, Amount);
                    break;

                case Keywords.KeywordTypeEnum.Strength:
                    SetBuffUI(isPlayer, BuffIcons.Strength, Amount);
                    break;

                case Keywords.KeywordTypeEnum.Bleed:
                    SetBuffUI(isPlayer, BuffIcons.Bleed , Amount);
                   // StatsUIManager.GetInstance.UpdateHealthBar(isPlayer, Amount);
                    break;

                case Keywords.KeywordTypeEnum.MaxHealth:
                    StatsUIManager.GetInstance.UpdateMaxHealthBar(isPlayer, Amount);
                    break;
                default:
                    break;
            }
        }
    }
}



