using Account.GeneralData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMaga.ValidatorSystem
{
    class SystemCharacterValidationGroup : BaseValidationConditionGroup<CharactersData>
    {
        public override ValidationTag ValidationTag => ValidationTag.SystemCharactersData;

        public override BaseValidatorCondition<CharactersData>[] ValidatorConditions => new BaseValidatorCondition<CharactersData>[]
        {
            new ValidateCharactersCount(ValidationLevel.Warning),
            new ValidateMainCharacter(ValidationLevel.Warning),
            new CharactersValidation(ValidationLevel.Warning)
        };
    }
}
