using System;
using UnityEngine;

public abstract class NotificationUIElement : MonoBehaviour
{
    public event Action OnSetDirty;
    
    public event Action OnSetClean;
    
    [SerializeField] private NotificationUIElement[] _children;
    
    private bool _isDirty;

    private bool _isBranchDirty;

    public void SetDirty()
    {
        _isDirty = true;
        UpdateBranch(_isDirty);
        OnDirty();
        OnSetDirty?.Invoke();
    }

    public void Clean()
    {
        _isDirty = false;
        UpdateBranch(_isDirty);
        OnClean();
        OnSetClean?.Invoke();
    }

    private void UpdateBranch(bool isDirty)
    {
        for (int i = 0; i < _children.Length; i++)
        {
            if (isDirty)
            {
                _children[i].SetDirty();
            }
            else
            {
                _children[i].Clean();
            }
        }
    }

    protected abstract void OnDirty();
    protected abstract void OnClean();
}
