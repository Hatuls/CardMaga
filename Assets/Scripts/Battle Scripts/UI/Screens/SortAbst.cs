using Map.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Rei.Utilities
{
    public abstract class SortAbst<T> : MonoBehaviour, ISort<T> where T : class
    {
        [SerializeField]
        protected SortCardEvent _cardEvent;
        [SerializeField]
        protected SortComboEvent _comboEvent;
        public abstract void SortRequest();
        public abstract IEnumerable<T> Sort();
    }
}