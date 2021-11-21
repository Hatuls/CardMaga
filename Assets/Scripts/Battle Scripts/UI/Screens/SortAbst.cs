using System.Collections.Generic;
using UnityEngine;


namespace Rei.Utilities
{
    public abstract class SortAbst<T> : MonoBehaviour, ISort<T> where T : class
    {
        public abstract IEnumerable<T> Sort();
    }
}