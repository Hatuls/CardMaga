using System;

namespace CardMaga.ValidatorSystem
{
    public class ValidationException : Exception
    {
        public static event Action<IValidInfo> OnValidationFailed; 

        public ValidationException(IValidInfo validInfo): base($"Validation failed ID: {validInfo.ID}\nValidation message: {validInfo.Message}")
        {
            OnValidationFailed?.Invoke(validInfo);
        }
    }
}