using Account.GeneralData;
namespace CardMaga.ValidatorSystem
{
    public class CharactersValidation : BaseValidatorCondition<CharactersData>
    {
        public CharactersValidation(ValidationLevel level) : base(level)
        {
        }

        public override int ID => 103;

        public override string Message => "One Of your character failed";

        public override bool Valid(CharactersData obj, out IValidFailedInfo validFailedInfo, params ValidationTag[] validationTag)
        {
            bool result = true ;
            validFailedInfo = this;

            foreach (var character in obj.Characters)
            {
                result &= Validator.Valid(character, out validFailedInfo, ValidationTag.Default);
                if (!result)
                    break;
            }
            
            return result;
        }
    }

}