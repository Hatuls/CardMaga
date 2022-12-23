using System;

namespace CardMaga.ValidatorSystem
{
    [Serializable]
    public abstract class BaseTypeValidator<T>
    {
        public abstract BaseValidatorCondition<T>[] ValidatorCondition { get; }
    }
}