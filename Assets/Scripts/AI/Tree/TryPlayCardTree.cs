namespace CardMaga.AI
{
    public class TryPlayCardTree : Tree<AICard>
    {
        public bool IsPlayer { get; set; }
        public int AttackComboWeight { get; set; }
        public int UtilityComboWeight { get; set; }
        public int DefenseComboWeight { get; set; }

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
                                WeightToAddForCombo = AttackComboWeight,
                                WeightToAddForCard = AttackWeight
                        },
                        new CardTypeDecisionTree()
                        {
                                IsPlayer = IsPlayer,
                                Type = Card.CardTypeEnum.Defend,
                                WeightToAddForCombo = DefenseComboWeight,
                                WeightToAddForCard = DefenseWeight,
                        },
                        new CardTypeDecisionTree()
                        {
                                IsPlayer = IsPlayer,
                                Type = Card.CardTypeEnum.Utility,
                                WeightToAddForCombo = UtilityComboWeight,
                                WeightToAddForCard = UtilityWeight
                        }
                    }
                }
            };
        }
    }
}
