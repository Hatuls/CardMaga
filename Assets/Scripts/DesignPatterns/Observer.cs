using System.Collections.Generic;

namespace DesignPattern
{




    public class Observer : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        public void Notify(IObserver theOneWhoDidIt)
        {
            for (int i = 0; i < _observers.Count; i++)
                _observers[i].OnNotify(theOneWhoDidIt);
        }

        public void Subscribe(IObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void UnSubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }
    }

    public class Observer<T> : ISubject<T>
    {
        private List<IObserver<T>> _observers = new List<IObserver<T>>();
        public void Notify(T data)
        {
            for (int i = 0; i < _observers.Count; i++)
                _observers[i].OnNotify(data);

        }

        public void Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void UnSubscribe(IObserver<T> observer)
        {
            _observers.Remove(observer);
        }
    }


    public interface IObserver<T>
    {
        void OnNotify(T subject);
    }
    public interface IObserver
    {
        void OnNotify(IObserver Myself);
    }
    public interface ISubject<T>
    {
        void Notify(T data);
        void Subscribe(IObserver<T> observer);
        void UnSubscribe(IObserver<T> observer);
    }  
    public interface ISubject
    {
        void Notify(IObserver observer);
        void Subscribe(IObserver observer);
        void UnSubscribe(IObserver observer);
    }


}