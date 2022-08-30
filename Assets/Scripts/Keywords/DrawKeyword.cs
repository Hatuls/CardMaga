using Battle;
using CardMaga.Battle.UI;

namespace Keywords
{
    public class DrawKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword =>   KeywordTypeEnum.Draw;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            if (data.GetTarget == TargetEnum.MySelf)
            {
             var deck =   playersManager.GetCharacter(true).DeckHandler;
                deck.ResetDeck(Battle.Deck.DeckEnum.Hand);
                CardUIManager.Instance.RemoveHands();
                deck.DrawHand(-
                    Characters.Stats.CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(Keyword).Amount
                    );
                data.KeywordSO.SoundEventSO.PlaySound();
            }
            else
                throw new System.Exception("Keyword Draw: Illegal action - Target is opponent\n cannot draw cards when its not your turn!");
        }
    }
}