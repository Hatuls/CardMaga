

namespace CardMaga.ValidatorSystem
{
    public abstract class BaseValidatorCondition<T> : IValid<T>
    {
        public abstract string FailedMassage { get; }
        
        public abstract bool Valid(T obj, out string failedMassage);
    }
}