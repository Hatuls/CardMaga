using UnityEngine;
namespace Conditions
{
    public enum ConditionEnum {
        Health,
        Attack,
        Defend,
        Cards,
        InDeck,
        InDisposal,
        InHand,
        OnDraw,
        OnExhaust,
        OnDispose,
        OnKnockUp,
        EnemyHealth,
        OnFall,
        OnJump,
        OnFirstCard,
        OnLastCard,
        OnRecievingDamage,
        OnBlock,
        On
    };
    public enum OperatorEnum { 
        SmallerOrEqualTo,
        BiggerOrEqualTo,
        SmallerThan,
        BiggerThan,
        NotEqualTo,
        EqualTo 
    };
    public abstract class ConditionAbst
    {
        public abstract bool IsConditionMet();
    }

}