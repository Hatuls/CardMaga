using CardMaga.MetaData.AccoutData;
using CardMaga.ValidatorSystem.ValidatorConditions.MetaDeckData;

namespace CardMaga.ValidatorSystem.ValidationConditionGroup
{
    public class TestGroup : BaseValidationConditionGroup<MetaDeckData>
    {
        private BaseValidatorCondition<MetaDeckData>[] _conditions = {
            new IsDeckFull()
        };
        
        public override ValidationTag ValidationTag => default;

        public override BaseValidatorCondition<MetaDeckData>[] ValidatorConditions => _conditions;
    }
}