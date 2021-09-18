using Cards;

namespace Battles.Deck
{
    public class PlayerDeck : DeckAbst
    {
        BuffIcon _deckIcon;
        public PlayerDeck(bool isPlayer,Card[] deckCards, BuffIcon DeckIcon) : base(isPlayer, deckCards)
        {
            _deckIcon = DeckIcon;
        }
        public override void AddCard(Card card)
        {
            base.AddCard(card);
            _deckIcon?.SetAmount(GetAmountOfFilledSlots);
        }
        public override void ResetDeck()
        {
            if (isPlayer)
                SetDeck = Managers.PlayerManager.Instance.Deck;
            
            else
            SetDeck = EnemyManager.Instance.Deck;


            _deckIcon?.SetAmount(GetAmountOfFilledSlots);
        }
        public override void DiscardCard(in Card card)
        {
            base.DiscardCard(card);
            _deckIcon?.SetAmount(GetAmountOfFilledSlots);
        }
    }
}