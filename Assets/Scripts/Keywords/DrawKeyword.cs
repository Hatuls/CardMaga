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

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data.GetTarget == TargetEnum.MySelf)
            {
                var deck = _playersManager.GetCharacter(true).DeckHandler;
                deck.ResetDeck(DeckEnum.Hand);

                OnDrawKeywordActivated?.Invoke();
                //    CardUIManager.Instance.RemoveHands();


                deck.DrawHand(-_playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Amount
                    );
                data.KeywordSO.SoundEventSO.PlaySound();
            }
            else
                throw new System.Exception("Keyword Draw: Illegal action - Target is opponent\n cannot draw cards when its not your turn!");
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            // Undo Logic missing
        }
    }
}