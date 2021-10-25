using System;
using System.Collections.Generic;
using UnityEngine;

namespace Account
{
    public class TimeManager : MonoBehaviour
    {
        #region Singleton
        private static TimeManager _instance;
        public static TimeManager GetInstance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("TimeManager is null!");

                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        #endregion
        #region Fields
        static List<TimeEvent> _timeEvents;
        #endregion
        #region Private Methods
        bool CheckIfComplete(TimeEvent timeEvent)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Public Methods
        public void Init()
        {
            
        }
        public DateTime GetCurrentTime()
        {
            Debug.Log($"{DateTime.Now}");
            return DateTime.Now;
        }
        public void Update()
        {
            
        }
        public void RegisterTimeEvent(TimeEvent timeEvent)
        {

        }
        #endregion
    }
}
