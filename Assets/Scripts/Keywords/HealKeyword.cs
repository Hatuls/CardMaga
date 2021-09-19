﻿using Characters.Stats;

namespace Keywords
{
    public class HealKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Heal;

        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if ( keywordData != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.KeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);
                StatsHandler.GetInstance.AddHealh(isToPlayer,keywordData.GetAmountToApply);
            }
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            switch (data.GetTarget)
            {
                case TargetEnum.MySelf:
                    StatsHandler.GetInstance.AddHealh(currentPlayer, data.GetAmountToApply);
                    break;

                case TargetEnum.All:
                    StatsHandler.GetInstance.AddHealh(currentPlayer, data.GetAmountToApply);
                    StatsHandler.GetInstance.AddHealh(!currentPlayer, data.GetAmountToApply);
                    break;

                case TargetEnum.Opponent:
                    StatsHandler.GetInstance.AddHealh(!currentPlayer, data.GetAmountToApply);
                    break;
                case TargetEnum.None:
                default:
                    break;
            }

        }
    }
}