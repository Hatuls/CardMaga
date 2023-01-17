

namespace CardMaga.ValidatorSystem
{
    public abstract class ValidationConditionGroup<T>
    {
        public abstract ValidationTag ValidationTag { get; }

        public abstract BaseValidatorCondition<T>[] ValidatorConditions { get; }
    }


    public abstract class BaseValidatorCondition<T> : IValid<T>
    {
        public abstract string FailedMassage { get; }
        
        public abstract bool Valid(T obj, out string failedMassage,ValidationTag validationTag = default);

        protected BaseValidatorCondition()
        {
            
        }
    }

    // public struct ValidationTag
    // {
    //     private static int _uniqueID = 0;
    //     private static int UniqueID => _uniqueID++;
    //
    //     private static readonly int _tagId = UniqueID;
    //     public int TagId => _tagId;
    //     
    //     public override int GetHashCode()
    //     {
    //         return _tagId;
    //     }
    // }
}