using System;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class ServerManager :MonoBehaviour
    {
        #region Singleton
        private static ServerManager _instance;
        public static ServerManager GetInstance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("ServerManager is null!");

                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        #endregion
        #region Fields
        NewsData _newsData;
        DealData _dealData;
        #endregion
        #region Public Methods
        public void PopUpNews(DateTime dateTime)
        {

        }
        #endregion

    }
}
