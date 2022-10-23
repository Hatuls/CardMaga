using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> :  IPoolObject<T> where T : MonoBehaviour, IPoolable<T>
{
    protected T _prefabOfType;

    private Stack<T> _poolToType = new Stack<T>();

    private List<T> _totalPoolType = new List<T>();
    
    private Transform _parent;
    
    public ObjectPool(T objectToPool,Transform parent)
    {
        _parent = parent;
        _prefabOfType = objectToPool;
    }

    public T Pull()
    {
        T cache = null;

        if (_poolToType.Count > 0)
            cache = _poolToType.Pop();
        else
            cache = GenerateNewOfType();

        cache.Init();

        return cache;
    }

    private T GenerateNewOfType()
    {
        T cache = MonoBehaviour.Instantiate(_prefabOfType, _parent);

        if (cache == null)
        {
            Debug.LogError($"Failed to Pull object, { typeof(T) } is null");
            return null;
        }
        
        cache.OnDisposed += AddToQueue;
        _totalPoolType.Add(_prefabOfType);
        return cache;
    }

    private void AddToQueue(T type)
    {
        _poolToType.Push(type);
        type.gameObject.SetActive(false);
    }
    public void ResetPool()
    {
        for (int i = 0; i < _totalPoolType.Count; i++)
            _totalPoolType[i].Dispose();
    }

    ~ObjectPool()
    {
        for (int i = 0; i < _totalPoolType.Count; i++)
            _totalPoolType[i].OnDisposed -= AddToQueue;
    }
}


public interface IPoolObject<T> where T : IPoolable<T>
{
    T Pull();
    void ResetPool();
}

public interface IPoolMBObject<T> where T :MonoBehaviour, IPoolableMB<T>
{

}

public interface IPoolable<T> : IDisposable
{
    event Action<T> OnDisposed;
    void Init();
}


public interface IPoolableMB<T> : IPoolable<T> where T: MonoBehaviour
{

}
public interface IPoolableClass<T> : IDisposable where T : new()
{
    event Action<T> OnDisposed;
}

public class PoolClass<T>
{

    private Stack<T> _stackPool;
    public PoolClass()
    {
        _stackPool = new Stack<T>();


    }
}