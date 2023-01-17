using System;
using System.Collections.Generic;

namespace CardMaga.ValidatorSystem
{
    public class TypeValidator<T> : IValid<T>
    {
        private Dictionary<ValidationTag, BaseValidatorCondition<T>[]> _validatorConditions;

        private int _id;
        public int ID => _id;

        public TypeValidator()
        {
            _validatorConditions = new Dictionary<ValidationTag, BaseValidatorCondition<T>[]>();
        }

        public bool Valid(T obj,out string failedMassage,ValidationTag validationTag = default)
        {
            if (!GetValidationCondition(validationTag,out var validatorConditions))
            {
                //didn't find any validatorConditions
            }
            
            if (validatorConditions.Length == 0)//need to remove
            {
                failedMassage = String.Empty;
                return true;//return true only if the validator in implement but have no validation in the default validatorConditions
            }

            foreach (var condition in validatorConditions)
            {
                if (condition.Valid(obj, out string massage)) 
                    continue;
                
                failedMassage = massage;
                return false;
            }
            
            failedMassage = String.Empty;
            return true;
        }

        private bool GetValidationCondition(ValidationTag validationTag,out BaseValidatorCondition<T>[] conditions)
        {
            if (_validatorConditions.TryGetValue(validationTag,out BaseValidatorCondition<T>[] value))
            {
                conditions = value;
                return true;
            }

            conditions = null;
            return false;
        }
    }
}