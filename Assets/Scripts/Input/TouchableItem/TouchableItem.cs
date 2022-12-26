using Sirenix.OdinInspector;
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
        [SerializeField,EventsGroup] private UnityEvent OnClickEvent;
        [SerializeField,EventsGroup] private UnityEvent OnBeginHoldEvent;
        [SerializeField,EventsGroup] private UnityEvent OnEndHoldEvent;
        [SerializeField,EventsGroup] private UnityEvent OnHoldEvent;
        [SerializeField,EventsGroup] private UnityEvent OnPointDownEvent;
        [SerializeField,EventsGroup] private UnityEvent OnPointUpEvent;
        
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
        [Tooltip("Disable Click")] public bool DisableClick = false;
        
        [SerializeField,Tooltip("The delay between moving from point down to hold")] private float _holdDelay = .5f;
        [SerializeField,Tooltip("The distance between the start position to the current position point to hold")] private float _holdDistance = .5f;
        [SerializeField,Tooltip("The current input state")] [ReadOnly] private State _currentState;
        [SerializeField, Tooltip("Need to add tooltip")] private InputIdentificationSO _identification;

        private InputBehaviour _inputBehaviour;

        private Vector2 _startPosition;
        private bool _isHold;
        private bool _isTouchable;
        
        
#if UNITY_EDITOR
        private int _holdLogCount;
#endif

        #endregion

        #region Prop
        
        
        public State CurrentState => _currentState;

        #endregion
        
        #region UnityCallBack

        protected virtual void Awake()
        {
            Lock();//start Lock
            
            if ((InputIdentification != null) && (LockAndUnlockSystem.Instance != null))
                LockAndUnlockSystem.Instance.AddTouchableItemToList(this);
        }
        
        protected virtual void OnDestroy()
        {
            if(LockAndUnlockSystem.Instance != null)
            LockAndUnlockSystem.Instance.RemoveTouchableItemFromAllLists(this);
          
        }

        #endregion

        #region EventCallBack

        protected virtual void Click()
        {
            if (DisableClick)
                return;
            OnClick?.Invoke();
            OnClickEvent?.Invoke();
            _inputBehaviour?.Click();
#if UNITY_EDITOR
            //Debug.Log(_touchableItem.name + GetInstanceID() + " Click");
#endif
        }
        
        protected virtual void BeginHold()
        {
            OnBeginHold?.Invoke();
            OnBeginHoldEvent?.Invoke();
            _inputBehaviour?.BeginHold();
#if UNITY_EDITOR
            // Debug.Log(_touchableItem.name + GetInstanceID() + " BeginHold");
#endif
        }
        
        protected virtual void EndHold()
        {
            OnEndHold?.Invoke();
            OnEndHoldEvent?.Invoke();
            _inputBehaviour?.EndHold();
#if UNITY_EDITOR
            // Debug.Log(_touchableItem.name + GetInstanceID() + " EndHold");
#endif
        }
        
        protected virtual void Hold()
        {
            OnHold?.Invoke();
            OnHoldEvent?.Invoke();
            _inputBehaviour?.Hold();
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
            _inputBehaviour?.PointDown();
#if UNITY_EDITOR
            //Debug.Log(_touchableItem.name + GetInstanceID() + " PointDown");
#endif
        }
        
        protected virtual void PointUp()
        {
            OnPointUp?.Invoke();
            OnPointUpEvent?.Invoke();
            _inputBehaviour?.PointUp();
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

            while (!_isHold)
            {
                yield return null;
            }
            StopAllCoroutines();
            StartCoroutine(ProcessHoldTouchCoroutine(eventData));
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
                Vector2 currentTouchPosition = InputReciever.Instance.TouchPosOnScreen;
                
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

        #region InputBehaviourManagement

        public bool TrySetInputBehaviour(InputBehaviour inputBehaviour)
        {
            if (inputBehaviour == null)
            {
                Debug.LogWarning(name + "InputBehaviour is null");
                return false;
            }

            _inputBehaviour = inputBehaviour;

            if (_inputBehaviour.Equals(inputBehaviour))
                return true;

            return false;
        }
        
        public void ForceResetInputBehaviour()
        {
            _inputBehaviour = null;
        }

        #endregion

        #region ILockable

        public bool IsLock
        {
            get => !_isTouchable;
        }

        public InputIdentificationSO InputIdentification
        {
            get => _identification;
        }

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
        
        public event Action<T> OnClickValue;
        public event Action<T> OnBegineValue;
        public event Action<T> OnEndHoldValue;
        public event Action<T> OnHoldValue;
        public event Action<T> OnPointDownValue;
        public event Action<T> OnPointUpValue;


        #endregion

        #region Fields

        [SerializeField,Tooltip("The Touchable Item")] T _touchableItem;
        [SerializeField,Tooltip("The current input behaviour state")] [ReadOnly] private InputBehaviourState _currentInputBehaviourState;

        private InputBehaviour<T> _inputBehaviour;
        
        #endregion

        #region Prop
        
        public InputBehaviour<T> InputBehaviour => _inputBehaviour;
        
        public InputBehaviourState CurrentInputBehaviourState
        {
            get => _currentInputBehaviourState;
        }

        #endregion

        #region EventCallBack

        protected override void Click()
        {
            if (DisableClick)
                return;
            base.Click();
            OnClickValue?.Invoke(_touchableItem);
            _inputBehaviour?.Click(_touchableItem);
        }

        protected override void BeginHold()
        {
            base.BeginHold();
            OnBegineValue?.Invoke(_touchableItem);
            _inputBehaviour?.BeginHold(_touchableItem);
        }

        protected override void Hold()
        {
            base.Hold();
            OnHoldValue?.Invoke(_touchableItem);
            _inputBehaviour?.Hold(_touchableItem);
        }

        protected override void EndHold()
        {
            base.EndHold();
            OnEndHoldValue?.Invoke(_touchableItem);
            _inputBehaviour?.EndHold(_touchableItem);
        }

        protected override void PointDown()
        {
            base.PointDown();
            OnPointDownValue?.Invoke(_touchableItem);
            _inputBehaviour?.PointDown(_touchableItem);
        }

        protected override void PointUp()
        {
            base.PointUp();
            OnPointUpValue?.Invoke(_touchableItem);
            _inputBehaviour?.PointUp(_touchableItem);
        }

        #endregion
        
        #region InputBehaviourManagement

        public void ChangeState(InputBehaviourState state)
        {
            _currentInputBehaviourState = state;
        }
        
        public bool TrySetInputBehaviour(InputBehaviour<T> inputBehaviour)
        {
            if (inputBehaviour == null)
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
            _inputBehaviour = null;
        }

        #endregion
    }

    public interface ILockable
    {
        bool IsLock { get; }
        InputIdentificationSO InputIdentification { get; }
        void Lock();
        void UnLock();
    }
}