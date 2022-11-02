using UnityEngine;

public abstract class BaseFilter<T> : ScriptableObject , IFilter<T>
{
    public abstract bool Filter(T obj);
}

public interface IFilter<T>
{
    bool Filter(T obj);
}
