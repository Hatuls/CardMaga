using UnityEngine;
using UI.Meta.Workshop;

namespace Meta
{
    public class WorkshopManager : MonoBehaviour
    {
        #region Singleton
        private static WorkshopManager _instance;
        public static WorkshopManager GetInstance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("WorkshopManager is null!");

                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        #endregion

        #region Fields
        Bundle[] _bundles;
        #endregion
        #region Public Methods
        public void RewardBundle(int index)
        {

        }
        #endregion
    }
}
