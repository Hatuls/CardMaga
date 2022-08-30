using Battle;

namespace Keywords
{
    public class ClearKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword =>  KeywordTypeEnum.Clear;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
        
            data.KeywordSO.SoundEventSO.PlaySound();
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                playersManager.GetCharacter(currentPlayer).DeckHandler.ResetDeck(Battle.Deck.DeckEnum.CraftingSlots);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                playersManager.GetCharacter(!currentPlayer).DeckHandler.ResetDeck(Battle.Deck.DeckEnum.CraftingSlots);

        }
    }
}