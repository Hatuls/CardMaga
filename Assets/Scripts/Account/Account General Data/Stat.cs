using System;
using UnityEngine;

namespace Account.GeneralData
{
    [Serializable]
    public abstract class Stat<T> where T : struct
    {
        [SerializeField]
        protected T _value;
        public T Value => _value;

        public Stat(T val)
        {
            _value = val;
        }
        public Stat()
        {
            _value = default;
        }
        public abstract bool CheckStat(T value);
        public abstract bool AddValue(T value);
        public abstract bool ReduceValue(T value);

    }

}