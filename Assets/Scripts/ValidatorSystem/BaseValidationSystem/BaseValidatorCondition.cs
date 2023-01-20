using System;
using System.Collections.Generic;

namespace CardMaga.ValidatorSystem
{
    public abstract class BaseValidationConditionGroup<T>
    {
        public abstract ValidationTag ValidationTag { get; }

        public abstract BaseValidatorCondition<T>[] ValidatorConditions { get; }
    }


    public abstract class BaseValidatorCondition<T> : IValid<T> ,IValidInfo
    {
        public abstract int ID { get; }
        public abstract string Message { get; }

        public abstract bool Valid(T obj, out string failedMassage, params ValidationTag[] validationTag);

        public bool Valid(IEnumerable<T> objs, out string failedMessage,params ValidationTag[] validationTag)
        {
            foreach (var obj in objs)
            {
                if (Valid(obj,out failedMessage,validationTag))
                    continue;

                failedMessage = Message;
                return false;
            }
            
            failedMessage = String.Empty;
            return true;
        }
    }
}