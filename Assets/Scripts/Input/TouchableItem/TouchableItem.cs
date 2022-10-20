﻿using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CardMaga.Input
{
    public abstract class TouchableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ILockable
    {
        #region Events

        [Header("Unity Events")]
        [SerializeField] private UnityEvent OnClickEvent;
        [SerializeField] private UnityEvent OnBeginHoldEvent;
        [SerializeField] private UnityEvent OnEndHoldEvent;
        [SerializeField] private UnityEvent OnHoldEvent;
        [SerializeField] private UnityEvent OnPointDownEvent;
        [SerializeField] private UnityEvent OnPointUpEvent;
        
        public event Action OnClick;
        public event Action OnBeginHold;
        public event Action OnEndHold;
        public event Action OnHold;
        public event Action OnPointDown;
        public event Action OnPointUp;

        #endregion

        #region Fields

        public enum State
        {
            Lock,
            UnLock
        }
        
        [Header("Touchable Item configuration")]
        [Tooltip("Disable Hold")] public bool DisableHold = false;
        
        [SerializeField,Tooltip("The delay between moving from point down to hold")] private float _holdDelay = .5f;
        [SerializeField,Tooltip("The distance between the start position to the current position point to hold")] private float _holdDistance = .5f;
        [SerializeField,Tooltip("The current input state")] [ReadOnly] private State _currentState;

        private Vector2 _startPosition;
        private bool _isHold;
        private bool _isTouchable;
        
#if UNITY_EDITOR
        private int _holdLogCount;
#endif

        #endregion

        #region Prop

        public abstract InputIdentificationSO InputIdentification { get; }

        public bool IsTouchable
        {
            get => _isTouchable;
        }
        
        public State CurrentState => _currentState;

        #endregion
        
        #region UnityCallBack

        protected virtual void Awake()
        {
            Lock(); //start Lock
        }

        protected virtual void Start()
        {
            LockAndUnlockSystem.Instance.AddTouchableItemToList(this);
        }

        private void OnDisable()
        {
            if (InputIdentification == null)
                return;
            
            LockAndUnlockSystem.Instance.RemoveTouchableItemToActiveList(this);
        }

        private void OnEnable()
        {
            if (InputIdentification == null || LockAndUnlockSystem.Instance == null)
                return;

            LockAndUnlockSystem.Instance.AddTouchableItemToActiveList(this);
        }

        protected virtual void OnDestroy()
        {
            LockAndUnlockSystem.Instance.RemoveTouchableItemFromAllLists(this);
        }

        #endregion

        #region EventCallBack

        protected virtual void Click()
        {
            OnClick?.Invoke();
            OnClickEvent?.Invoke();
#if UNITY_EDITOR
            //Debug.Log(_touchableItem.name + GetInstanceID() + " Click");
#endif
        }
        
        protected virtual void BeginHold()
        {
            OnBeginHold?.Invoke();
            OnBeginHoldEvent?.Invoke();
#if UNITY_EDITOR
            // Debug.Log(_touchableItem.name + GetInstanceID() + " BeginHold");
#endif
        }
        
        protected virtual void EndHold()
        {
            OnEndHold?.Invoke();
            OnEndHoldEvent?.Invoke();
#if UNITY_EDITOR
            // Debug.Log(_touchableItem.name + GetInstanceID() + " EndHold");
#endif
        }
        
        protected virtual void Hold()
        {
            OnHold?.Invoke();
            OnHoldEvent?.Invoke();
#if UNITY_EDITOR
            if (_holdLogCount % 10 == 0)
            {
                //Debug.Log(_touchableItem.name + GetInstanceID() + " Hold");
            }
            _holdLogCount++;

#endif     
        }
        
        protected virtual void PointDown()
        {
            OnPointDown?.Invoke();
            OnPointDownEvent?.Invoke();
#if UNITY_EDITOR
            //Debug.Log(_touchableItem.name + GetInstanceID() + " PointDown");
#endif
        }
        
        protected virtual void PointUp()
        {
            OnPointUp?.Invoke();
            OnPointUpEvent?.Invoke();
#if UNITY_EDITOR
            //Debug.Log(_touchableItem.name + GetInstanceID() + " PointUp");
#endif
        }

        #endregion
        
        #region TouchProcess

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isTouchable)
                return;
            
            _isHold = false;
            
            if (!DisableHold)
            {
                StartCoroutine(HoldCheck(eventData));
            }
            PointDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isTouchable)
                return;
            
            StopAllCoroutines();
            
            if (_isHold)
            { 
                EndHold(eventData);
                return;
            }
            
            ProcessTouch(eventData);
        }

        private IEnumerator HoldCheck(PointerEventData eventData)
        {
            StartCoroutine(HoldDistance(eventData));
            StartCoroutine(HoldDelay(eventData));

            while (true)
            {
                if (_isHold)
                {
                    StopAllCoroutines();
                    StartCoroutine(ProcessHoldTouchCoroutine(eventData));
                    yield break;
                }
                yield return null;
            }
        }
        
        private IEnumerator HoldDelay(PointerEventData eventData)
        {
            yield return new WaitForSeconds(_holdDelay);
            _isHold = true;
        }

        private IEnumerator HoldDistance(PointerEventData eventData)
        {
            _startPosition = transform.position;
            
            yield return null;
            
            while (!_isHold)
            {
                Vector2 currentTouchPosition = InputReciever.TouchPosOnScreen;
                
                if (Vector2.Distance(_startPosition,currentTouchPosition) > _holdDistance)
                {
                    _isHold = true;
                    yield break;
                }
                
                yield return null;
            }
        }

        private IEnumerator ProcessHoldTouchCoroutine(PointerEventData eventData)
        {
            BeginHold();
            
            while (_isHold)
            {
                Hold();
                yield return null;
            }
        }

        private void EndHold(PointerEventData eventData)
        {
            _isHold = false;
            EndHold();
            PointUp();
        }

        private void ProcessTouch(PointerEventData eventData)
        {
            Click();
            PointUp();
        }


        #endregion
        
        #region StateManagement

        private void ChangeState(State state)
        {
            _currentState = state;

            switch (_currentState)
            {
                case State.Lock:
                    _isTouchable = false;
                    break;
                case State.UnLock:
                    _isTouchable = true;
                    break;
                default:
                    Debug.LogError(name + " State Not Set");
                    break;
            }
        }
        
        [ContextMenu("ToggleState")]
        public void ToggleState()
        {
            if (_currentState == State.Lock)
                ChangeState(State.UnLock);

            else if (_currentState == State.UnLock) ChangeState(State.Lock);
        }

        private void ChangeState(bool isTouchable)
        {
            if (isTouchable)
                ChangeState(State.UnLock);
            else
            {
                StopAllCoroutines();
                ChangeState(State.Lock);
            }
        }


        #endregion

        #region ILockable

        public void Lock()
        {
            ChangeState(false);
        }

        public void UnLock()
        {
            ChangeState(true);
        }

        #endregion
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position,_holdDistance);
        }        
    }

    public abstract class TouchableItem<T> : TouchableItem
        where T : MonoBehaviour
    {
        #region Events
        
        public event Action<T> OnClick;
        public event Action<T> OnBeginHold;
        public event Action<T> OnEndHold;
        public event Action<T> OnHold;
        public event Action<T> OnPointDown;
        public event Action<T> OnPointUp;

        #endregion

        #region Fields

        [SerializeField,Tooltip("The Touchable Item")] T _touchableItem;
        [SerializeField,Tooltip("The current input behaviour state")] [ReadOnly] private InputBehaviourState _currentInputBehaviourState;

        private InputBehaviour<T> _inputBehaviour;
        private InputBehaviour<T> _defaultInputBehaviour;
        
        #endregion

        #region Prop
        
        public InputBehaviour<T> InputBehaviour => _inputBehaviour;
        
        public InputBehaviour<T> DefaultInputBehaviour => _defaultInputBehaviour;

        public InputBehaviourState CurrentInputBehaviourState
        {
            get => _currentInputBehaviourState;
        }

        #endregion

        #region UnityCallBack

        protected override void Awake()
        {
            base.Awake();
            
            if (_defaultInputBehaviour != null)
                return;

            _defaultInputBehaviour = new InputBehaviour<T>();

            _inputBehaviour = _defaultInputBehaviour;
        }
        
        #endregion

        #region EventCallBack

        protected override void Click()
        {
            base.Click();
            OnClick?.Invoke(_touchableItem);
            _inputBehaviour.Click(_touchableItem);
        }

        protected override void BeginHold()
        {
            base.BeginHold();
            OnBeginHold?.Invoke(_touchableItem);
            _inputBehaviour.BeginHold(_touchableItem);
        }

        protected override void Hold()
        {
            base.Hold();
            OnHold?.Invoke(_touchableItem);
            _inputBehaviour.Hold(_touchableItem);
        }

        protected override void EndHold()
        {
            base.EndHold();
            OnEndHold?.Invoke(_touchableItem);
            _inputBehaviour.EndHold(_touchableItem);
        }

        protected override void PointDown()
        {
            base.PointDown();
            OnPointDown?.Invoke(_touchableItem);
            _inputBehaviour.PointDown(_touchableItem);
        }

        protected override void PointUp()
        {
            base.PointUp();
            OnPointUp?.Invoke(_touchableItem);
            _inputBehaviour.PointUp(_touchableItem);
        }

        #endregion
        
        #region InputBehaviourManagement

        public void ChangeState(InputBehaviourState state)
        {
            _currentInputBehaviourState = state;
        }
        
        public bool TrySetInputBehaviour(InputBehaviour<T> inputBehaviour)
        {
            if (inputBehaviour == null || _inputBehaviour == null)
            {
                Debug.LogWarning(name + "InputBehaviour is null");
                return false;
            }

            _inputBehaviour = inputBehaviour;

            if (_inputBehaviour.Equals(inputBehaviour))
                return true;

            return false;
        }
        
        public void ForceSetInputBehaviour(InputBehaviour<T> inputBehaviour)
        {
            _inputBehaviour = inputBehaviour;
        }

        public void ForceResetInputBehaviour()
        {
            _currentInputBehaviourState = InputBehaviourState.Default;
            _inputBehaviour = _defaultInputBehaviour;
        }

        #endregion
    }

    public interface ILockable
    {
        void Lock();
        void UnLock();
    }
}