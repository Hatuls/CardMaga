using System;

namespace CardMaga.ValidatorSystem
{
    public class ValidationException : Exception
    {
        public static event Action<IValidFailedInfo> OnValidationFailed; 

        public ValidationException(IValidFailedInfo validFailedInfo): base($"Validation failed ID: {validFailedInfo.ID}\nValidation message: {validFailedInfo.Message}")
        {
            OnValidationFailed?.Invoke(validFailedInfo);
        }
    }
}