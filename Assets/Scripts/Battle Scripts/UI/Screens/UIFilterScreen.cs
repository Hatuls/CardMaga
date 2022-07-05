using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.UI
{
    public abstract class UIFilterScreen<T, U> : MonoBehaviour where T : MonoBehaviour where U : class
    {
        [SerializeField]
        protected GameObject _cardUIPrefab;
        [SerializeField]
        protected List<T> _collection = new List<T>();

        [SerializeField]
        UnityEvent OnUIFinishSorting;
        public IList<T> Collection => _collection;
        public IReadOnlyList<T> OnlyActiveCollection => _collection.Where((x)=> x.gameObject.activeSelf).ToList();
        ISort<U> _lastSort;
        protected abstract void OnActivate(IEnumerable<U> sortedDeck, int i); // redo
        protected abstract void CreatePool();

        public virtual void Refresh()
        {
            SortBy(_lastSort);
        }

        public void SortBy(ISort<U> sortMethod)
        {
            if (sortMethod == null)
                return;

            _lastSort = sortMethod;
            // replace with pool
            CreatePool();
            int length = _collection.Count;
            var sortedDeck = sortMethod.Sort();
            int sortedDeckLength = sortedDeck.Count();



            for (int i = 0; i < length; i++)
            {
                if (i < sortedDeckLength && sortedDeck.ElementAt(i) != null)
                {

                    _collection[i].gameObject.SetActive(true);
                    OnActivate(sortedDeck, i);
                }
                else
                {

                    _collection[i].gameObject.SetActive(false);
                }
            }
            OnUIFinishSorting?.Invoke();

        }

    }
}

