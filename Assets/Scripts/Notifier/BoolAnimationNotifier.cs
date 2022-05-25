using System;
using UnityEngine;
namespace UI
{
    [Serializable]
    public class BoolAnimationNotifier : AnimationNotifier
    {
        [SerializeField] bool Value;
        public override void Notify()
        {
            _animator.SetBool(_parameterHashName, Value);
        }
    }
}