using System.Collections.Generic;
using Battle;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.Input
{
    public class LockAndUnlockSystem : MonoBehaviour, ISequenceOperation<IBattleManager>
    {
        #region Singelton

        private static LockAndUnlockSystem _instance;

        public static LockAndUnlockSystem Instance
        {
            get => _instance;
        }

        #endregion

        #region Fields

        [SerializeField] private InputGroup[] _inputGroups;
    
        private readonly List<TouchableItem> _touchableItems = new List<TouchableItem>();
    
        private List<TouchableItem> _activeTouchableItems = new List<TouchableItem>();
    
        private InputGroup _currentInputGroup;

        #endregion
    

    public void Awake()
    {
        Debug.Log(isActiveAndEnabled);
        if (_instance == null)
            _instance = this;
        else
            Destroy(this);
        
        BattleManager.Register(this,OrderType.After);
    }

    public void AddTouchableItemToList(TouchableItem touchableItem)
    {
        if (touchableItem == null)
            return;
        
        _touchableItems.Add(touchableItem);

        if (FindTouchableItemInCurrentInputIDList(touchableItem))
        {
            UpdateInputState();
        }
    }
    
    public void RemoveTouchableItemToList(TouchableItem touchableItem)
    {
        if (touchableItem == null)
            return;
        
        _touchableItems.Remove(touchableItem);

        if (_activeTouchableItems.Contains(touchableItem))
        {
            _activeTouchableItems.Remove(touchableItem);
        }
    }

    private void SetInputState()
    {
        FindTouchableItemByID(_currentInputGroup);
        ChangeTouchableItemsState(_activeTouchableItems.ToArray(),true);
    }

    private void UpdateInputState()
    {
        ChangeTouchableItemsState(_activeTouchableItems.ToArray(),true);
    }

    private void FindTouchableItemByID(InputGroup inputGroup)
    {
        foreach (var touchableItem in _touchableItems)
        {
            if (touchableItem.InputIdentification == null)
                continue;

            foreach (var inputId in inputGroup.InputIDs)
            {
                if (touchableItem.InputIdentification == inputId)
                {
                    _activeTouchableItems.Add(touchableItem);
                    break;
                }
            }
        }
    }

    private bool FindTouchableItemInCurrentInputIDList(TouchableItem touchableItem)
    {
        if (_currentInputGroup == null)
            return false;
        
        foreach (var inputID in _currentInputGroup.InputIDs)
        {
            if (inputID == touchableItem.InputIdentification)
            {
                _activeTouchableItems.Add(touchableItem);
                return true;
            }
        }

        return false;
    }

    #region LockAndUnlockAll
    
    public void ChangeTouchableItemsState(TouchableItem[] touchableItems,bool isTouchable)
    {
        for (int i = 0; i < touchableItems.Length; i++)
        {
            if (touchableItems[i] == null)
                continue;
            if (isTouchable && touchableItems[i].IsTouchable)
                continue;
            if (!isTouchable && !touchableItems[i].IsTouchable)
                continue;
            
            if (isTouchable)
                touchableItems[i].UnLock();
            else
                touchableItems[i].Lock();
        }
    }

    public void ChangeAllTouchableItemsState(bool isTouchable)
    {
        for (int i = 0; i < _touchableItems.Count; i++)
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
        for (int i = 0; i < _touchableItems.Count; i++)
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
        SetInputState();
    }

    public int Priority { get; }
}
}

