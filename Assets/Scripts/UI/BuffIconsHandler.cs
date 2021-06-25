﻿using UnityEngine;
using UnityEngine.Events;
public class RemoveUIIcon: UnityEvent<bool ,BuffIcons>{}
public class BuffIconsHandler : MonoBehaviour
{
    #region Fields
    [SerializeField]
    BuffIconCollectionSO _buffCollection;

    [SerializeField]
    BuffIcon[] _buffSlots;
    #endregion
    private void Start()
    {
        Init();
    }
    private void Init()//check for bugs
    {
        if(_buffSlots != null && _buffSlots.Length>0)
        {
            for (int i = 0; i < _buffSlots.Length; i++)
            {
                if(_buffSlots[i] != null && _buffSlots[i].gameObject.activeSelf)
                {
                    _buffSlots[i].ResetEnumType();
                }
            }
        }
    }
    BuffIcon GetFreeSlot()
    {
        if (_buffSlots != null && _buffSlots.Length > 0)
        {
            for (int i = 0; i < _buffSlots.Length; i++)
            {
                if (_buffSlots[i] != null && !_buffSlots[i].gameObject.activeSelf)
                {
                    return _buffSlots[i];
                }
            }
        }
        Debug.LogError("Error in GetBuffIcon");
        return null;
    }
    public void SetBuffIcon(BuffIcons icon)
    {
        if(CheckForDuplicates(icon))
        {
            GetDuplicate(icon);
            //set text
            return;
        }
        var buffSlot = GetFreeSlot();
        buffSlot.gameObject.SetActive(true);
        buffSlot.GetSetName = icon;
        buffSlot.InitIconData(_buffCollection.GetIconData(icon));
    }
    public void RemoveBuffIcon(BuffIcons icon)
    {
        if (_buffSlots != null && _buffSlots.Length > 0)
        {
            for (int i = 0; i < _buffSlots.Length; i++)
            {
                if (_buffSlots[i].GetSetName == icon)
                {
                    _buffSlots[i].ResetEnumType();
                    OrderArray();
                    return;
                }
            }
        }
    }
    private bool CheckForDuplicates(BuffIcons icon)
    {
        if (_buffSlots != null && _buffSlots.Length > 0)
        {
            for (int i = 0; i < _buffSlots.Length; i++)
            {
                if (_buffSlots[i].GetSetName == icon)
                {
                    return true;
                }
            }
        }
        return false;
    }
    void OrderArray()
    {
        for (int i = 0; i < _buffSlots.Length-1; i++)
        {
            if (_buffSlots[i].gameObject.activeSelf==false)
            {
                for (int j = i + 1; j < _buffSlots.Length; j++)
                {
                    if(_buffSlots[j].gameObject.activeSelf)
                    {
                        _buffSlots[i].InitIconData(_buffCollection.GetIconData(_buffSlots[j].GetSetName.GetValueOrDefault()));
                        _buffSlots[j].ResetEnumType();
                        break;
                    }
                    if(j == _buffSlots.Length-1)
                    {
                        return;
                    }
                }
            }
        }
    }
    BuffIcon GetDuplicate(BuffIcons icon)
    {
        if (_buffSlots != null && _buffSlots.Length > 0)
        {
            for (int i = 0; i < _buffSlots.Length; i++)
            {
                if (_buffSlots[i].GetSetName == icon)
                {
                    return _buffSlots[i];
                }
            }
        }
        Debug.LogError("Error in GetDuplicate");
        return null;
    }
    //accses to all icons and colors
    //Array of Icons
    //void(Enum)add
    //void(Enum)remove
}
