namespace CardMaga.ValidatorSystem
{
    public interface IValid<in T>
    {
        bool Valid(T obj,out string failedMassage,ValidationTag validationTag = default);
    }
    
    public interface IValid
    {
        bool Valid(out string failedMassage,ValidationTag validationTag = default);
    }
}