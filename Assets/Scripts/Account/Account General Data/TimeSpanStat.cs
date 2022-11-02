using System;
namespace Account.GeneralData
{
    public class TimeSpanStat : Stat<TimeSpan>
    {
        public TimeSpanStat(TimeSpan val) : base(val)
        {
        }

        public override bool AddValue(TimeSpan value)
        {
            throw new NotImplementedException();
        }

        public override bool CheckStat(TimeSpan value)
        {
            throw new NotImplementedException();
        }

        public override bool ReduceValue(TimeSpan value)
        {
            throw new NotImplementedException();
        }
    }


}