using System;
using Battle.Combo;
using CardMaga.Tools.Pools;
using CardMaga.UI.ScrollPanel;
using UnityEngine;
namespace CardMaga.UI.Combos
{ 
public class ComboUI : MonoBehaviour , IShowableUI , IPoolableMB<ComboUI> , IVisualAssign<ComboData>
{
    public event Action<ComboUI> OnDisposed;
    
    [SerializeField] private ComboVisualHandler _comboVisual;

    public void Dispose()
    {
        _comboVisual.Dispose();
    }

    public void Show()
    {
        Init();
    }

    public void Init()
    {
        gameObject.SetActive(true);
    }

    public void AssingVisual(ComboData data)
    {
        _comboVisual.Init(data);
    }
}

}