using CardMaga.MetaData.AccoutData;
using CardMaga.ValidatorSystem;

namespace ValidatorSystem.ValidatorConditions.MetaCharecterData
{
    public class IsMainDeckValid : BaseValidatorCondition<MetaCharacterData>
    {
        public override int ID { get; }
        public override string Message => "Main deck is Invalid";
        public override bool Valid(MetaCharacterData obj, out IValidFailedInfo validFailedInfo, params ValidationTag[] validationTag)
        {
            MetaDeckData defaultDeck = obj.MainDeck;

            return Validator.Valid(defaultDeck, out validFailedInfo, ValidationTag.MetaDeckDataSystem);
        }
    }
}