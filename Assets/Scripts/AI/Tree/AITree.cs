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
                                                       KeywordType= Keywords.KeywordType.Bleed,
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
                                                                           KeywordA = Keywords.KeywordType.Bleed,
                                                                           KeywordB = Keywords.KeywordType.Heal,
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
                                                                                   AttackComboWeight =  _brain.SectionA.Weights.Combos.AttackWeightAddition,
                                                                                   UtilityComboWeight = _brain.SectionA.Weights.Combos.UtilityWeightAddition,
                                                                                   DefenseComboWeight = _brain.SectionA.Weights.Combos.DefenseWeightAddition,

                                                                                   AttackWeight =  _brain.SectionA.Weights.Cards.AttackWeightAddition,
                                                                                   UtilityWeight = _brain.SectionA.Weights.Cards.UtilityWeightAddition,
                                                                                   DefenseWeight = _brain.SectionA.Weights.Cards.DefenseWeightAddition,
                                                                              },
                                                                              new CardDoKeywordTree // section B
                                                                              {
                                                                                   IsPlayer = _amIPlayerLeft,
                                                                                   Keyword = _brain.SectionB.Keyword,
                                                                                   AttackComboWeight =  _brain.SectionB.Weights.Combos.AttackWeightAddition,
                                                                                   UtilityComboWeight = _brain.SectionB.Weights.Combos.UtilityWeightAddition,
                                                                                   DefenseComboWeight = _brain.SectionB.Weights.Combos.DefenseWeightAddition,
                                                                                   AttackWeight =  _brain.SectionB.Weights.Cards.AttackWeightAddition,
                                                                                   UtilityWeight = _brain.SectionB.Weights.Cards.UtilityWeightAddition,
                                                                                   DefenseWeight = _brain.SectionB.Weights.Cards.DefenseWeightAddition,
                                                                              },
                                                                              new WillFinishStaminaTree // section C
                                                                              {
                                                                                   IsPlayer = _amIPlayerLeft,
                                                                                   AttackComboWeight =  _brain.SectionC.Combos.AttackWeightAddition,
                                                                                   UtilityComboWeight = _brain.SectionC.Combos.UtilityWeightAddition,
                                                                                   DefenseComboWeight = _brain.SectionC.Combos.DefenseWeightAddition,
                                                                                   AttackWeight =  _brain.SectionC.Cards.AttackWeightAddition,
                                                                                   UtilityWeight = _brain.SectionC.Cards.UtilityWeightAddition,
                                                                                   DefenseWeight = _brain.SectionC.Cards.DefenseWeightAddition,
                                                                              },
                                                                              new TryPlayCardTree // Section D
                                                                              {
                                                                                   IsPlayer = _amIPlayerLeft,
                                                                                   AttackComboWeight  =  _brain.SectionD.Combos.AttackWeightAddition,
                                                                                   UtilityComboWeight = _brain.SectionD.Combos.UtilityWeightAddition,
                                                                                   DefenseComboWeight = _brain.SectionD.Combos.DefenseWeightAddition,
                                                                                   AttackWeight       =  _brain.SectionD.Cards.AttackWeightAddition,
                                                                                   UtilityWeight      = _brain.SectionD.Cards.UtilityWeightAddition,
                                                                                   DefenseWeight      = _brain.SectionD.Cards.DefenseWeightAddition,
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
                                                                  AttackComboWeight  =  _brain.SectionE.Weights.Combos.AttackWeightAddition,
                                                                  UtilityComboWeight = _brain.SectionE.Weights.Combos.UtilityWeightAddition,
                                                                  DefenseComboWeight = _brain.SectionE.Weights.Combos.DefenseWeightAddition,
                                                                  AttackWeight       =  _brain.SectionE.Weights.Cards.AttackWeightAddition,
                                                                  UtilityWeight      = _brain.SectionE.Weights.Cards.UtilityWeightAddition,
                                                                  DefenseWeight      = _brain.SectionE.Weights.Cards.DefenseWeightAddition,
                                                            },
                                                            new StatCheckTree // Section F
                                                            {
                                                               IsPlayer = _amIPlayerLeft,
                                                               Keyword = _brain.SectionF.Keyword.Keyword,
                                                               Operator = _brain.SectionF.Values.MathOperation,
                                                               Amount= (int)_brain.SectionF.Values.Amount,
                                                               AttackComboWeight =  _brain.SectionF.Keyword.Weights.Combos.AttackWeightAddition,
                                                               UtilityComboWeight = _brain.SectionF.Keyword.Weights.Combos.UtilityWeightAddition,
                                                               DefenseComboWeight = _brain.SectionF.Keyword.Weights.Combos.DefenseWeightAddition,
                                                               AttackWeight       =  _brain.SectionF.Keyword.Weights.Cards.AttackWeightAddition,
                                                               UtilityWeight      = _brain.SectionF.Keyword.Weights.Cards.UtilityWeightAddition,
                                                               DefenseWeight      = _brain.SectionF.Keyword.Weights.Cards.DefenseWeightAddition,
                                                            },
                                                            new TryPlayCardTree // Section G
                                                            {
                                                                IsPlayer = _amIPlayerLeft,
                                                                AttackComboWeight =  _brain.SectionG.Combos.AttackWeightAddition,
                                                                UtilityComboWeight = _brain.SectionG.Combos.UtilityWeightAddition,
                                                                DefenseComboWeight = _brain.SectionG.Combos.DefenseWeightAddition,
                                                                AttackWeight       =  _brain.SectionG.Cards.AttackWeightAddition,
                                                                UtilityWeight      = _brain.SectionG.Cards.UtilityWeightAddition,
                                                                DefenseWeight      = _brain.SectionG.Cards.DefenseWeightAddition,
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
                                                    AttackComboWeight = _brain.SectionH.Weights.Combos.AttackWeightAddition,
                                                    UtilityComboWeight = _brain.SectionH.Weights.Combos.UtilityWeightAddition,
                                                    DefenseComboWeight = _brain.SectionH.Weights.Combos.DefenseWeightAddition,
                                                    AttackWeight       =  _brain.SectionH.Weights.Cards.AttackWeightAddition,
                                                    UtilityWeight      = _brain.SectionH.Weights.Cards.UtilityWeightAddition,
                                                    DefenseWeight      = _brain.SectionH.Weights.Cards.DefenseWeightAddition,
                                              },
                                               new StatCheckTree // Section I
                                               {
                                                IsPlayer = _amIPlayerLeft,
                                                Keyword = _brain.SectionI.Keyword.Keyword,
                                                Operator = _brain.SectionI.Values.MathOperation,
                                                Amount= (int)_brain.SectionI.Values.Amount,
                                                AttackComboWeight =  _brain.SectionI.Keyword.Weights.Combos.AttackWeightAddition,
                                                UtilityComboWeight = _brain.SectionI.Keyword.Weights.Combos.UtilityWeightAddition,
                                                DefenseComboWeight = _brain.SectionI.Keyword.Weights.Combos.DefenseWeightAddition,
                                                AttackWeight       =  _brain.SectionI.Keyword.Weights.Cards.AttackWeightAddition,
                                                UtilityWeight      = _brain.SectionI.Keyword.Weights.Cards.UtilityWeightAddition,
                                                DefenseWeight      = _brain.SectionI.Keyword.Weights.Cards.DefenseWeightAddition,
                                               },
                                                new TryPlayCardTree // Section J
                                                {
                                                    IsPlayer = _amIPlayerLeft,
                                                   AttackComboWeight =  _brain.SectionJ.Combos.AttackWeightAddition,
                                                   UtilityComboWeight = _brain.SectionJ.Combos.UtilityWeightAddition,
                                                   DefenseComboWeight = _brain.SectionJ.Combos.DefenseWeightAddition,
                                                   AttackWeight =  _brain.SectionJ.Cards.AttackWeightAddition,
                                                   UtilityWeight = _brain.SectionJ.Cards.UtilityWeightAddition,
                                                   DefenseWeight = _brain.SectionJ.Cards.DefenseWeightAddition,
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
                                   AttackComboWeight =  _brain.SectionK.Weights.Combos.AttackWeightAddition,
                                   UtilityComboWeight = _brain.SectionK.Weights.Combos.UtilityWeightAddition,
                                   DefenseComboWeight = _brain.SectionK.Weights.Combos.DefenseWeightAddition,
                                   AttackWeight       = _brain.SectionK.Weights.Cards.AttackWeightAddition,
                                   UtilityWeight      = _brain.SectionK.Weights.Cards.UtilityWeightAddition,
                                   DefenseWeight      = _brain.SectionK.Weights.Cards.DefenseWeightAddition,
                              },
                              new StatCheckTree // Section L:
                              {
                                   IsPlayer = _amIPlayerLeft,
                                   Keyword = _brain.SectionL.Keyword.Keyword,
                                   Operator = _brain.SectionL.Values.MathOperation,
                                   Amount= (int)_brain.SectionL.Values.Amount,
                                   AttackComboWeight =  _brain.SectionL.Keyword.Weights.Combos.AttackWeightAddition,
                                   UtilityComboWeight = _brain.SectionL.Keyword.Weights.Combos.UtilityWeightAddition,
                                   DefenseComboWeight = _brain.SectionL.Keyword.Weights.Combos.DefenseWeightAddition,
                                   AttackWeight       = _brain.SectionL.Keyword.Weights.Cards.AttackWeightAddition,
                                   UtilityWeight      = _brain.SectionL.Keyword.Weights.Cards.UtilityWeightAddition,
                                   DefenseWeight      = _brain.SectionL.Keyword.Weights.Cards.DefenseWeightAddition,
                              },
                         new TryPlayCardTree // Section M:
                         {
                              IsPlayer = _amIPlayerLeft,
                              AttackComboWeight =  _brain.SectionM.Combos.AttackWeightAddition,
                              UtilityComboWeight = _brain.SectionM.Combos.UtilityWeightAddition,
                              DefenseComboWeight = _brain.SectionM.Combos.DefenseWeightAddition,
                              AttackWeight =  _brain.SectionM.Cards.AttackWeightAddition,
                              UtilityWeight = _brain.SectionM.Cards.UtilityWeightAddition,
                              DefenseWeight = _brain.SectionM.Cards.DefenseWeightAddition,
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
