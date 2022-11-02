using System.Collections.Generic;

namespace DesignPattern
{
    public class Observer<T1,T2> : ISubject<T1,T2>
    {
        private List<IObserver<T1,T2>> _observers = new List<IObserver<T1, T2>>();

        public void Notify(T1 data, T2 Data)
        {
            for (int i = 0; i < _observers.Count; i++)
                _observers[i].OnNotify(data,Data);
        }

        public void Subscribe(IObserver<T1, T2> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void UnSubscribe(IObserver<T1, T2> observer)
        {
            _observers.Remove(observer);
        }
    }



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
    public interface IObserver<T1,T2>
    {
        void OnNotify(T1 data, T2 secondData);
    }

    public interface IObserver<T>
    {
        void OnNotify(T subject);
    }
    public interface IObserver
    {
        void OnNotify(IObserver Myself);
    }
    public interface ISubject<T1, T2>
    {
        void Notify(T1 data, T2 Data);
        void Subscribe(IObserver<T1,T2> observer);
        void UnSubscribe(IObserver<T1,T2> observer);
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