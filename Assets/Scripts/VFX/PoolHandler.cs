
using CardMaga.Battle.Players;
using CardMaga.Tools.Pools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private readonly List<TObject> _allPoolsObjects;
        private readonly List<TObject> _reservedList;
        private readonly Transform _parent;

        public PoolHandler(Transform defaultParent)
        {
            _parent = defaultParent;
            _allPoolsObjects = new List<TObject>();
            _reservedList = new List<TObject>();
        }

        public  void PopulatePool(TIdSO basePoolSO, int amount)
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
            _allPoolsObjects.Add(cache);
            cache.OnDisposed += ReturnBack;
            return cache;
        }

        private void ReturnBack(TObject returningEffect)
        {
            if (_reservedList.Contains(returningEffect))
                return;

            returningEffect.transform.SetParent(_parent);
            _reservedList.Add(returningEffect);
            returningEffect.gameObject.SetActive(false);
        }

        public void DestoryAllAndDispose()
        {
            for (int i = 0; i < _allPoolsObjects.Count; i++)
            {
                _allPoolsObjects[i].Dispose();
                _allPoolsObjects[i].OnDisposed -= ReturnBack;
                MonoBehaviour.Destroy(_allPoolsObjects[i].gameObject);
            }
            _allPoolsObjects.Clear();
            _reservedList.Clear();
        }
        public void Dispose()
        {
            for (int i = 0; i < _allPoolsObjects.Count; i++)
            {
                _allPoolsObjects[i].Dispose();
                _allPoolsObjects[i].OnDisposed -= ReturnBack;
            }
            _allPoolsObjects.Clear();
            _reservedList.Clear();
        }
    }
}