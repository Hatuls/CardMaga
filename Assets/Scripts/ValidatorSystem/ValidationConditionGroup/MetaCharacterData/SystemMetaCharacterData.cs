using CardMaga.ValidatorSystem;

namespace ValidatorSystem.ValidationConditionGroup.MetaCharacterData
{
    public class SystemMetaCharacterData : BaseValidationConditionGroup<CardMaga.MetaData.AccoutData.MetaCharacterData>
    {
        public override ValidationTag ValidationTag => ValidationTag.MetaCharacterDataSystem;

        public override BaseValidatorCondition<CardMaga.MetaData.AccoutData.MetaCharacterData>[] ValidatorConditions { get; } =
        {
            
        };
    }
}