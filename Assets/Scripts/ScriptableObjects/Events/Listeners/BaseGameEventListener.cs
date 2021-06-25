
using UnityEngine;
namespace Unity.Events
{
    [System.Serializable]
    public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour, IGameEventListener<T>
        where E : BaseGameEvent<T> where UER : UnityEngine.Events.UnityEvent<T>
    {
        [SerializeField] private E _gameEvent;
        [SerializeField] private UER _unityEventResponse;
        public E GameEvent { get => _gameEvent; set => _gameEvent = value; }

        private void OnEnable()
        {
            if (GameEvent == null)
                return;

            GameEvent.RegisterListener(this);
        }
        private void OnDisable()
        {
            if (GameEvent == null)
                return;

            GameEvent.UnRegisterListener(this);
        }

        public void OnEventRaised(T item)
        {
            if (_unityEventResponse != null)
            {
                _unityEventResponse.Invoke(item);
            }
        }
        [System.Serializable]
        public class EventSlot : IGameEventListener<T>
        {
            [SerializeField] private E _gameEvent;
            [SerializeField] private UER _unityEventResponse;
            public E GameEvent { get => _gameEvent; set => _gameEvent = value; }
            public void OnDisable()
            {

                if (GameEvent == null)
                    return;

                GameEvent.UnRegisterListener(this);

            }
            public void OnEnable()
            {
                if (GameEvent == null)
                    return;

                GameEvent.RegisterListener(this);
            }
            public void OnEventRaised(T item)
            {
                if (_unityEventResponse != null)
                {
                    _unityEventResponse.Invoke(item);
                }
            }
        }
    }




}
