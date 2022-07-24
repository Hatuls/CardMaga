namespace CardMaga.AI
{
    public class StatCheckTree : Tree<AICard>
    {
        public bool IsPlayer { get; set; }
        public OperatorType Operator { get; set; }
        public Keywords.KeywordTypeEnum Keyword { get; set; }

        public int Amount;

        public  int AttackComboWeight { get; set; }
        public  int UtilityComboWeight { get; set; }
        public  int DefenseComboWeight { get; set; }

        public int AttackWeight { get; set; }
        public int UtilityWeight { get; set; }
        public int DefenseWeight { get; set; }
        public override void SetupTree()
        {
            Children = new IEvaluator<AICard>[]
            {
                new AndNode<AICard>
                {
                   Children =new IEvaluator<AICard>[]
                   {
                       new StatNode()
                       {
                           IsPlayer = IsPlayer,
                           Operator = Operator,
                           KeywordType = Keyword,
                           Amount = Amount
                       },
                      new TryPlayCardTree
                      {
                           IsPlayer = IsPlayer,
                           AttackComboWeight = AttackComboWeight,
                           DefenseComboWeight = DefenseComboWeight,
                           UtilityComboWeight = UtilityComboWeight,
                           AttackWeight = AttackWeight,
                           DefenseWeight = DefenseWeight,
                           UtilityWeight = UtilityWeight
                      }
                   }
                }
            };
        }
    }

    public class CardDoKeywordTree : Tree<AICard>
    {
        public bool IsPlayer { get; set; }
        public Keywords.KeywordTypeEnum Keyword { get; set; }
        public int AttackComboWeight { get; set; }
        public int UtilityComboWeight { get; set; }
        public int DefenseComboWeight { get; set; }

        public int DefenseWeight { get; set; }
        public int UtilityWeight { get; set; }
        public int AttackWeight {get;set;}
        public override void SetupTree()
        {
            Children = new IEvaluator<AICard>[]
             {
                new AndNode<AICard>
                {
                   Children =new IEvaluator<AICard>[]
                   {
                       new CardCanDoKeywordNode()
                       {
                            Keyword =Keyword
                       },
                       new TryPlayCardTree
                       {
                           IsPlayer = IsPlayer,
                           AttackComboWeight = AttackComboWeight,
                           DefenseComboWeight = DefenseComboWeight,
                           UtilityComboWeight = UtilityComboWeight,
                           AttackWeight = AttackWeight,
                           DefenseWeight = DefenseWeight,
                           UtilityWeight = UtilityWeight
                       }
                   }
                }
             };
        }
    }

    public class CharacterHealthTree : Tree<AICard>
    {
        public bool IsPlayer { get; set; }
        public OperatorType Operator { get; set; }
        public float Precentage { get; set; }
        public int  AttackComboWeight { get; set; }
        public int UtilityComboWeight { get; set; }
        public int DefenseComboWeight { get; set; }
        public int AttackWeight { get; set; }
        public int UtilityWeight { get; set; }
        public int DefenseWeight { get; set; }
        public override void SetupTree()
        {
            Children = new IEvaluator<AICard>[]
            {
                new AndNode<AICard>
                {
                   Children =new IEvaluator<AICard>[]
                   {
                       new HealthPrecentageNode()
                       {
                           IsPlayer = IsPlayer,
                           Operator = Operator,
                           Precentage = Precentage
                       },
                       new TryPlayCardTree
                       {
                           IsPlayer = IsPlayer,
                           AttackComboWeight = AttackComboWeight,
                           DefenseComboWeight = DefenseComboWeight,
                           UtilityComboWeight = UtilityComboWeight,
                           AttackWeight = AttackWeight,
                           DefenseWeight = DefenseWeight,
                           UtilityWeight = UtilityWeight
                       }
                   }
                }
            };
        }
    }

    public class WillFinishStaminaTree : Tree<AICard>
    {
        public bool IsPlayer { get; set; }
        public int AttackComboWeight { get; set; }
        public int UtilityComboWeight { get; set; }
        public int DefenseComboWeight { get; set; }
        public int AttackWeight { get; set; }
        public int DefenseWeight { get; set; }
        public int UtilityWeight { get; set; }



        public override void SetupTree()
        {
            Children = new IEvaluator<AICard>[]
            {
                new AndNode<AICard>
                {
                   Children =new IEvaluator<AICard>[]
                   {
                       new IsGoingToFinishStamina()
                       {
                         IsPlayer = IsPlayer
                       },
                       new TryPlayCardTree
                       {
                           IsPlayer = IsPlayer,
                           AttackComboWeight = AttackComboWeight,
                           DefenseComboWeight = DefenseComboWeight,
                           UtilityComboWeight = UtilityComboWeight,
                           AttackWeight = AttackWeight,
                           DefenseWeight = DefenseWeight,
                           UtilityWeight = UtilityWeight,
                       }
                   }
                }
            };
        }
    }
}
