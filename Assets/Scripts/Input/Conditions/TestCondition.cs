using UnityEngine;

public class TestCondition : BaseCondition
{
    [SerializeField] private bool Test = false;
    public override bool CheckCondition()
    {
        if (Test)
        {
            return true;
        }

        return false;
    }
}
