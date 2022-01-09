using System;
using UnityEngine;
namespace UI
{

    public class FrameCounterNotifier : MonoBehaviour
    {
        [SerializeField]
        float _frameCounter;

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
                if (_notifiers[i].TimeBasedCounter == false)
                  CheckFrameCycle(_notifiers[i]);
                else
                    CheckTimerCycle(_notifiers[i]);
                
            }
        }

        private void CheckTimerCycle(INotifier notifier)
        {
            if (notifier.Timer >= notifier.TimerCycle)
            {
                notifier.Notify();
                notifier.Timer = 0;
            }
            else
                notifier.Timer += Time.deltaTime;
        }

        private void FrameCounter()
        {
            _frameCounter = (_frameCounter < 0) ? 1 : _frameCounter + 1;
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
        bool TimeBasedCounter { get; }
        float Timer { get; set; }
        float TimerCycle { get; }
        int FrameCycle { get; }
        void Notify();
    }

    #region Notifiers
    [Serializable]
    public class EventNotifier : INotifier
    {
        [SerializeField]
        bool toUseTime;
        [Range(0, ushort.MaxValue)]
        [SerializeField]
        int _frameCycle;
        [Range(0, byte.MaxValue)]
        [SerializeField]
        float _timerCycle;


        public int FrameCycle => _frameCycle;
        float _currentTimer;
        public float Timer { get => _currentTimer;set=> _currentTimer = value; }

        public float TimerCycle => _timerCycle;

        public bool TimeBasedCounter => toUseTime;

        [SerializeField]
        UnityEngine.Events.UnityEvent _onFrameCycle;
        public void Notify()
        {
            _onFrameCycle?.Invoke();
        }
    }
    [Serializable]
    public abstract class AnimationNotifier : INotifier

    {
        [SerializeField]
        private bool toUseTime;

        [Range(0, ushort.MaxValue)]
        [SerializeField]
        int _frameCycle;
        [Range(0, byte.MaxValue)]
        [SerializeField]
        float _timeCycle;

        float _timeCounter =0;
        public int FrameCycle => _frameCycle;

        public bool TimeBasedCounter { get => toUseTime; }
        public float Timer { get => _timeCounter; set => _timeCounter = value; }

        public float TimerCycle => _timeCycle;

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