
using CardMaga.Battle.Players;
using CardMaga.Tools.Pools;
using System;
using System.Collections.Generic;
using UnityEngine;
using static CardMaga.Battle.Players.TagHelper;
namespace CardMaga.ObjectPool
{
    public interface IPool<TIdSO, TObject> where TIdSO : BasePoolSO<TObject> where TObject : MonoBehaviour, ITaggable, IPoolableMB<TObject>
    {
        TObject Pull(TIdSO poolSO);
    }

    public class PoolHandler<TIdSO, TObject> : IDisposable, IPool<TIdSO, TObject> where TIdSO : BasePoolSO<TObject> where TObject : MonoBehaviour, IPoolableMB<TObject>, ITaggable
    {

        private readonly List<TObject> _allPoolObjects;
        private readonly List<TObject> _reservedList;

        private Transform _parent;

        public PoolHandler(Transform defaultParent)
        {
            _parent = defaultParent;
            _allPoolObjects = new List<TObject>();
            _reservedList = new List<TObject>();
        }

        public void PopulatePool(TIdSO basePoolSO, int amount)
        {
            for (int i = 0; i < amount; i++)
                InstantiateObject(basePoolSO).Dispose();

        }

        public TObject Pull(TIdSO typeSO)
        {
            TObject @object = default;
            if (_reservedList.Count > 0)
            {
                for (int i = _reservedList.Count - 1; i >= 0; i--)
                {
                    if (_reservedList[i].ContainTag(typeSO))
                    {
                        @object = _reservedList[i];
                        _reservedList.RemoveAt(i);
                        break;
                    }
                }
            }

            if (@object == null)
                @object = InstantiateObject(typeSO);


            return @object;
        }

        private TObject InstantiateObject(TIdSO basePoolSO)
        {

            var cache = MonoBehaviour.Instantiate(basePoolSO.PullPrefab, _parent);
            _allPoolObjects.Add(cache);
            cache.OnDisposed += ReturnBack;
            return cache;
        }

        private void ReturnBack(TObject returningEffect)
        {
            returningEffect.transform.SetParent(_parent);
            _reservedList.Add(returningEffect);
            returningEffect.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            for (int i = 0; i < _allPoolObjects.Count; i++)
            {
                _allPoolObjects[i].Dispose();
                _allPoolObjects[i].OnDisposed -= ReturnBack;
            }
            _allPoolObjects.Clear();
            _reservedList.Clear();
        }
    }
}