using UnityEngine;


namespace Conditions
{
    [System.Serializable]
    public class Condition
    {
        #region Fields
        [Tooltip("Is by combo")]
        [SerializeField] ComboCondition[] _comboCondition;
        [Tooltip("Is by parameter")]
        [SerializeField] ParamCondition[] _paramaterCondition;
        #endregion
        #region Properties
        public ComboCondition[] GetComboCondition => _comboCondition;
        public ParamCondition[] GetParamaterCondition => _paramaterCondition;
        #endregion
       
    }
}
