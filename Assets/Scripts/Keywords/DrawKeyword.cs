using Battle;
using CardMaga.Battle.UI;

namespace Keywords
{
    public class DrawKeyword : BaseKeywordLogic
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
                 playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword).Amount
                    );
                data.KeywordSO.SoundEventSO.PlaySound();
            }
            else
                throw new System.Exception("Keyword Draw: Illegal action - Target is opponent\n cannot draw cards when its not your turn!");
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
           // Undo Logic missing
        }
    }
}