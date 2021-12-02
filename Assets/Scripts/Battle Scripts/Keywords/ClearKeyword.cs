namespace Keywords
{
    public class ClearKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword =>  KeywordTypeEnum.Clear;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            data.KeywordSO.SoundEventSO.PlaySound();
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                Battles.Deck.DeckManager.GetCraftingSlots(currentPlayer).ResetDeck();

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                Battles.Deck.DeckManager.GetCraftingSlots(!currentPlayer).ResetDeck();
        }
    }
}