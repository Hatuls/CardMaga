using UnityEngine;
using Conditions;
[System.Serializable]
public class ParamCondition : ConditionAbst
{
    #region Fields
    [Header("Condition: ")]
    [Tooltip("What Aspect is it based on?")]
    [SerializeField] ConditionEnum _paramaterFrom;

    [Tooltip(" is the previous Parameter is ***** to the amount under")]
    [SerializeField] OperatorEnum _mathOperator;

    [Header("Compared to section:")]
    [Tooltip("if its false it will take the number from amount.\n if true it will take it from the Enum spesification\n ")]
    [SerializeField] bool _isByNumberOrEnum;

    [Tooltip("Is the amount underneath is precentage?")]
    [SerializeField] bool _isPrecentage;

    [Tooltip("Amount to Check")]
    [SerializeField] int _amount;

    [Tooltip("What Aspect is it based on?")]
    [SerializeField] ConditionEnum _parameterTo;
    #endregion
    #region Properties
    public ConditionEnum GetConditionEnumFROM => _paramaterFrom;
    public OperatorEnum GetMathOperator => _mathOperator;
    public bool GetIsByNumberOrEnum => _isByNumberOrEnum;
    public bool GetIsPrecentage => _isPrecentage;
    public int GetAmount => _amount;
    public ConditionEnum GetConditionEnumTO => _parameterTo;

    #endregion
    public override bool IsConditionMet()
    {
        Debug.Log("Always true need to fix!");
        return true;
    }
}
