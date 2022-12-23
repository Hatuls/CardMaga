namespace CardMaga.ValidatorSystem
{
    public interface IValid<T>
    {
        bool Valid(T obj);
    }
}