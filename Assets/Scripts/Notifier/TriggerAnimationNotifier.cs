namespace UI
{
    public class TriggerAnimationNotifier : AnimationNotifier
    {
        public override void Notify()
        {
            _animator.SetTrigger(_parameterHashName);
        }
    }
}