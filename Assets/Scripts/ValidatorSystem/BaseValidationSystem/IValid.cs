using System.Collections.Generic;

namespace CardMaga.ValidatorSystem
{
    public interface IValid<in T>
    {
        bool Valid(T obj,out IValidFailedInfo validFailedInfo,params ValidationTag[] validationTag);
        bool Valid(IEnumerable<T> objs,out IValidFailedInfo validFailedInfo,params ValidationTag[] validationTag);
    }

    public interface IValid
    {
        bool Valid(out IValidFailedInfo validFailedInfo,params ValidationTag[] validationTag);
    }

    public interface IValidFailedInfo
    {
        string Message { get; }
        int ID { get; }
    }
}