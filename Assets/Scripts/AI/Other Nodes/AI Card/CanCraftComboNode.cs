using Battle;
using CardMaga.Battle;
using CardMaga.Card;
using Cards;
using System.Collections.Generic;
using System.Linq;
namespace CardMaga.AI
{
    public class CanCraftComboNode : BaseNode<AICard>
    {
        public bool IsPlayer { get => _isPlayer; set => _isPlayer = value; }
        public IBattleManager BM { get => _bM; set => _bM = value; }
        private CraftingHandler _craftingHandler;
        private IBattleManager _bM;
        private bool _isPlayer;

        public CanCraftComboNode() : base()
        {
            if (BM == null)
                BM = BattleManager.Instance;
            _craftingHandler = BM.PlayersManager.GetCharacter(IsPlayer).CraftingHandler;
        }


        public override NodeState Evaluate(AICard basedEvaluationObject)
        {

            CardTypeData[] craftingSlots = new CardTypeData[_craftingHandler.CardsTypeData.Count() + 1];
            System.Array.Copy(_craftingHandler.CardsTypeData.ToArray(), craftingSlots, 0);
            // CardData[] craftingSlots = new CardData[deck.GetDeck.Length + 1];

            //  System.Array.Copy(deck.GetDeck, craftingSlots, deck.GetDeck.Length);
            craftingSlots[craftingSlots.Length - 1] = basedEvaluationObject.Card.CardTypeData;

            // checking how many of them are not null
            List<CardTypeData> craftingItems;

            if (GetCraftingSlotsFilled() > 1)
                CheckRecipe();
            else
                NodeState = NodeState.Failure;

            return NodeState;


            int GetCraftingSlotsFilled()
            {
                int amountCache = _craftingHandler.CountFullSlots;

                craftingItems = new List<CardTypeData>(amountCache);

                for (int i = 0; i < craftingSlots.Length; i++)
                {
                    if (craftingSlots[i] != null)
                        craftingItems.Add(craftingSlots[i]);
                }

                return amountCache;
            }
            void CheckRecipe()
            {
                // need to make algorithem better!!! 
                var recipes = BM.PlayersManager.GetCharacter(IsPlayer).Combos.GetCollection.ToArray();

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