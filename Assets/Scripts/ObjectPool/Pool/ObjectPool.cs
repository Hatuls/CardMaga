using System;
using System.Collections.Generic;
using CardMaga.UI;
using UnityEngine;

namespace CardMaga.Tools.Pools
{
    public class MBPool<T> : IPoolMBObject<T> where T : MonoBehaviour, IPoolableMB<T>, new()
    {
        private readonly Stack<T> _poolToType = new Stack<T>();

        private readonly List<T> _totalPoolType = new List<T>();
        public Type GetPoolableType => typeof(T);

        private T _prefabOfType;
        private Transform _parent;
        
        public MBPool(T prefabOfType, int startSize, Transform parent = null) : this(prefabOfType, parent)
        {
            InitSize(startSize);
        }
        
        public MBPool(T prefabOfType, Transform parent = null)
        {
            _parent = parent;
            _prefabOfType = prefabOfType;
        }

        private void InitSize(int amount)
        {
            for (int i = 0; i < amount; i++)
                Pull();

            ResetPool();
        }
        
        public T Pull()
        {
            T cache = null;

            if (_poolToType.Count > 0)
                cache = _poolToType.Pop();
            else
                cache = GenerateNewOfType();
            
            cache.transform.SetParent(_parent);

            return cache;
        }
        
        public T Pull(Transform parent)
        {
            T cache = null;

            if (_poolToType.Count > 0)
                cache = _poolToType.Pop();
            else
                cache = GenerateNewOfType();
            
            cache.transform.SetParent(parent);
            
            return cache;
        }

        private void AddToQueue(T type)
        {
            _poolToType.Push(type);
            type.gameObject.SetActive(false);
        }

        private T GenerateNewOfType()
        {
            T cache = MonoBehaviour.Instantiate(_prefabOfType);

            if (cache == null)
            {
                Debug.LogError($"Failed to Pull object, { typeof(T) } is null");
                return null;
            }

            cache.OnDisposed += AddToQueue;
            _totalPoolType.Add(cache);
            return cache;
        }
        
        public void ResetPool()
        {
            for (int i = 0; i < _totalPoolType.Count; i++)
                _totalPoolType[i].Dispose();
        }
    }
    
    public class ObjectPool<T> : IPoolObject<T> where T : class, IPoolable<T>, new()
    {
        private Stack<T> _poolToType = new Stack<T>();

        private List<T> _totalPoolType = new List<T>();
        public Type GetPoolableType => typeof(T);
        
        public ObjectPool()
        {
        }

        public ObjectPool(int amount)
        {
            for (int i = 0; i < amount; i++)
                Pull();

            ResetPool();
        }

        public T Pull()
        {
            T cache = null;

            if (_poolToType.Count > 0)
                cache = _poolToType.Pop();
            else
                cache = GenerateNewOfType();

            return cache;
        }

        private T GenerateNewOfType()
        {
            T cache = new T();

            if (cache == null)
            {
                Debug.LogError($"Failed to Pull object, { typeof(T) } is null");
                return null;
            }

            cache.OnDisposed += AddToQueue;
            _totalPoolType.Add(cache);
            return cache;
        }

        private void AddToQueue(T type)
        {
            _poolToType.Push(type);
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
        Type GetPoolableType { get; }
        void ResetPool();
    }

    public interface IPoolMBObject<T> : IPoolObject<T> where T : MonoBehaviour, IPoolableMB<T>
    {
        T Pull(Transform parent);
    }

    public interface IPoolable<T> : IDisposable
    {
        event Action<T> OnDisposed;
    }
    
    public interface IPoolableMB<T> : IPoolable<T>,IInitializable where T : MonoBehaviour
    {
    }
}