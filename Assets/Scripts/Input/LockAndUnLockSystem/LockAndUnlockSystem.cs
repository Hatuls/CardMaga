using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.Input
{
    [DefaultExecutionOrder(-1000)]
    public class LockAndUnlockSystem : MonoSingleton<LockAndUnlockSystem>
    {
        #region Fields

        [SerializeField] private InputGroup[] _inputGroups;
    
        private  List<TouchableItem> _touchableItems = new List<TouchableItem>();
    
        private List<TouchableItem> _activeTouchableItems = new List<TouchableItem>();
    
        private InputGroup _currentInputGroup;

        private int _inputGroupIndex;

        #endregion

        public override void Awake()
        {
            base.Awake();
            _inputGroupIndex = 0;
            _currentInputGroup = _inputGroups[_inputGroupIndex];
        }

        #region TouchableItemManagment

        public void AddTouchableItemToList(TouchableItem touchableItem)
        {
            if (touchableItem == null)
                return;

            if (!_touchableItems.Contains(touchableItem))
                _touchableItems.Add(touchableItem);

            UpdateInputState();
            // if (FindTouchableItemInCurrentInputIDList(touchableItem))
            // {
            //     AddTouchableItemToActiveList(touchableItem);
            // }
        }

        public void RemoveTouchableItemFromAllLists(TouchableItem touchableItem)
        {
            if (touchableItem == null)
                return;
        
            if (_activeTouchableItems.Contains(touchableItem))
            {
                _activeTouchableItems.Remove(touchableItem);
            }
        
            _touchableItems.Remove(touchableItem);
        }

        #endregion
        
        #region InputGroup

    public void MoveToNextInputGroup()
    {
        _inputGroupIndex++;
        _currentInputGroup = _inputGroups[_inputGroupIndex];
        
        SetNewInputGroup(_currentInputGroup);
    }

    public void SetInputGroup(InputGroup inputGroup)
    {
        SetNewInputGroup(inputGroup);
    }

    private void SetNewInputGroup(InputGroup inputGroup)
    {
        _currentInputGroup = inputGroup;
        
        ResetActiveList();
        FindTouchableItemByID(_currentInputGroup);
        UpdateInputState();
    }
    
    #endregion
    
        private void UpdateInputState()
        {
            FindTouchableItemByID(_currentInputGroup);
            ChangeTouchableItemsState(_activeTouchableItems.ToArray(),true);
        }
        
        private void FindTouchableItemByID(InputGroup inputGroup)
        {
            foreach (var touchableItem in _touchableItems)
            {
                if (touchableItem.InputIdentification == null)
                    continue;
                
                if (_activeTouchableItems.Contains(touchableItem))
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
                    return true;
                }
            }
        
            return false;
        }
        
        private void ResetActiveList()
        {
            _activeTouchableItems.Clear();
        }
        
        #region LockAndUnlockAll
        
        public void ChangeTouchableItemsState(TouchableItem[] touchableItems,bool isTouchable)
        {
            for (int i = 0; i < touchableItems.Length; i++)
            {
                if (touchableItems[i] == null)
                    continue;
                if (!touchableItems[i].gameObject.activeSelf)
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
        
        }
}       
        
        