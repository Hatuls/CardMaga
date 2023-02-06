using CardMaga.MetaData.AccoutData;
using CardMaga.ValidatorSystem.ValidatorConditions.MetaDeckData;

namespace CardMaga.ValidatorSystem.ValidationConditionGroup
{
    public class SystemMetaDeckDataValidGroup : BaseValidationConditionGroup<MetaDeckData>
    {
        public override ValidationTag ValidationTag => ValidationTag.MetaDeckDataSystem;

        public override BaseValidatorCondition<MetaDeckData>[] ValidatorConditions { get; } =
        {
            new IsDeckFull(ValidationLevel.GameDesign)
        };
    }


}