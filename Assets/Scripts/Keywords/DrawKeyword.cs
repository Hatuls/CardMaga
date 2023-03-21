using Battle.Deck;
using CardMaga.Battle;
using System;

namespace CardMaga.Keywords
{
    public class DrawKeyword : BaseKeywordLogic
    {
        public DrawKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public event Action OnDrawKeywordActivated;

        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf)
            {
                var deck = _playersManager.GetCharacter(true).DeckHandler;
                deck.ResetDeck(DeckEnum.Hand);

                OnDrawKeywordActivated?.Invoke();
                int drawAmount = _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Amount;

                deck.DrawHand(-drawAmount);
                KeywordSO.SoundEventSO.PlaySound();
            }
            else
                throw new System.Exception("Keyword Draw: Illegal action - Target is opponent\n cannot draw cards when its not your turn!");
        }

        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            throw new NotImplementedException();
        }
    }
}