using CardMaga.Card;
using System;
namespace CardMaga.AI
{
    [Serializable]
    public class AICard : IWeightable
    {
        public CardData Card { get; private set; }
        public int Weight { get; set; }

        public void AssignCard(CardData card) => Card = card;
        public void Reset()
        {
            Card = null;
            Weight = 0;
        }
    }

}