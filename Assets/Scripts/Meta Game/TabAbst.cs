using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public abstract class TabAbst : MonoBehaviour, IOpenCloseUIHandler
    {
        [SerializeField]
        UnityEvent OnOpen;
        [SerializeField]
        UnityEvent OnClose;
        public abstract void Open();
        public abstract void Close();

    }
}
