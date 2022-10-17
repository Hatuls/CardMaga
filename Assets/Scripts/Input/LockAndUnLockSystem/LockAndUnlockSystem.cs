using System;
using System.Collections.Generic;
using Battle;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.Input
{ 
    [Serializable]
public class LockAndUnlockSystem : MonoSingleton<LockAndUnlockSystem>, ISequenceOperation<IBattleManager>
{
    [SerializeField] private InputGroup[] _inputGroups;

    private InputGroup _currentInputGroup;
    
    [SerializeField] TouchableItem[] _touchableItems;
    
    [ContextMenu("Test FindTouchableItemByID")]
    private void FindTouchableItem()
    {
        _touchableItems = FindObjectsOfType<TouchableItem>();

        foreach (var item in _touchableItems)
        {
            Debug.Log(item);
        }
    }

    public void Init()
    {
        BattleManager.Register(this,OrderType.After);
    }

    private void SetInputState()
    {
        ChangeTouchableItemsState(FindTouchableItemByID(_currentInputGroup).ToArray(),true);
    }

    private List<TouchableItem> FindTouchableItemByID(InputGroup inputGroup)
    {
        List<TouchableItem> output = new List<TouchableItem>(inputGroup.InputIDs.Length);

        foreach (var touchableItem in _touchableItems)
        {
            if (touchableItem.InputIdentification == null)
                continue;

            foreach (var inputId in inputGroup.InputIDs)
            {
                if (touchableItem.InputIdentification == inputId)
                {
                    output.Add(touchableItem);
                    break;
                }
            }
        }

        return output;
    }

    #region LockAndUnlockAll
    
    public void ChangeTouchableItemsState(TouchableItem[] touchableItems,bool isTouchable)
    {
        for (int i = 0; i < touchableItems.Length; i++)
        {
            if (touchableItems[i] == null)
                continue;

            if (isTouchable)
                touchableItems[i].UnLock();
            else
                touchableItems[i].Lock();
            
        }
    }

    public void ChangeAllTouchableItemsState(bool isTouchable)
    {
        for (int i = 0; i < _touchableItems.Length; i++)
        {
            if (_touchableItems[i] == null)
                continue;

            if (isTouchable)
                _touchableItems[i].UnLock();
            else
                _touchableItems[i].Lock();
            
        }
    }

    public void ChangeAllTouchableItemsStateExcept(bool isTouchable,params TouchableItem[] exceptTouchableItem)
    {
        for (int i = 0; i < _touchableItems.Length; i++)
        {
            bool isExcept = false;
            
            if (_touchableItems[i] == null)
                continue;

            for (int j = 0; j < exceptTouchableItem.Length; j++)
            {
                isExcept = _touchableItems[i] == exceptTouchableItem[j];
                
                if (isExcept)
                    break;  
            }
            
            if (!isExcept)
                continue;  
            
            if (isTouchable)
                _touchableItems[i].UnLock();
            else
                _touchableItems[i].Lock();
        }
    }

    #endregion

    public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
    {
        _currentInputGroup = _inputGroups[0];
        FindTouchableItem();
        SetInputState();
    }

    public int Priority { get; }
}
}

