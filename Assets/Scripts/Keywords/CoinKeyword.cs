using Battle;

namespace Keywords
{
    public class CoinKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Coins;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            data.KeywordSO.SoundEventSO.PlaySound();
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                var gold = playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword);
                if (data.GetAmountToApply > 0)
                    gold.Add(data.GetAmountToApply);
                else
                    gold.Reduce(-1 * data.GetAmountToApply);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var gold = playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword);
                if (data.GetAmountToApply > 0)
                    gold.Add(data.GetAmountToApply);
                else
                    gold.Reduce(-1 * data.GetAmountToApply);
            }
        }
    }
    public class DoubleKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Double;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            data.KeywordSO.SoundEventSO.PlaySound();
            var target = data.GetTarget;
            var characters = playersManager;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                var craftingSlots = characters.GetCharacter(currentPlayer).CraftingHandler;
                craftingSlots.AddFront(craftingSlots.LastCardTypeData, false) ;
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var craftingSlots = characters.GetCharacter(!currentPlayer).CraftingHandler;
                craftingSlots.AddFront(craftingSlots.LastCardTypeData, false);
            }
        }
    }
}