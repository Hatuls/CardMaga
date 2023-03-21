using CardMaga.MetaData.AccoutData;

namespace CardMaga.ValidatorSystem.ValidationConditionGroup
{
    public class GameDesignMetaDeckDataValidGroup : BaseValidationConditionGroup<MetaDeckData>
    {
        public override ValidationTag ValidationTag => ValidationTag.MetaDeckDataGameDesign;

        public override BaseValidatorCondition<MetaDeckData>[] ValidatorConditions { get; } =
        {
            
        };
    }
}