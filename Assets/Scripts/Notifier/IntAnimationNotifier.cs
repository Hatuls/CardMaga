using UnityEngine;
namespace UI
{
    public class IntAnimationNotifier : AnimationNotifier
    {
        [SerializeField] int Value;
        public override void Notify()
        {
            _animator.SetInteger(_parameterHashName, Value);
        }
    }
}