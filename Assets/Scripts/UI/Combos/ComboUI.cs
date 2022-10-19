using System;
using Battle.Combo;
using CardMaga.UI.Combos;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

public class ComboUI : MonoBehaviour , IShowableUI , IPoolable<ComboUI> , IVisualAssign<Combo>
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

    public void AssingVisual(Combo data)
    {
        _comboVisual.Init(data);
    }
}
