using Battle;
using Battle.Deck;
using CardMaga.Card;
using Cards;
using Managers;
using System.Collections.Generic;
using System.Linq;
namespace CardMaga.AI
{
    public class CanCraftComboNode : BaseNode<AICard>
    {
        public bool IsPlayer { get; set; }
        public BattleManager BM { get; set; }
        private DeckHandler _deckHandler;
        public CanCraftComboNode() : base()
        {
            if (BM == null)
                BM = BattleManager.Instance;
            _deckHandler = BM.PlayersManager.GetCharacter(IsPlayer).DeckHandler;
        }


        public override NodeState Evaluate(AICard basedEvaluationObject)
        {

            var deck = _deckHandler[DeckEnum.CraftingSlots] as PlayerCraftingSlots; //DeckManager.GetCraftingSlots(IsPlayer);

            CardData[] craftingSlots = new CardData[deck.GetDeck.Length + 1];

            System.Array.Copy(deck.GetDeck, craftingSlots, deck.GetDeck.Length);
            craftingSlots[craftingSlots.Length - 1] = basedEvaluationObject.Card;

            // checking how many of them are not null
            List<CardTypeData> craftingItems;

            if (GetCraftingSlotsFilled() > 1)
                CheckRecipe();
            else
                NodeState = NodeState.Failure;

            return NodeState;


            int GetCraftingSlotsFilled()
            {
                int amountCache = deck.AmountOfFilledSlots;
           
                craftingItems = new List<CardTypeData>(amountCache);

                for (int i = 0; i < craftingSlots.Length; i++)
                {
                    if (craftingSlots[i] != null)
                        craftingItems.Add(craftingSlots[i].CardSO.CardType);
                }

                return amountCache;
            }
            void CheckRecipe()
            {
                // need to make algorithem better!!! 
                var recipes = BM.PlayersManager.GetCharacter(IsPlayer).Combos;

                var comparer = new CardTypeComparer();
                CardTypeData[] cardTypeDatas;
                for (int i = 0; i < recipes.Length; i++)
                {
                    var comboSO = recipes[i].ComboSO;
                    cardTypeDatas = new CardTypeData[comboSO.ComboSequence.Length];

                    for (int j = 0; j < comboSO.ComboSequence.Length; j++)
                        cardTypeDatas[j] = comboSO.ComboSequence[j];


                    if (craftingItems.SequenceEqual(cardTypeDatas, comparer))
                    {
                        NodeState = NodeState.Success;
                        return;
                    }

                }
                NodeState = NodeState.Failure;
            }
        }
    }
}