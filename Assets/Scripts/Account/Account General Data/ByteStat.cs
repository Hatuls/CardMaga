using System;
namespace Account.GeneralData
{
    [Serializable]
    public class ByteStat : Stat<byte>
    {
        public ByteStat() : base()
        {

        }
        public ByteStat(byte val) : base(val)
        {
        }

        public override bool AddValue(byte value)
        {
            if (value <= 0)
            {
                throw new Exception("ByteStat value inserted is lower or equal to 0");
            }
            _value += value;
            return true;
        }

        public override bool CheckStat(byte value)
        {
            if (value <= 0)
            {
                throw new Exception("ByteStat value inserted is lower or equal to 0");
            }
            if (_value >= value)
            {
                return true;
            }
            return false;
        }

        public override bool ReduceValue(byte value)
        {
            if (value <= 0)
            {
                throw new Exception("ByteStat value inserted is lower or equal to 0");
            }
            if (_value < value)
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