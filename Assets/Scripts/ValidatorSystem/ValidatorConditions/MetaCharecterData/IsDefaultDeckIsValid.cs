using CardMaga.MetaData.AccoutData;
using CardMaga.ValidatorSystem;

namespace ValidatorSystem.ValidatorConditions.MetaCharecterData
{
    public class IsDefaultDeckIsValid : BaseValidatorCondition<MetaCharacterData>
    {
        public override int ID { get; }
        public override string Message { get; }

        public override bool Valid(MetaCharacterData obj, out IValidFailedInfo validFailedInfo, params ValidationTag[] validationTag)
        {
            MetaDeckData defaultDeck = obj.Decks[0];

            return Validator.Valid(defaultDeck, out validFailedInfo, validationTag);
        }
    }
}