using System;
using UnityEngine;

public abstract class BaseNotificationUIElement : MonoBehaviour
{
    public event Action OnSetDirty;
    
    public event Action OnSetClean;
    
    [SerializeField] private BaseNotificationUIElement[] _children;
    
    private bool _isDirty;

    private bool _isBranchDirty;

    public void SetDirty()
    {
        _isDirty = true;
        UpdateBranch(_isDirty);
        Dirty();
        OnSetDirty?.Invoke();
    }

    public void SetClean()
    {
        _isDirty = false;
        UpdateBranch(_isDirty);
        Clean();
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
                _children[i].SetClean();
            }
        }
    }

    protected abstract void Dirty();
    protected abstract void Clean();
}
