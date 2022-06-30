using Characters.Stats;
using System.Collections.Generic;

namespace Keywords
{
    public class CoinKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword =>  KeywordTypeEnum.Coins;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            data.KeywordSO.SoundEventSO.PlaySound();
            var target = data.GetTarget;
            if (target == TargetEnum.All|| target == TargetEnum.MySelf)
            {
                var gold = CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(Keyword);
                if (data.GetAmountToApply > 0)
                    gold.Add(data.GetAmountToApply);
                else
                    gold.Reduce(-1*data.GetAmountToApply);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var gold = CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer).GetStats(Keyword);
                if (data.GetAmountToApply > 0)
                    gold.Add(data.GetAmountToApply);
                else
                    gold.Reduce(-1 * data.GetAmountToApply);
            }
        }
    } 
    public class DoubleKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword =>  KeywordTypeEnum.Double;

        public async override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            data.KeywordSO.SoundEventSO.PlaySound();
            var target = data.GetTarget;
            await System.Threading.Tasks.Task.Yield();
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                var cards = Battle.Deck.DeckManager.GetCraftingSlots(currentPlayer);
                Battle.Deck.DeckManager.AddToCraftingSlot(currentPlayer, cards.LastCardEntered);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var cards = Battle.Deck.DeckManager.GetCraftingSlots(!currentPlayer);
                Battle.Deck.DeckManager.AddToCraftingSlot(!currentPlayer, cards.LastCardEntered);

            }
        }
      

    }
}