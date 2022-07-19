using System;
using UnityEngine;
using UnityEngine.Events;

namespace Account.GeneralData
{
    [Serializable]
    public abstract class IntStat : Stat<int>
    {
        /// <summary>
        /// The Value before it being changed<br></br>
        /// Example: 
        /// Value: 5<br></br>
        /// Incoming value: 4<br></br>
        /// Function AddValue()<br></br>
        /// The outcome: 9 (5+4)<br></br>
        /// You Will Recieve: 5
        /// </summary>
        public event Action<int> OnBeforeValueChanged;
        /// <summary>
        /// The Value before it being changed<br></br>
        /// Example: 
        /// Value: 5<br></br>
        /// Incoming value: 4<br></br>
        /// Function AddValue()<br></br>
        /// The outcome: 9 (5+4)<br></br>
        /// You Will Recieve: 4
        /// </summary>
        public event Action<int> OnRecieveingChangingValue;
        /// <summary>
        /// The Value before it being changed
        /// <br></br>
        /// Example: 
        /// Value: 5<br></br>
        /// Incoming value: 4<br></br>
        /// Function AddValue()<br></br>
        /// The outcome: 9 (5+4)<br></br>
        /// You Will Recieve: 9
        /// </summary>
        public event Action<int> OnAfterValueChanged;


        public IntStat(int val) : base(val) { }



        public override bool AddValue(int value)
        {
            OnBeforeValueChanged?.Invoke(_value);
            OnValueChange?.Invoke(value);
            _value += value;
            OnAfterValueChanged?.Invoke(_value);
            return true;
        }


        public override bool CheckStat(int value)
  => true;


        public override bool ReduceValue(int value)
        {
            OnBeforeValueChanged?.Invoke(_value);
            OnValueChange?.Invoke(value);
            _value -= value;
            OnAfterValueChanged?.Invoke(_value);
            return true;

        }


    }


    [Serializable]
    public abstract class Stat<T> where T : struct
    {
        [SerializeField]
        protected T _value;
        public T Value => _value;

        public UnityAction<T> OnValueChange;
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