﻿using Keywords;
using ReiTools.TokenMachine;
using Unity.Events;
using UnityEngine;

namespace Battle.UI
{
    public class UpdateUiStats : UnityEngine.Events.UnityEvent<bool, int, KeywordTypeEnum> { }
    public class BattleUiManager : MonoSingleton<BattleUiManager>
    {
        #region Fields



        [SerializeField]
        Tutorial.TutorialManager _tutorialManager;
        #endregion


        #region Events
        [SerializeField] VoidEvent _endTurn;

        public static System.Action<bool, int, KeywordTypeEnum> _buffEvent;
        #endregion

        #region MonoBehaviour Callbacks
        public override void Awake()
        {
            base.Awake();
            SceneHandler.OnBeforeSceneShown += Init;
        }
        public void OnDestroy()
        {
            SceneHandler.OnBeforeSceneShown -= Init;
        }
        #endregion
        public void EndTurn()
        {

            _endTurn?.Raise();
        }


        // Need To be Re-Done
        public override void Init(ITokenReciever token)
        {
            using (token.GetToken())
            {

               // if ((Account.AccountManager.Instance.BattleData.Opponent.CharacterData.CharacterSO.CharacterType == CharacterTypeEnum.Tutorial))
                {
                    //   _tutorialManager.StartTutorial();
                }
            }
        }


        public void UpdateUiStats(bool isPlayer, int Amount, KeywordTypeEnum actionTypeEnum)
        {
            switch (actionTypeEnum)
            {
                case KeywordTypeEnum.Attack:

                    //    _textEvent?.Raise(TextType.NormalDMG, TextPopUpHandler.TextPosition(isPlayer), Amount.ToString());
                    StatsUIManager.Instance.UpdateHealthBar(isPlayer, Amount);
                    StatsUIManager.Instance.UpdateShieldBar(isPlayer, Amount);

                    break;

                case KeywordTypeEnum.Shield:

                    //   _textEvent?.Raise(TextType.Shield, TextPopUpHandler.TextPosition(isPlayer), Amount.ToString());
                    StatsUIManager.Instance.UpdateShieldBar(isPlayer, Amount);

                    break;

                case KeywordTypeEnum.Heal:
                    StatsUIManager.Instance.UpdateHealthBar(isPlayer, Amount);
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
                case KeywordTypeEnum.Stun:
                case KeywordTypeEnum.RageShard:
                case KeywordTypeEnum.ProtectionShard:
                    _buffEvent.Invoke(isPlayer, Amount, actionTypeEnum);
                    break;

                case KeywordTypeEnum.MaxHealth:
                    StatsUIManager.Instance.UpdateMaxHealthBar(isPlayer, Amount);
                    break;
                default:
                    break;
            }
        }
    }
}



