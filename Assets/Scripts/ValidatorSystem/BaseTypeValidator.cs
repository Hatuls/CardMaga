using System;

namespace CardMaga.ValidatorSystem
{
    [Serializable]
    public abstract class BaseTypeValidator<T>
    {
        public abstract BaseValidatorCondition<T>[] ValidatorCondition { get; }

        public bool Valid(T obj)
        {
            if (ValidatorCondition == null)
                return true;

            foreach (var condition in ValidatorCondition)
            {
                if (!condition.Valid(obj))
                    return false;
            }

            return true;
        }
    }
}