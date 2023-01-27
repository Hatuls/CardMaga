
using Account.GeneralData;
using System;
namespace CardMaga.ValidatorSystem
{

    public class MatchMakingValidation : BaseValidatorTerminal
    {
        protected override Type[] TypeValidator =>
            new Type[]
            {
            typeof(TypeValidator<CharactersData>),
            typeof(TypeValidator<Character>),
            typeof(TypeValidator<DeckData>)
            };
    }
}