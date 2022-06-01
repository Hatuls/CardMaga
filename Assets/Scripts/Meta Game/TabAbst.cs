using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public abstract class TabAbst : MonoBehaviour, IOpenCloseUIHandler
    {
        [SerializeField,EventsGroup]
       protected UnityEvent OnOpen;
        [SerializeField,EventsGroup]
        protected UnityEvent OnClose;
        public abstract void Open();
        public abstract void Close();

    }
}
