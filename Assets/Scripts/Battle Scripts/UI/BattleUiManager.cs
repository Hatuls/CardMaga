﻿using UnityEngine;
using Unity.Events;
using Keywords;
namespace Battles.UI
{
    public class UpdateUiStats : UnityEngine.Events.UnityEvent<bool,int, KeywordTypeEnum> { }
    public class BattleUiManager : MonoSingleton<BattleUiManager>
    {
        #region Fields

    


        #endregion


        #region Events
        [SerializeField] TextPopUpEvent _textEvent;
        [SerializeField] VoidEvent _endTurn;
 
        #endregion


        public void EndTurn() {

            _endTurn?.Raise();
        }

   
     
        public override void Init()
        {
           
        }

        public static System.Action<bool, int, KeywordTypeEnum> _buffEvent;

        public void UpdateUiStats(bool isPlayer, int Amount, KeywordTypeEnum actionTypeEnum)
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
                    StatsUIManager.GetInstance.UpdateHealthBar(isPlayer, Amount);
                    break;
                case KeywordTypeEnum.Burn:
                case KeywordTypeEnum.Protected:
                case KeywordTypeEnum.Rage:
                case KeywordTypeEnum.Weak:
                case KeywordTypeEnum.Vulnerable:
                case KeywordTypeEnum.Bleed:
                case KeywordTypeEnum.Strength:
                case KeywordTypeEnum.Dexterity:
                case KeywordTypeEnum.Regeneration:
                case KeywordTypeEnum.StaminaShards:
                case KeywordTypeEnum.StunShard:
                case KeywordTypeEnum.RageShard:
                case KeywordTypeEnum.ProtectionShard:
                    _buffEvent.Invoke(isPlayer,Amount ,actionTypeEnum );
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



