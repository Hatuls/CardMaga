using System;
using System.Collections.Generic;

namespace CardMaga.ValidatorSystem
{
    [Serializable]
    public class TypeValidator<T>
    {
        private List<BaseValidatorCondition<T>> _validatorConditions;

        public TypeValidator()
        {
            _validatorConditions = new List<BaseValidatorCondition<T>>();
        }
        
        public TypeValidator(List<BaseValidatorCondition<T>> validatorConditions)
        {
            _validatorConditions = validatorConditions;
        }

        public bool Valid(T obj,out string failedMassage)
        {
            if (_validatorConditions == null)
            {
                failedMassage = String.Empty;
                return true;
            }

            foreach (var condition in _validatorConditions)
            {
                if (condition.Valid(obj, out string Massage)) 
                    continue;
                
                failedMassage = Massage;
                return false;
            }
            
            failedMassage = String.Empty;
            return true;
        }
    }
}