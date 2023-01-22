using CardMaga.ValidatorSystem;

namespace ValidatorSystem.ValidationConditionGroup.CardInstance
{
    public class SystemCardInstance : BaseValidationConditionGroup<Account.GeneralData.CardInstance>
    {
        public override ValidationTag ValidationTag => ValidationTag.SystemCardInstance;

        public override BaseValidatorCondition<Account.GeneralData.CardInstance>[] ValidatorConditions { get; } =
        {
            
        };
    }
}