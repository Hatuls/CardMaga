using Account.GeneralData;
namespace CardMaga.ValidatorSystem
{
    public class ValidateMainCharacter : BaseValidatorCondition<CharactersData>
    {
        public ValidateMainCharacter(ValidationLevel level) : base(level)
        {
        }

        public override int ID =>101;

        public override string Message => "Main Character is not pointing to any character in the character data";

        public override bool Valid(CharactersData obj, out IValidFailedInfo validFailedInfo, params ValidationTag[] validationTag)
        {
            validFailedInfo = this;
            return Contain();

            bool Contain()
            {
                int mainCharacterID = obj.MainCharacterID;

                foreach (var character in obj.Characters)
                {
                    if (character.ID == mainCharacterID)
                        return true;
                }
                return false;
            }
        }
    }

}