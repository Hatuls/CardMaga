using Account.GeneralData;
using System.Linq;
namespace CardMaga.ValidatorSystem
{
    public class ValidateCharactersCount : BaseValidatorCondition<CharactersData>
    {
        public ValidateCharactersCount(ValidationLevel level) : base(level)
        {
        }

        public override int ID =>100;

        public override string Message => "Data does not contain a valid characers amount, either is empty or not assigned!";

        public override bool Valid(CharactersData obj, out IValidFailedInfo validFailedInfo, params ValidationTag[] validationTag)
        {
                validFailedInfo = this;
            return obj.Characters != null && obj.Characters.Count > 0;
        }
    }

}