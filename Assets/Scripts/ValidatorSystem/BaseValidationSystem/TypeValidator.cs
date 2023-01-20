using System;
using System.Collections.Generic;

namespace CardMaga.ValidatorSystem
{
    public class TypeValidator<T> : IValid<T>
    {
        private Dictionary<ValidationTag, BaseValidatorCondition<T>[]> _validatorConditions;

        public TypeValidator()
        {
            var validationConditionGroups = Factory.GameFactory.Instance.ValidatorFactory.GetValidationConditionGroup<T>();
            
            _validatorConditions = new Dictionary<ValidationTag, BaseValidatorCondition<T>[]>(validationConditionGroups.Count);

            foreach (var validationGroup in validationConditionGroups)
            {
                _validatorConditions.Add(validationGroup.ValidationTag,validationGroup.ValidatorConditions);
            }
        }

        public bool Valid(T obj,out IValidFailedInfo validFailedInfo,params ValidationTag[] validationTag)
        {
            var validatorConditions = GetValidationCondition(validationTag);
            
            foreach (var condition in validatorConditions)
            {
                if (condition.Valid(obj, out IValidFailedInfo info,validationTag)) 
                    continue;
                
                validFailedInfo = info;
                return false;
            }
            
            validFailedInfo = null;
            return true;
        }
        
        public bool Valid(IEnumerable<T> objs,out IValidFailedInfo validFailedInfo,params ValidationTag[] validationTag)
        {
            var validatorConditions = GetValidationCondition(validationTag);

            foreach (var condition in validatorConditions)
            { 
                if (condition.Valid(objs, out IValidFailedInfo massage,validationTag)) 
                    continue;

                validFailedInfo = massage;
                return false;
            }
            
            validFailedInfo = null;
            return true;
        }

        private BaseValidatorCondition<T>[] GetValidationCondition(ValidationTag[] validationTag)
        {
            List<BaseValidatorCondition<T>> output = new List<BaseValidatorCondition<T>>();

            foreach (var tag in validationTag)
            {
                if (!_validatorConditions.TryGetValue(tag, out BaseValidatorCondition<T>[] value))
                    throw new Exception($"TypeValidator: a given validationTag tag: {tag} don't have a BaseValidatorCondition associated with it");

                foreach (var validatorCondition in value)
                {
                    output.Add(validatorCondition);
                }
            }

            if (output.Count == 0)
            {
                //throw new Exception($"TypeValidator: Didn't find any BaseValidatorCondition, the list count is 0");
            }

            return output.ToArray();
        }
    }
}