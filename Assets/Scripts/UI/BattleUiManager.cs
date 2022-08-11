using Characters.Stats;
using Keywords;
using Managers;
using ReiTools.TokenMachine;
using Unity.Events;
using UnityEngine;

namespace Battle.UI
{
    public class UpdateUiStats : UnityEngine.Events.UnityEvent<bool, int, KeywordTypeEnum> { }
    public class BattleUiManager :MonoBehaviour 
    {
        #region Fields

        [SerializeField]
        Tutorial.TutorialManager _tutorialManager;
        #endregion


        #region Events
        [SerializeField] VoidEvent _endTurn;

        public static System.Action<bool, int, KeywordTypeEnum> _buffEvent;

        public int Priority => throw new System.NotImplementedException();

        public OrderType Order => throw new System.NotImplementedException();
        #endregion


        public void EndTurn()
        {
            _endTurn?.Raise();
        }

        public void UpdateUiStats(bool isPlayer, int Amount, KeywordTypeEnum actionTypeEnum)
        {
            switch (actionTypeEnum)
            {
                case KeywordTypeEnum.Shield:

                    //   _textEvent?.Raise(TextType.Shield, TextPopUpHandler.TextPosition(isPlayer), Amount.ToString());
                   // StatsUIManager.Instance.UpdateShieldBar(isPlayer, Amount);

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
                    break;
                case KeywordTypeEnum.Attack:
                case KeywordTypeEnum.Heal:
                default:
                    break;
            }
        }

        #region Monobehaviour Callbacks

        private void Awake()
        {
            BaseStat.OnStatsUpdated += UpdateUiStats;
        }
        private void OnDestroy()
        {
            BaseStat.OnStatsUpdated -= UpdateUiStats;
        }
        #endregion
    }
}



