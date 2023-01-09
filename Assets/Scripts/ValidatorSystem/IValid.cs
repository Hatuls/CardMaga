namespace CardMaga.ValidatorSystem
{
    public interface IValid<in T>
    {
        bool Valid(T obj,out string failedMassage);
    }
}