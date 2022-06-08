using System;
using System.Collections.Generic;
using Battles.UI;
using UnityEngine;
using Object = System.Object;

[Serializable]
public class PoolObject<T> : MonoBehaviour, IPoolObject<T> where T : MonoBehaviour, IPoolable<T>
{
    [SerializeField]
    protected T _prefabOfType;

    private Stack<T> _poolToType = new Stack<T>();

    private List<T> _totalPoolType = new List<T>();
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
        T cache = MonoBehaviour.Instantiate(_prefabOfType);
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

    ~PoolObject()
    {
        for (int i = 0; i < _totalPoolType.Count; i++)
            _totalPoolType[i].OnDisposed -= AddToQueue;
    }
}


public interface IPoolObject<T> where T : MonoBehaviour, IPoolable<T>
{
    T Pull();
    void ResetPool();
}

public interface IPoolable<T> : IDisposable where T : MonoBehaviour
{
    event Action<T> OnDisposed;
    void Init();
}
