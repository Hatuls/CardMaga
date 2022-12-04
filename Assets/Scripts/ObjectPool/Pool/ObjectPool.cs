using System;
using System.Collections.Generic;
using CardMaga.UI;
using UnityEngine;

namespace CardMaga.Tools.Pools
{
    public class MBPool<T> : ObjectPool<T>, IPoolMBObject<T> where T : MonoBehaviour, IPoolableMB<T>, new()
    {
        protected T _prefabOfType;
        private Transform _parent;
        
        public MBPool(T prefabOfType, Transform parent, int startSize) : this(prefabOfType, parent)
        {
            InitSize(startSize);
        }
        
        public MBPool(T prefabOfType, Transform parent)
        {
            _prefabOfType = prefabOfType;
            _parent = parent;
        }
      
        private void InitSize(int amount)
        {
            for (int i = 0; i < amount; i++)
                Pull();

            ResetPool();
        }
        
        public override T Pull(Predicate<T> condition = null)
        {
            T type = base.Pull(condition);
            type.Init();
            return type;
        }
        
        protected override void AddToQueue(T type)
        {
            base.AddToQueue(type);
            type.gameObject.SetActive(false);
        }

        protected override T GenerateNewOfType()
        {
            T cache = MonoBehaviour.Instantiate(_prefabOfType, _parent);

            if (cache == null)
            {
                Debug.LogError($"Failed to Pull object, { typeof(T) } is null");
                return null;
            }

            cache.OnDisposed += AddToQueue;
            _totalPoolType.Add(cache);
            return cache;
        }
    }
    
    public class ObjectPool<T> : IPoolObject<T> where T : class, IPoolable<T>, new()
    {
        protected List<T> _reservesOfType = new List<T>();

        protected List<T> _totalPoolType = new List<T>();
        
        public ObjectPool()
        {
        }

        public ObjectPool(int amount)
        {
            for (int i = 0; i < amount; i++)
                Pull();

            ResetPool();
        }
        
        public virtual T Pull(Predicate<T> condition = null)
        {
            T cache = null;
            int count = _reservesOfType.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if (condition == null || condition.Invoke(_reservesOfType[i]))
                    {
                        cache = _reservesOfType[i];
                        _reservesOfType.RemoveAt(i);
                        break;
                    }
                }
            }

            if(cache == null)
                cache = GenerateNewOfType();

            return cache;
        }
        
        protected virtual T GenerateNewOfType()
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

        protected virtual void AddToQueue(T type)
        {
            _reservesOfType.Add(type);
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


        T Pull(Predicate<T> condition=null);
        void ResetPool();
    }

    public interface IPoolMBObject<T> : IPoolObject<T> where T : MonoBehaviour, IPoolableMB<T>
    {
    }

    public interface IPoolable<T> : IDisposable
    {
        event Action<T> OnDisposed;
    }
    
    public interface IPoolableMB<T> : IPoolable<T>,IInitializable where T : MonoBehaviour
    {
    }
}