using System;
namespace Account.GeneralData
{
    public class DateTimeStat : Stat<DateTime>
    {
        public DateTimeStat(DateTime val) : base(val)
        {
        }

        public override bool AddValue(DateTime value)
        {
            throw new NotImplementedException();
        }

        public override bool CheckStat(DateTime value)
        {
            throw new NotImplementedException();
        }

        public override bool ReduceValue(DateTime value)
        {
            throw new NotImplementedException();
        }
    }

}