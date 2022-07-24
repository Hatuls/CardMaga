namespace CardMaga.AI
{
    public class TryPlayCardTree : Tree<AICard>
    {
        public bool IsPlayer { get; set; }
        public int AttackWeight { get; set; }
        public int UtilityWeight { get; set; }
        public int DefenseWeight { get; set; }
        public override void SetupTree()
        {
            Children = new IEvaluator<AICard>[]
            {
                new ORNode<AICard>
                {
                    Children = new IEvaluator<AICard>[]
                    {
                        new CardTypeDecisionTree()
                        {
                                IsPlayer = IsPlayer,
                                Type = Card.CardTypeEnum.Attack,
                                WeightToAdd = AttackWeight,
                        },
                        new CardTypeDecisionTree()
                        {
                                IsPlayer = IsPlayer,
                                  Type = Card.CardTypeEnum.Defend,
                                WeightToAdd = DefenseWeight,
                        },
                        new CardTypeDecisionTree()
                        {
                                IsPlayer = IsPlayer,
                                    Type = Card.CardTypeEnum.Utility,
                                WeightToAdd = UtilityWeight,
                        }
                    }
                }
            };
        }
    }
}
