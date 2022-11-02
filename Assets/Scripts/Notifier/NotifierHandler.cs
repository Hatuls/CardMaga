using System;
using UnityEngine;
namespace UI
{

    public class NotifierHandler : MonoBehaviour
    {
        [SerializeField]
        float _frameCounter;

        [SerializeReference]
        BaseNotifier[] _notifiers;
        // Update is called once per frame
        void Update()
        {
            CheckArray();
        }
        void CheckArray()
        {
            for (int i = 0; i < _notifiers.Length; i++)
                _notifiers[i].Tick();
        }
      }

    public interface INotifier
    {
        void Notify();
    }

    #region Notifiers

    public abstract class BaseNotifier : MonoBehaviour, INotifier
    {
        public virtual void Tick()
        {
            if (ConditionsMet())
                Notify();
        }
        public abstract bool ConditionsMet();
        public abstract void Notify();
    }
    public abstract class BaseEventNotifier :BaseNotifier
    {
        [SerializeField]
     protected   UnityEngine.Events.UnityEvent OnNotifiy;
        public override void Notify()
        {
            OnNotifiy?.Invoke();
        }
    }

    [Serializable]
    public abstract class AnimationNotifier :  BaseNotifier
    {
       
        [Range(0, byte.MaxValue)]
        [SerializeField]
        private float _loopTime;

        public float Counter { get; private set; }

      

        [SerializeField]
        protected Animator _animator;

        [HideInInspector]
        [SerializeField]
        protected int _parameterHashName;


        public override bool ConditionsMet()
        {
            bool result = Counter>= _loopTime;

            if (result)
                Counter = 0;
            else
                Counter += Time.deltaTime;

                return result;
        }

#if UNITY_EDITOR
        [SerializeField]
        [Sirenix.OdinInspector.OnValueChanged("SetHash")]
        protected string _parameterName;
        private void SetHash()
        {
            _parameterHashName = Animator.StringToHash(_parameterName);
        }
#endif
   
    }
    #endregion
}