using UnityEngine;
namespace UI
{
    public class FloatAnimationNotifier : AnimationNotifier
    {
        [SerializeField] float Value;
        public override void Notify()
        {
            _animator.SetFloat(_parameterHashName, Value);
        }
    }
}