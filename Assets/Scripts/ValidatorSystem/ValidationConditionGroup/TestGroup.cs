using CardMaga.MetaData.AccoutData;
using CardMaga.ValidatorSystem.ValidatorConditions.MetaDeckData;

namespace CardMaga.ValidatorSystem.ValidationConditionGroup
{
    public class TestGroup : ValidationConditionGroup<MetaDeckData>
    {
        private BaseValidatorCondition<MetaDeckData>[] _conditions = new[]
        {
            new IsFull()
        };
        
        public override ValidationTag ValidationTag => ValidationTag.metaDeckDataDefualt;

        public override BaseValidatorCondition<MetaDeckData>[] ValidatorConditions => _conditions;
    }
}