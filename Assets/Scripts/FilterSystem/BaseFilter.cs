using UnityEngine;

public abstract class BaseFilter<T> : ScriptableObject
{
    public abstract bool Filter(T obj);
}
