namespace CardMaga.ValidatorSystem
{
    public interface IValid<in T> // need to check rei
    {
        bool Valid(T obj,out string failedMassage);
    }
}