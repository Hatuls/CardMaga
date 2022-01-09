using System;
using UnityEngine;
namespace UI
{

    public class FrameCounterNotifier : MonoBehaviour
    {
        int _frameCounter;
        [SerializeReference]
        INotifier[] _notifiers;
        // Update is called once per frame
        void Update()
        {
            CheckArray();
        }
        void CheckArray()
        {
            FrameCounter();

            for (int i = 0; i < _notifiers.Length; i++)
            {
                CheckFrameCycle(_notifiers[i]);
            }
        }

        private void FrameCounter()
        {
            _frameCounter = (_frameCounter < 0) ? 1 : _frameCounter++;
        }

        private void CheckFrameCycle(INotifier notifer)
        {
            if (_frameCounter % notifer.FrameCycle == 0)
            {
                notifer.Notify();
            }
        }
    }

    public interface INotifier
    {
        int FrameCycle { get; }
        void Notify();
    }

    #region Notifiers
    [Serializable]
    public class EventNotifier : INotifier
    {
        [Range(0, byte.MaxValue)]
        [SerializeField]
        int _frameCycle;
        public int FrameCycle => _frameCycle;
        [SerializeField]
        UnityEngine.Events.UnityEvent _onFrameCycle;
        public void Notify()
        {
            _onFrameCycle?.Invoke();
        }
    }

    public abstract class AnimationNotifier : INotifier
    {
        [Range(0, byte.MaxValue)]
        [SerializeField]
        int _frameCycle;
        public int FrameCycle => _frameCycle;
        [SerializeField]
        protected Animator _animator;

        [HideInInspector]
        [SerializeField]
        protected int _parameterHashName;

#if UNITY_EDITOR
        [SerializeField]
        [Sirenix.OdinInspector.OnValueChanged("SetHash")]
        protected string _parameterName;
        private void SetHash()
        {
            _parameterHashName = Animator.StringToHash(_parameterName);
        }
#endif
        public abstract void Notify();
    }
    [Serializable]
    public class TriggerAnimationNotifier : AnimationNotifier
    {
        public override void Notify()
        {
            _animator.SetTrigger(_parameterHashName);
        }
    }
    [Serializable]
    public class BoolAnimationNotifier : AnimationNotifier
    {
        [SerializeField] bool Value;
        public override void Notify()
        {
            _animator.SetBool(_parameterHashName, Value);
        }
    }
    [Serializable]
    public class IntAnimationNotifier : AnimationNotifier
    {
        [SerializeField] int Value;
        public override void Notify()
        {
            _animator.SetInteger(_parameterHashName, Value);
        }
    }
    [Serializable]
    public class FloatAnimationNotifier : AnimationNotifier
    {
        [SerializeField] float Value;
        public override void Notify()
        {
            _animator.SetFloat(_parameterHashName, Value);
        }
    }
    #endregion
}