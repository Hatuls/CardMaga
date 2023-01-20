using System;
using Factory;

namespace CardMaga.ValidatorSystem
{
    public abstract class BaseValidatorTerminal
    {
        protected abstract Type[] TypeValidator { get; }
        
        protected BaseValidatorTerminal()
        {
            GameFactory.Instance.ValidatorFactory.GetTypeValidator(TypeValidator);
        }
    }
}