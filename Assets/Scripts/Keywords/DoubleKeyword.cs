﻿using CardMaga.Battle;
using CardMaga.Card;
using Characters.Stats;
using System.Collections.Generic;

namespace CardMaga.Keywords
{

    public class DoubleKeyword : BaseKeywordLogic
    {
        private List<CardTypeData> _leftCardTypeDatas;
        private List<CardTypeData> _rightCardTypeDatas;

        public DoubleKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            data.KeywordSO.SoundEventSO.PlaySound();
            var target = data.GetTarget;
            var characters = _playersManager;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                var craftingSlots = characters.GetCharacter(currentPlayer).CraftingHandler;
                _leftCardTypeDatas = CardTypeDatas(craftingSlots.CraftingSlots);
                craftingSlots.AddFront(craftingSlots.LastCardTypeData, false);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var craftingSlots = characters.GetCharacter(!currentPlayer).CraftingHandler;
                _rightCardTypeDatas = CardTypeDatas(craftingSlots.CraftingSlots);
                craftingSlots.AddFront(craftingSlots.LastCardTypeData, false);
            }


            List<CardTypeData> CardTypeDatas(IReadOnlyList<CraftingSlot> craftingSlots)
            {
                int length = craftingSlots.Count;
                List<CardTypeData> _cardTypeData = new List<CardTypeData>(length);
                for (int i = 0; i < length; i++)
                    _cardTypeData.Add(craftingSlots[i].CardType);

                return _cardTypeData;
            }
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            var target = data.GetTarget;
            var characters = _playersManager;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                var craftingSlots = characters.GetCharacter(currentPlayer).CraftingHandler;
                craftingSlots.AssignCraftingSlots(_leftCardTypeDatas);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var craftingSlots = characters.GetCharacter(!currentPlayer).CraftingHandler;
                craftingSlots.AssignCraftingSlots(_rightCardTypeDatas);
            }

        }
    }


    public class PierceDamageKeyword : BaseKeywordLogic
    {
        int _amount;

        public PierceDamageKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            data.KeywordSO.SoundEventSO.PlaySound();
            var target = data.GetTarget;
            _amount = data.GetAmountToApply;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                CharacterStatsHandler applier = _playersManager.GetCharacter(currentPlayer).StatsHandler;
                applier.GetStat(KeywordType.Heal).Reduce(_amount);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                CharacterStatsHandler applier = _playersManager.GetCharacter(!currentPlayer).StatsHandler;
                applier.GetStat(KeywordType.Heal).Reduce(_amount);
            }
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                CharacterStatsHandler applier = _playersManager.GetCharacter(currentPlayer).StatsHandler;
                applier.GetStat(KeywordType.Heal).Add(_amount);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                CharacterStatsHandler applier = _playersManager.GetCharacter(!currentPlayer).StatsHandler;
                applier.GetStat(KeywordType.Heal).Add(_amount);
            }
        }
    }
}