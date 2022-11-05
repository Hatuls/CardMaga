﻿using CardMaga.Battle;

namespace CardMaga.Keywords
{
    public class MaxStaminaKeyword : BaseKeywordLogic
    {
        public MaxStaminaKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(data.GetAmountToApply);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StaminaHandler.AddStaminaAddition(data.GetAmountToApply);
            data.KeywordSO.SoundEventSO.PlaySound();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(-data.GetAmountToApply);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StaminaHandler.AddStaminaAddition(-data.GetAmountToApply);
        }
    }
}