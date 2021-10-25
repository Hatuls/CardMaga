using System;

namespace Account
{
    public class TimeEvent
    {
        #region Fields
        public DateTime _finishTime;
        public Action ExecuteOnFinish;
        #endregion
        #region Public Methods
        public TimeEvent(DateTime dateTime, Action action)
        {

        }
        public bool CheckIfFinished(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
