namespace Keywords
{
    public class DrawKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword =>   KeywordTypeEnum.Draw;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data.GetTarget == TargetEnum.MySelf)
            {
                //  Battles.Deck.DeckManager.Instance.OnEndTurn(currentPlayer);
                Battles.Deck.DeckManager.Instance.ResetCharacterDeck(currentPlayer, Battles.Deck.DeckEnum.Hand);
                Battles.UI.CardUIManager.Instance.RemoveHands();
                Battles.Deck.DeckManager.Instance.DrawHand(
                    currentPlayer,
                    Characters.Stats.CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(Keyword).Amount
                    );
            }
            else
                throw new System.Exception("Keyword Draw: Illegal action - Target is opponent\n cannot draw cards when its not your turn!");
        }
    }
}