using CardMaga.Battle.UI;

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
                Battle.Deck.DeckManager.Instance.ResetCharacterDeck(currentPlayer, Battle.Deck.DeckEnum.Hand);
                CardUIManager.Instance.RemoveHands();
                Battle.Deck.DeckManager.Instance.DrawHand(
                    currentPlayer,
                    Characters.Stats.CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(Keyword).Amount
                    );
                data.KeywordSO.SoundEventSO.PlaySound();
            }
            else
                throw new System.Exception("Keyword Draw: Illegal action - Target is opponent\n cannot draw cards when its not your turn!");
        }
    }
}