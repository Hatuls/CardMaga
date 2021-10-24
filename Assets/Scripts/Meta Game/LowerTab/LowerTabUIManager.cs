using UI.Meta.PlayScreen;
using UnityEngine;

namespace UI.Meta
{
    public class LowerTabUIManager : MonoBehaviour
    {
        #region Singleton
        private static LowerTabUIManager _instance;
        public static LowerTabUIManager GetInstance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("LowerTabUIManager is null!");

                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        #endregion
        #region Fields
        //TrainingHallScreenUI _trainingHallScreenUI;
        //WorkShopScreenUI _workshopScreenUI;
        //LabratoryScreenUI _laboratoryScreenUI;
        PlayScreenUI _PlayScreenUI;
        //ExtraTabUI _extraTabUI;
        #endregion
        #region Public Methods
        public void Selected(int index)
        {

        }
        public void ResetLowerTab()
        {

        }
        #endregion
    }
}