using CardMaga.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rei.Utilities
{
    public abstract class SortAbst<T> : MonoBehaviour, ISort<T> where T : class
    {
        public event Func<IReadOnlyCollection<T>> OnSortingCollectionRequested;
        public abstract void SortRequest();
        public abstract IEnumerable<T> Sort();
        protected IReadOnlyCollection<T> GetCollection() => OnSortingCollectionRequested.Invoke();
    }

}