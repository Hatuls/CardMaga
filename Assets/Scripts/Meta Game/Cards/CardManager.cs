using UnityEngine;
namespace Cards.Meta
{
    public class CardManager : MonoBehaviour
    {
        #region Singleton
        private static CardManager _instance;
        public static CardManager GetInstance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("CardManager is null!");

                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        #endregion
        #region Fields
        DismantleCardHandler _dismantleCardHandler;
        UpgradeCardHandler _upgradeCardHandler;
        DismantleChartSO _dismantleChartSO;
        UpgradeChartSO _upgradeChartSO;
        #endregion
        #region Properties
        public DismantleCardHandler DismantleCardHandler => _dismantleCardHandler;
        public UpgradeCardHandler UpgradeCardHandler => _upgradeCardHandler;
        #endregion
    }
}
