using CardMaga.Tools.Pools;
using CardMaga.UI;
using UnityEngine;

public interface IPoolableUIElement<T> : IUIElement , IPoolableMB<T> where T : MonoBehaviour
{
}
