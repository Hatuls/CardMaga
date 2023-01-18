using CardMaga.MetaData.AccoutData;
using CardMaga.ValidatorSystem;

namespace ValidatorSystem.StaticTypeValidators
{
    public class DefaultDeckValidator : IValid
    {
        private MetaDeckData _defaultDeck;


        public bool Valid(out string failedMessage, params ValidationTag[] validationTag)
        {
            Validator.Instance.Valid(_defaultDeck, out var failedMessage, validationTag);
        }
    }
}