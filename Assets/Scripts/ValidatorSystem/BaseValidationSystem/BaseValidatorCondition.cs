using System;
using System.Collections.Generic;

namespace CardMaga.ValidatorSystem
{
    public abstract class BaseValidationConditionGroup<T> 
    {
        public abstract ValidationTag ValidationTag { get; }
        public abstract BaseValidatorCondition<T>[] ValidatorConditions { get; }
    }


    public abstract class BaseValidatorCondition<T> : IValid<T> ,IValidFailedInfo
    {
        public abstract int ID { get; }
        public abstract string Message { get; }

        private ValidationLevel _level;
        public ValidationLevel Level => _level;


        public BaseValidatorCondition(ValidationLevel level)
        {
            _level = level;
        }

        public abstract bool Valid(T obj, out IValidFailedInfo validFailedInfo, params ValidationTag[] validationTag);

        public bool Valid(IEnumerable<T> objs, out IValidFailedInfo validFailedInfo,params ValidationTag[] validationTag)
        {
            foreach (var obj in objs)
            {
                if (Valid(obj,out validFailedInfo,validationTag))
                    continue;

                validFailedInfo = this;
                return false;
            }
            
            validFailedInfo = null;
            return true;
        }
    }
}