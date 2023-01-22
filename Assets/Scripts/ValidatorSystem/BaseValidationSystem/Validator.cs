using System;
using System.Collections.Generic;

namespace CardMaga.ValidatorSystem
{
    public static class Validator
    {
        private static Dictionary<Type, object> _validators = new Dictionary<Type,object>();
        
        public static bool Valid<T>(T obj,out IValidFailedInfo validFailedInfo,params ValidationTag[] validationTag)
        {
            var validator = GetValidator<T>();

            if (!validator.Valid(obj,out IValidFailedInfo massage,validationTag))
            {
                validFailedInfo = massage;
                return false;
            }

            validFailedInfo = null;
            return true;
        }
        
        public static bool Valid<T>(IEnumerable<T> objs,out IValidFailedInfo validFailedInfo,params ValidationTag[] validationTag)
        {
            var validator = GetValidator<T>();
            
            if (!validator.Valid(objs,out IValidFailedInfo info,validationTag))
            {
                validFailedInfo = info;
                return false;
            }

            validFailedInfo = null;
            return true;
        }

        internal static void AddValidator(object validator,Type type)
        {
            if (_validators.ContainsKey(type))
                return;
            
            _validators.Add(type,validator);
        }

        public static void ResetValidator()
        {
            _validators.Clear();
        }

        private static IValid<T> GetValidator<T>()
        {
            if (_validators.TryGetValue(typeof(T), out var value))
                return value as IValid<T>;

            throw new Exception($"Validator: Type validator of type {typeof(T)} is not found");
        }
    }
    
    public enum ValidationTag
    {
        Default,
        MetaDeckDataDefault,
        MetaDeckDataGameDesign,
        MetaDeckDataSystem,
        MetaCharacterDataSystem,
        SystemCardInstance,
    }
}