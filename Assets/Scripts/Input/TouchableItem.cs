using Sirenix.OdinInspector;
using System;
using System.Collections;
using PlayFab.MultiplayerModels;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardMaga.Input
{

    public class TouchableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        
        protected event Action OnClick;
        protected event Action OnBeginHold;
        protected event Action OnEndHold;
        protected event Action OnHold;
        protected event Action OnPointDown;
        protected event Action OnPointUp;
        
        public enum State
        {
            Lock,
            UnLock
        }

        [SerializeField] private float _holdDelaySce = .5f;

        [SerializeField] [ReadOnly] private State _currentState;
        private bool _isHold;

        private bool _isTouchable;

        public State CurrentState => _currentState;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isTouchable)
            {
                StartCoroutine(HoldDelay(eventData));
                OnPointDown?.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isTouchable)
            {
                if (_isHold)
                {
                    EndHold(eventData);
                    return;
                }

                ProcessTouch(eventData);
            }
        }
        
        private IEnumerator HoldDelay(PointerEventData eventData)
        {
            yield return new WaitForSeconds(_holdDelaySce);
            _isHold = true;
            StartCoroutine(ProcessHoldTouchCoroutine(eventData));
        }

        private IEnumerator ProcessHoldTouchCoroutine(PointerEventData eventData)
        {
            yield return null;
            OnBeginHold?.Invoke();

            while (_isHold)
            {
                yield return null;
                OnHold?.Invoke();
            }
        }

        private void EndHold(PointerEventData eventData)
        {
            _isHold = false;
            StopAllCoroutines();
            OnEndHold?.Invoke();
            OnPointUp?.Invoke();
        }

        private void ProcessTouch(PointerEventData eventData)
        {
            StopAllCoroutines();
            OnClick?.Invoke();
            OnPointUp?.Invoke();
        }

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

            Debug.Log(name + " is touchable set to " + _isTouchable);
        }

        [ContextMenu("ToggleState")]
        public void ToggleState()
        {
            if (_currentState == State.Lock)
                ChangeState(State.UnLock);

            else if (_currentState == State.UnLock) ChangeState(State.Lock);
        }

        public void ForceChangeState(bool isTouchable)
        {
            if (isTouchable)
                ChangeState(State.UnLock);
            else
                ChangeState(State.Lock);
        }
    }


    public class TouchableItem<T> : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
        where T : MonoBehaviour
    {
        
        public virtual event Action<T> OnClick;
        public virtual event Action<T> OnBeginHold;
        public virtual event Action<T> OnEndHold;
        public virtual event Action<T> OnHold;
        public virtual event Action<T> OnPointDown;
        public virtual event Action<T> OnPointUp;
        
        public enum State
        {
            Lock,
            UnLock
        }

        [SerializeField,Tooltip("The Touchable Item")] T _touchableItem;
        [SerializeField,Tooltip("The delay between moving from point down to hold")] private float _holdDelay = .5f;
        [SerializeField,Tooltip("The distance between the start position to the current position point to hold")] private float _holdDistance = .5f;
        [SerializeField,Tooltip("The current input state")] [ReadOnly] private State _currentState;

        private Vector2 _startPosition;
        
        private bool _isHold;
        private bool _isTouchable;
        public State CurrentState => _currentState;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isTouchable)
                return;
            
            _isHold = false;
            StartCoroutine(HoldCheck(eventData)); 
            OnPointDown?.Invoke(_touchableItem);
            Debug.Log( base.name + "OnPointDown");
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
            Debug.Log("From dely");
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
                    Debug.Log("FromDis");
                    _isHold = true;
                    yield break;
                }
                
                yield return null;
            }
        }

        private IEnumerator ProcessHoldTouchCoroutine(PointerEventData eventData)
        {
            OnBeginHold?.Invoke(_touchableItem);
            Debug.Log(base.name + "OnBeginHold");
            
            int count = 0;
            while (_isHold)
            {
                OnHold?.Invoke(_touchableItem);
                
                if (count % 10 == 0)
                {
                    Debug.Log(base.name + "OnHold");
                }
                count++;
                
                yield return null;
            }
        }

        private void EndHold(PointerEventData eventData)
        {
            _isHold = false;
            OnEndHold?.Invoke(_touchableItem);
            Debug.Log(base.name + "OnEndHold");
            OnPointUp?.Invoke(_touchableItem);
            Debug.Log(base.name + "OnPointUp");
        }

        private void ProcessTouch(PointerEventData eventData)
        {
            OnClick?.Invoke(_touchableItem);
            Debug.Log(base.name + "OnClick");
            OnPointUp?.Invoke(_touchableItem);
            Debug.Log(base.name + "OnPointUp");
        }

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

            Debug.Log(name + " is touchable set to " + _isTouchable);
        }

        [ContextMenu("ToggleState")]
        public void ToggleState()
        {
            if (_currentState == State.Lock)
                ChangeState(State.UnLock);

            else if (_currentState == State.UnLock) ChangeState(State.Lock);
        }

        public void ForceChangeState(bool isTouchable)
        {
            if (isTouchable)
                ChangeState(State.UnLock);
            else
                ChangeState(State.Lock);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position,_holdDistance);
        }
    }
}