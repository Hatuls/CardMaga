using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rei.Utilities
{
    public abstract class SortAbst<T> : MonoBehaviour, ISort<T> where T : class
    {

        public abstract void SortRequest();
        public abstract IEnumerable<T> Sort();
    }
}