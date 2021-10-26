using System;
namespace Account.GeneralData
{
    public class UshortStat : Stat<ushort>
    {
        public UshortStat(ushort val) : base(val)
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
            if (value <= 0)
            {
                throw new Exception("UshortStat value inserted is lower or equal to 0");
            }
            if (_value >= value)
            {
                return true;
            }
            return false;
        }

        public override bool ReduceValue(ushort value)
        {
            if (value <= 0)
            {
                throw new Exception("UshortStat value inserted is lower or equal to 0");
            }
            if(_value < value)
            {
                return false;
            }
            else
            {
                _value -= value;
                return true;
            }

        }
    }

}