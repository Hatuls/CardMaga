using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Map.UI
{
    public abstract class UIFilterScreen<T, U> : MonoBehaviour where T :  MonoBehaviour where U : class 
    {
        [SerializeField]
        protected GameObject _cardUIPrefab;
        [SerializeField]
        protected List<T> _collection = new List<T>();
        [SerializeField]
        UnityEvent OnBeforeSorting;
        public IReadOnlyList<T> Collection=>_collection;
        ISort<U> _lastSort;
        protected abstract void OnActivate(IEnumerable<U> sortedDeck, int i);
        protected abstract void CreatePool();
        public virtual void Refresh()
        {
            SortBy(_lastSort);
        }

        public void SortBy(ISort<U> sortedMethod)
        {
            if (sortedMethod == null)
                return;

            _lastSort = sortedMethod;
            CreatePool();
            int length = _collection.Count;
            var sortedDeck =sortedMethod.Sort();

            int sortedDeckLength = sortedDeck.Count();



            for (int i = 0; i < length; i++)
            {
                if (i < sortedDeckLength && sortedDeck.ElementAt(i) != null)
                {
                    if (_collection[i].gameObject.activeSelf == false)
                        _collection[i].gameObject.SetActive(true);
                    OnActivate(sortedDeck, i);
                }
                else
                {
                    if (_collection[i].gameObject.activeSelf == true)
                        _collection[i].gameObject.SetActive(false);
                }
            }
            OnBeforeSorting?.Invoke();
        }

     

    }
}

