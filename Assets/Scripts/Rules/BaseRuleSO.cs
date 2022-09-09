using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseRuleSO : ScriptableObject, IDisposable
{
    public abstract void IsConditionMet();
    public abstract void AfterRuleActivated();
    public abstract void InitRule();
    public abstract void Dispose();
}
