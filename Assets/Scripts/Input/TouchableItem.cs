using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardMaga.Input
{ 
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
        private InputBehaviour _inputBehaviour;
        private bool _isHold;
        private bool _isTouchable;

        public InputBehaviour InputBehaviour => _inputBehaviour;
        public State CurrentState => _currentState;

        protected virtual void Click()
        {
            OnClick?.Invoke(_touchableItem);
            _inputBehaviour.Click();
        }
        
        protected virtual void BeginHold()
        {
            OnBeginHold?.Invoke(_touchableItem);
        }
        
        protected virtual void EndHold()
        {
            OnEndHold?.Invoke(_touchableItem);
        }
        
        protected virtual void Hold()
        {
            OnHold?.Invoke(_touchableItem);
        }
        
        protected virtual void PointDown()
        {
            OnPointDown?.Invoke(_touchableItem);
        }
        
        protected virtual void PointUp()
        {
            OnPointUp?.Invoke(_touchableItem);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isTouchable)
                return;
            
            _isHold = false;
            StartCoroutine(HoldCheck(eventData)); 
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

            int count = 0;
            while (_isHold)
            {
                Hold();
#if UNITY_EDITOR
                if (count % 10 == 0)
                {
                    Debug.Log(base.name + "OnHold");
                }
                count++;

#endif       
                
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
    [System.Serializable]
    public abstract class InputBehaviour
    {
        
        
        public event Action OnClick; 
        public event Action OnBeginHold;
        public event Action OnEndHold;
        public event Action OnHold;
        public event Action OnPointDown;
        public event Action OnPointUp;

        public virtual void Click()
        {
            OnClick?.Invoke();
        }

        public virtual void Hold()
        {
            
        }

        public virtual void BeginHold()
        {
            
        }

        public virtual void EndHold()
        {
            
        }

        public virtual void PointDown()
        {
            
        }

        public virtual void PointUp()
        {
            
        }
    }
}