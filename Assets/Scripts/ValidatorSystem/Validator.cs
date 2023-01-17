using System;
using System.Collections.Generic;

namespace CardMaga.ValidatorSystem
{
    public class Validator
    {
        private static Validator _instance;

        public static Validator Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = new Validator();
                return _instance;
            }
        }

        private Dictionary<Type, object> _validators = new Dictionary<Type,object>();

        private Validator()
        {
           // GameFactory.Instance.ValidatorFactory.GetTypeValidator();
        }
        
        public bool Valid<T>(T obj,out string failedMassage,ValidationTag validationTag = default)
        {
            var validator = GetValidator<T>();

            if (!validator.Valid(obj,out string massage))
            {
                failedMassage = massage;
                return false;
            }

            failedMassage = string.Empty;
            return true;
        }

        internal void AddValidator(object validator,Type type)
        {
            _validators.Add(type,validator);
        }

        public void AddConditionGroup(object conditionGroup, Type type)
        {
            var validator = GetValidator<>()
        }

        private TypeValidator<T> GetValidator<T>()
        {
            if (_validators.TryGetValue(typeof(T), out var value))
                return value as TypeValidator<T>;

            throw new Exception($"Validator: Type validator of type {typeof(T)} is not found");
        }
        
        private TypeValidator<T> GetValidator(Type type)
        {
            if (_validators.TryGetValue(type, out var value))
                return value as TypeValidator<T>;

            throw new Exception($"Validator: Type validator of type {typeof(T)} is not found");
        }
    }
    
    public enum ValidationTag
    {
        metaDeckDataDefualt,
    }
}