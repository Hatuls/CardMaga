using System.Collections.Generic;

namespace CardMaga.ValidatorSystem
{
    public interface IValid<in T>
    {
        bool Valid(T obj,out string failedMessage,params ValidationTag[] validationTag);
        bool Valid(IEnumerable<T> objs,out string failedMessage,params ValidationTag[] validationTag);
    }

    public interface IValid
    {
        bool Valid(out string failedMessage,params ValidationTag[] validationTag);
    }

    public interface IValidInfo
    {
        string Message { get; }
        int ID { get; }
    }
}