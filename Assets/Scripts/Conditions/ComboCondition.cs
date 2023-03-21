using UnityEngine;
using Conditions;

[System.Serializable]
public class ComboCondition : ConditionAbst
{
    #region Fields
    [Tooltip("What Are the Combo Body Parts Order?")]
    [SerializeField] CardMaga.Card.BodyPartEnum[] _bodyPartEnum;
    #endregion
    #region Properties
    public CardMaga.Card.BodyPartEnum[] GetBodyPartEnum => _bodyPartEnum;
    #endregion
    public override bool IsConditionMet()
    {
        Debug.Log("Always true need to fix!");
        return true;
    }
}
