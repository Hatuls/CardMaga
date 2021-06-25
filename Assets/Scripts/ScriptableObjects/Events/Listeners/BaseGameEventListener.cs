
using UnityEngine;
namespace Unity.Events
{

    #region One Generic
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
    #endregion

    #region Two Generics
    [System.Serializable]
    public abstract class BaseGameEventListener<T1,T2, E, UER> : MonoBehaviour, IGameEventListener<T1,T2>
    where E : BaseGameEvent<T1,T2> where UER : UnityEngine.Events.UnityEvent<T1,T2>
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

        public void OnEventRaised(T1 item, T2 item2)
        {
            if (_unityEventResponse != null)
            {
                _unityEventResponse.Invoke(item,item2);
            }
        }





        [System.Serializable]
        public class EventSlot : IGameEventListener<T1, T2>
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
            public void OnEventRaised(T1 item, T2 item2)
            {
                if (_unityEventResponse != null)
                {
                    _unityEventResponse.Invoke(item, item2);
                }
            }
        }
    

    }
    #endregion

    #region Three Generics
    [System.Serializable]
    public abstract class BaseGameEventListener<T1, T2, T3, E, UER> : MonoBehaviour, IGameEventListener<T1, T2, T3>
  where E : BaseGameEvent<T1, T2, T3> where UER : UnityEngine.Events.UnityEvent<T1, T2, T3>
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

        public void OnEventRaised(T1 item, T2 item2 , T3 item3)
        {
            if (_unityEventResponse != null)
            {
                _unityEventResponse.Invoke(item, item2, item3);
            }
        }





        [System.Serializable]
        public class EventSlot : IGameEventListener<T1, T2, T3>
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
            public void OnEventRaised(T1 item, T2 item2, T3 item3)
            {
                if (_unityEventResponse != null)
                {
                    _unityEventResponse.Invoke(item, item2,item3);
                }
            }
        }


    }
    #endregion
}
