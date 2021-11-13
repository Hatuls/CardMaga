
using System.Collections.Generic;
using UnityEngine;
namespace DesignPattern
{

    [CreateAssetMenu(fileName = "Observer", menuName = "ScriptableObjects/DesignPatterns/Observer")]
    public class Observer : ScriptableObject
    {

    }
    public interface IObserver
    {
        void Update(ISubject subject);
    }

    public interface ISubject
    {
   
        void Notify();
        void Subscribe(IObserver observer);
        void UnSubscribe(IObserver observer);
    }
}