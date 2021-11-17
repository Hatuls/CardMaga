using System;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class UshortStat : Stat<ushort>
    {
        public UshortStat(ushort val) : base(val)
        {

        }
        public UshortStat() : base()
        {

        }
        public override bool AddValue(ushort value)
        {
            if(value <= 0)
            {
                throw new Exception("UshortStat value inserted is lower or equal to 0");
            }
            _value += value;
            return true;
        }

        public override bool CheckStat(ushort value)
        {
            Debug.Log($"value Inserted to be compared against: {value}");
            Debug.Log($"Current Value: {_value}");
            if (value <= 0)
            {
                throw new Exception("UshortStat value inserted is lower or equal to 0");
            }
         
            return _value >= value;
        }

        public override bool ReduceValue(ushort value)
        {
            if (value <= 0)
            {
                throw new Exception("UshortStat value inserted is lower or equal to 0");
            }
            if(_value < value)
                return false;
            
            else
                _value -= value;
                return true;
            

        }
    }

}