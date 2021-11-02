using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public abstract class TabAbst : MonoBehaviour,IOpenCloseUIHandler
    {
        public abstract void Open();
        public abstract void Close();

    }
}
