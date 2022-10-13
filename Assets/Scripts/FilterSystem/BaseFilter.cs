using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFilter<T> : ScriptableObject
{
    public abstract bool Filter(T obj);
}
