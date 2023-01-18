using System;
using System.Collections.Generic;
using Factory;

namespace CardMaga.ValidatorSystem
{
    public class Validator
    {
        private Dictionary<Type, object> _validators = new Dictionary<Type,object>();
        
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
        
        private Validator()
        { 
            GameFactory.Instance.ValidatorFactory.GetTypeValidator();
        }
        
        public bool Valid<T>(T obj,out string failedMassage,ValidationTag validationTag)
        {
            var validator = GetValidator<T>();

            if (!validator.Valid(obj,out string massage,validationTag))
            {
                failedMassage = massage;
                return false;
            }

            failedMassage = string.Empty;
            return true;
        }
        
        public bool Valid<T>(IEnumerable<T> objs,out string failedMassage,ValidationTag validationTag)
        {
            var validator = GetValidator<T>();
            
            if (!validator.Valid(objs,out string massage,validationTag))
            {
                failedMassage = massage;
                return false;
            }

            failedMassage = string.Empty;
            return true;
        }

        internal void AddValidator(object validator,Type type)
        {
            if (_validators.ContainsKey(type))
                return;
            
            _validators.Add(type,validator);
        }

        private IValid<T> GetValidator<T>()
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
    }
}