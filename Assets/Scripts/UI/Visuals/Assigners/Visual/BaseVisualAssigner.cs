using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public abstract class BaseVisualAssigner<T> :BaseVisualHandler<T>
    {
        protected const int ZERO = 0;
    }
}
