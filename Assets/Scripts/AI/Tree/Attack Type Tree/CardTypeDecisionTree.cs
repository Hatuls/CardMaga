namespace CardMaga.AI
{
    public class CardTypeDecisionTree : Tree<AICard>
    {
        public bool IsPlayer { get; set; }
        public Card.CardTypeEnum Type { get; set; }
        public int WeightToAdd { get; set; }
        public void AttachTree()
        {
            Parent.Attach(null);
        }
        public override void SetupTree()
        {
            Children = new IEvaluator<AICard>[]
            {
                new AndNode<AICard>()
                 {
                  Children = new IEvaluator<AICard>[]
                  {
                    new CheckCardTypeNode{ CardType = Type},
                    new ORNode<AICard>
                    {
                         Children = new IEvaluator<AICard>[]
                         {
                             new AndNode<AICard>
                             {
                                 Children = new IEvaluator<AICard>[]
                                 {
                                     new CanCraftComboNode(){ IsPlayer = IsPlayer},
                                     new AddWeightToCardsWeightNode() { Weight = WeightToAdd }
                                 }
                             },
                             new UseCardsValueAsWeightNode()
                         }
                    }
                  }
                }
            };
        }
    }

    public class NoStaminaTree : Tree<AICard>
    {
        public bool IsPlayer { get; set; }
        public override void SetupTree()
        {
            Children = new IEvaluator<AICard>[]
            {
             new AndNode<AICard>
             {
                Children = new IEvaluator<AICard>[]
                {
                    new InvertNode<AICard>{ Children = new IEvaluator<AICard>[1] { new HaveEnoughStaminaLogic() { IsPlayer = IsPlayer} }},
                    new AssignWeightNode(){ Weight = -1 }
                }
             }
            };
        }
    }

 
}
