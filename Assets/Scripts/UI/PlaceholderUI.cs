
//using UnityEngine;

//[System.Serializable]
//public class PlaceholderUI : MonoBehaviour 
//{
//    #region Fields

//    [SerializeField] PlaceHolderSlotUI[] _placeHolderSlots;
//    PlaceHolderSlotUI _currentSlot;

//    public ref PlaceHolderSlotUI GetCurrentSlot => ref _currentSlot;
//    public ref PlaceHolderSlotUI[] GetPlaceHolderSlots => ref _placeHolderSlots;
//    public PlaceHolderSlotUI CurrentSlot { set => _currentSlot = value; }
//    [SerializeField]
//    ArtSO _artso;

//    #endregion
//    #region Events
//    [SerializeField] Unity.Events.UIColorPaletteSOEvent _resetColorEvent;
//    #endregion

//    #region Public Methods
//    public void SetCurrentSlot(PlaceHolderSlotUI clicked) => _currentSlot = clicked;
//    public void ResetSpecificSlot(int index)
//    {
//        if (index >= 0 && index < _placeHolderSlots.Length)
//            _placeHolderSlots[index].ResetSlot(_artso.UIColorPalette);
//    }
//    public void ResetDetectingCards()//check for missing Later
//    {
//    //    _resetColorEvent?.Raise(Battles.UI.CardUIManager.Instance.GetUIColorPalette);
//        for (int i = 0; i < _placeHolderSlots.Length; i++)
//        {
//            if (!_placeHolderSlots[i].IsHoldingCard)
//            {
//                _placeHolderSlots[i].ResetSlot(_artso.UIColorPalette);
//            }
//        }
//    }
//    public void ResetPlaceHolders()
//    {
//        _resetColorEvent?.Raise(_artso.UIColorPalette);
//    }
//    public PlaceHolderSlotUI GetTouchedSlot(int Index)
//    {

//        if (Index < _placeHolderSlots.Length && Index >= 0)
//            return _placeHolderSlots[Index];

//        Debug.LogError("PlaceHolderUI : Slot Was Touched but index was out of raged!");
//        return null;
//    }

//    internal void Init()
//    {
//        _resetColorEvent?.Raise(_artso.UIColorPalette);
//    }

//    public PlaceHolderSlotUI TryGetEmptyPlaceHolderSlotUI()
//    {
//        for (int i = 0; i < _placeHolderSlots.Length; i++)
//        {
//            if (_placeHolderSlots[i].IsHoldingCard == false)
//                return _placeHolderSlots[i];
//        }

//        return null;
//    }
//    #endregion
//}
