using UnityEngine;

namespace Server
{
    [CreateAssetMenu(fileName = "Deal SO", menuName = "ScriptableObjects/MetaGame/Deal SO")]

    public class DealSO : ScriptableObject
    {
        #region Fields
        Sprite _backgroundSprite;
        Sprite _dealSprite;
        string _dealText;
        #endregion

        #region Public Methods
        public DealSO(string deal)
        {

        }
        #endregion
    }
}
