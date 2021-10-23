using System;
namespace Account.GeneralData
{
    [Serializable]
    public class Stat<T> where T : struct
    {
        private T _value;
        public T Value => _value;

        public Stat(T val)
        {
            _value = val;
        }
        public virtual bool CheckStat(T value)
        {
            throw new NotImplementedException();
        
        }


        public virtual bool AddValue(T value)
        {
            throw new NotImplementedException();
        }
        public virtual bool ReduceValue(T value)
        {
            throw new NotImplementedException();
        }
    }
}