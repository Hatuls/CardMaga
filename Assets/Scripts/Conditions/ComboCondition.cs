using UnityEngine;
using Conditions;
using Cards;
[System.Serializable]
public class ComboCondition : ConditionAbst
{
    #region Fields
    [Tooltip("What Are the Combo Body Parts Order?")]
    [SerializeField] BodyPartEnum[] _bodyPartEnum;
    #endregion
    #region Properties
    public BodyPartEnum[] GetBodyPartEnum => _bodyPartEnum;
    #endregion
    public override bool IsConditionMet()
    {
        Debug.Log("Always true need to fix!");
        return true;
    }
}
