namespace CardMaga.AI
{
    public class AITree : Tree<AICard>
    {
        private AIBrain _brain;
        private bool _amIPlayerLeft;
        public AITree(bool isPlayer, AIBrain brain)
        {
            _brain = brain;
            _amIPlayerLeft = isPlayer;
            Attach(null);
        }
        public override void SetupTree()
        {
            Children = new IEvaluator<AICard>[]
            {
               new ORNode<AICard> // 1
               {
                 Children = new IEvaluator<AICard>[]
                 {
                     new NoStaminaTree() { IsPlayer = _amIPlayerLeft },
                     new ORNode<AICard> //2
                     {
                        Children = new IEvaluator<AICard>[]
                        {

                             new AndNode<AICard> //3
                             {
                                 Children = new IEvaluator<AICard>[]
                                 {
                                     new HealthPrecentageNode // Section N
                                     {
                                        IsPlayer = _amIPlayerLeft,
                                        Operator = _brain.SectionN.MathOperation,
                                        Precentage = _brain.SectionN.Amount,
                                     },
                                     new ORNode<AICard> // 4
                                     {
                                          Children = new IEvaluator<AICard>[]
                                          {
                                              new AndNode<AICard> //5
                                              {
                                                 Children = new IEvaluator<AICard>[]
                                                 {
                                                     new StatNode
                                                     {
                                                       IsPlayer = _amIPlayerLeft,
                                                       KeywordType= Keywords.KeywordTypeEnum.Bleed,
                                                       Operator = OperatorType.BiggerThan,
                                                       Amount = 0
                                                     },
                                                     new ORNode<AICard>//6
                                                     {
                                                         Children = new IEvaluator<AICard>[]
                                                         {
                                                            new AndNode<AICard> //7
                                                            {
                                                                Children = new IEvaluator<AICard>[]
                                                                {
                                                                     new CompareBetweenKeywordsNode()
                                                                     {
                                                                           IsPlayer = _amIPlayerLeft,
                                                                           KeywordA = Keywords.KeywordTypeEnum.Bleed,
                                                                           KeywordB = Keywords.KeywordTypeEnum.Heal,
                                                                           Operator = OperatorType.BiggerThanOrEqualTo,
                                                                     },
                                                                     new ORNode<AICard>//8
                                                                     {
                                                                        Children = new IEvaluator<AICard>[]
                                                                        {
                                                                                       #region A, B , C ,D, 
                                                                              new CharacterHealthTree // section A
                                                                              {
                                                                                   IsPlayer = !_amIPlayerLeft,
                                                                                   Operator = _brain.SectionA.HealthParams.MathOperation,
                                                                                   Precentage = _brain.SectionA.HealthParams.Amount,
                                                                                   AttackWeight =  _brain.SectionA.Weights.AttackWeightAddition,
                                                                                   UtilityWeight = _brain.SectionA.Weights.UtilityWeightAddition,
                                                                                   DefenseWeight = _brain.SectionA.Weights.DefenseWeightAddition,
                                                                              },
                                                                              new CardDoKeywordTree // section B
                                                                              {
                                                                                   IsPlayer = _amIPlayerLeft,
                                                                                   Keyword = _brain.SectionB.Keyword,
                                                                                   AttackWeight =  _brain.SectionB.Weights.AttackWeightAddition,
                                                                                   UtilityWeight = _brain.SectionB.Weights.UtilityWeightAddition,
                                                                                   DefenseWeight = _brain.SectionB.Weights.DefenseWeightAddition,
                                                                              },
                                                                              new WillFinishStaminaTree // section C
                                                                              {
                                                                                   IsPlayer = _amIPlayerLeft,
                                                                                   AttackWeight =  _brain.SectionC.AttackWeightAddition,
                                                                                   UtilityWeight = _brain.SectionC.UtilityWeightAddition,
                                                                                   DefenseWeight = _brain.SectionC.DefenseWeightAddition,
                                                                              },
                                                                              new TryPlayCardTree // Section D
                                                                              {
                                                                                   IsPlayer = _amIPlayerLeft,
                                                                                   AttackWeight =  _brain.SectionD.AttackWeightAddition,
                                                                                   UtilityWeight = _brain.SectionD.UtilityWeightAddition,
                                                                                   DefenseWeight = _brain.SectionD.DefenseWeightAddition,
                                                                              }
                                                            #endregion
                                                                        }
                                                                     }
                                                                }
                                                            },
#region Sections E,F,G
                                                            new CharacterHealthTree// section E
                                                            {
                                                                  IsPlayer = !_amIPlayerLeft,
                                                                  Operator  = _brain.SectionE.HealthParams.MathOperation,
                                                                  Precentage = _brain.SectionE.HealthParams.Amount,
                                                                  AttackWeight =  _brain.SectionE.Weights.AttackWeightAddition,
                                                                  UtilityWeight = _brain.SectionE.Weights.UtilityWeightAddition,
                                                                  DefenseWeight = _brain.SectionE.Weights.DefenseWeightAddition,
                                                            },
                                                            new StatCheckTree // Section F
                                                            {
                                                               IsPlayer = _amIPlayerLeft,
                                                               Keyword = _brain.SectionF.Keyword.Keyword,
                                                               Operator = _brain.SectionF.Values.MathOperation,
                                                               Amount= (int)_brain.SectionF.Values.Amount,
                                                               AttackWeight =  _brain.SectionF.Keyword.Weights.AttackWeightAddition,
                                                               UtilityWeight = _brain.SectionF.Keyword.Weights.UtilityWeightAddition,
                                                               DefenseWeight = _brain.SectionF.Keyword.Weights.DefenseWeightAddition,
                                                            },
                                                            new TryPlayCardTree // Section G
                                                            {
                                                                IsPlayer = _amIPlayerLeft,
                                                                AttackWeight =  _brain.SectionG.AttackWeightAddition,
                                                                UtilityWeight = _brain.SectionG.UtilityWeightAddition,
                                                                DefenseWeight = _brain.SectionG.DefenseWeightAddition,
                                                            }
#endregion
                                                         }
                                                     }
                                                 }
                                              },
#region H,I,J
                                              new CharacterHealthTree // Section H
                                              {
                                                  IsPlayer = !_amIPlayerLeft,
                                                   Operator = _brain.SectionH.HealthParams.MathOperation,
                                                   Precentage = (int) _brain.SectionH.HealthParams.MathOperation,
                                                    AttackWeight = _brain.SectionH.Weights.AttackWeightAddition,
                                                    UtilityWeight = _brain.SectionH.Weights.UtilityWeightAddition,
                                                    DefenseWeight = _brain.SectionH.Weights.DefenseWeightAddition,
                                              },
                                               new StatCheckTree // Section I
                                               {
                                                IsPlayer = _amIPlayerLeft,
                                                Keyword = _brain.SectionI.Keyword.Keyword,
                                                Operator = _brain.SectionI.Values.MathOperation,
                                                Amount= (int)_brain.SectionI.Values.Amount,
                                                AttackWeight =  _brain.SectionI.Keyword.Weights.AttackWeightAddition,
                                                UtilityWeight = _brain.SectionI.Keyword.Weights.UtilityWeightAddition,
                                                DefenseWeight = _brain.SectionI.Keyword.Weights.DefenseWeightAddition,
                                               },
                                                new TryPlayCardTree // Section J
                                                {
                                                    IsPlayer = _amIPlayerLeft,
                                                    AttackWeight  =  _brain.SectionJ.AttackWeightAddition,
                                                    UtilityWeight = _brain.SectionJ.UtilityWeightAddition,
                                                    DefenseWeight = _brain.SectionJ.DefenseWeightAddition,
                                                }
#endregion
                                          }
                                     }
                                 }
                             },
                                
#region Sections K,L,M
                              new CharacterHealthTree // Section K:
                              {
                                   IsPlayer = !_amIPlayerLeft,
                                   Operator = _brain.SectionK.HealthParams.MathOperation,
                                   Precentage = _brain.SectionK.HealthParams.Amount,
                                   AttackWeight =  _brain.SectionK.Weights.AttackWeightAddition,
                                   UtilityWeight = _brain.SectionK.Weights.UtilityWeightAddition,
                                   DefenseWeight = _brain.SectionK.Weights.DefenseWeightAddition,
                              },
                              new StatCheckTree // Section L:
                              {
                                   IsPlayer = _amIPlayerLeft,
                                   Keyword = _brain.SectionL.Keyword.Keyword,
                                   Operator = _brain.SectionL.Values.MathOperation,
                                   Amount= (int)_brain.SectionL.Values.Amount,
                                   AttackWeight =  _brain.SectionL.Keyword.Weights.AttackWeightAddition,
                                   UtilityWeight = _brain.SectionL.Keyword.Weights.UtilityWeightAddition,
                                   DefenseWeight = _brain.SectionL.Keyword.Weights.DefenseWeightAddition,
                              },
                         new TryPlayCardTree // Section M:
                         {
                              IsPlayer = _amIPlayerLeft,
                              AttackWeight =  _brain.SectionM.AttackWeightAddition,
                              UtilityWeight = _brain.SectionM.UtilityWeightAddition,
                              DefenseWeight = _brain.SectionM.DefenseWeightAddition,
                         }
#endregion
                        }
                     }
                 }
               }
            };
        }
    }
}
