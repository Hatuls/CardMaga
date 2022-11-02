namespace Unity.Events
{
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T item);
    } 
    public interface IGameEventListener<T1 , T2>
    {
        void OnEventRaised(T1 item1, T2 item2);
    }
    public interface IGameEventListener<T1, T2,T3>
    {
        void OnEventRaised(T1 item1, T2 item2, T3 item3);
    }
}