using Battles.Deck;
using Art;
using UnityEngine;

namespace Battles.UI
{
    public class PlaceHolderHandler : MonoBehaviour
    {
        //reset Slots UI
        //give refrence to crafring slot Data
        //get icon from slot data
        //set slots UI by Data and index
        //arr of crafting slot UI
        #region Fields


        [SerializeField] CraftingSlotUI[] _CraftingSlotsUIArr;
        [SerializeField] RectTransform _firstSlotTransform;
        [SerializeField] float _leanTweenTime;

        [SerializeField] float _offsetPos =1 ;
          static  PlaceHolderHandler _instance;

        [SerializeField] GameObject _buttonGlow;



        #region Events
        #endregion

        #region Properties
        public CraftingSlotUI[] GetCraftingSlotsUIArr => _CraftingSlotsUIArr;
        #endregion
        internal void ResetSlotsDetection()
        {
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                if (_CraftingSlotsUIArr[i] != null)
                    _CraftingSlotsUIArr[i].ActivateGlow(false);
            }
            LeanTween.alpha(_instance._CraftingSlotsUIArr[_instance._CraftingSlotsUIArr.Length - 1].RectTransform, 0, 0.001f);

            if (_buttonGlow!= null && _buttonGlow.activeSelf != false)
                _buttonGlow?.SetActive(false);
            
            
        }

        internal void MarkSlotsDetected()
        {
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                if (_CraftingSlotsUIArr[i] != null && DeckManager.GetCraftingSlots.GetDeck[i]!= null)
                    _CraftingSlotsUIArr[i].ActivateGlow(true);
            }

            if (_buttonGlow != null && _buttonGlow.activeSelf != true)
                _buttonGlow?.SetActive(true);
        }
        #endregion
        private void Awake()
        {
            _instance = this;
        }
        private void Start()
        {

            ResetAllSlots();
         
        }
        public static void ResetAllSlots()
        {
            if(_instance._CraftingSlotsUIArr == null)
            {
                Debug.LogError("Error in ResetAllSlots");
                return;
            }
            for (int i = 0; i < _instance._CraftingSlotsUIArr.Length; i++)
            {
                _instance._CraftingSlotsUIArr[i].SlotID = i + 1;
                ResetPlaceHolderUI(i);
            }

            LeanTween.alpha(_instance._CraftingSlotsUIArr[_instance._CraftingSlotsUIArr.Length - 1].RectTransform, 0, 0.001f);
        }
        public static void ResetPlaceHolderUI(int index)
        {
     

            if (index < 0  || index >= _instance._CraftingSlotsUIArr.Length)
            {
                Debug.LogError("Error in ResetCraftingSlot");
                return;
            }
            _instance._CraftingSlotsUIArr[index].ResetSlotUI();
        }
        public static void ChangeSlotsPos(Cards.Card[] cards )
        {
            // check thread possability for color check


            var type = cards[0].GetSetCard.GetCardType._cardType;

            _instance._CraftingSlotsUIArr[0].Appear(_instance._leanTweenTime, type);
            for (int i = 0; i < _instance._CraftingSlotsUIArr.Length; i++)
            {
                _instance._CraftingSlotsUIArr[i].MovePlaceHolderSlot(ref _instance.GetRectTransform(i), _instance._CraftingSlotsUIArr.Length - 1 == i? 0: _instance._offsetPos);
                _instance._CraftingSlotsUIArr[i].MoveDown(_instance._leanTweenTime);
            }
            if (cards[_instance._CraftingSlotsUIArr.Length - 1] != null)
            {

            type = cards[_instance._CraftingSlotsUIArr.Length - 1].GetSetCard.GetCardType._cardType;

            _instance._CraftingSlotsUIArr[_instance._CraftingSlotsUIArr.Length - 1].Disapear(_instance._leanTweenTime,type);
            }
        }
        public ref RectTransform GetRectTransform(int index)
        {
            if(index == 0)
            {
                return ref _firstSlotTransform;
            }
            else
            {

                return ref _CraftingSlotsUIArr[index -1].RectTransform;
            }
        }
        public  static void PlaceOnPlaceHolder(int index, Cards.Card cardCache)
        {
            if (cardCache == null )
            {
                ResetPlaceHolderUI(index);
                return;
            }
            //_instance._CraftingSlotsUIArr[index].InitPlaceHolder(_instance._artSO.UIColorPalette, cardCache.GetSetCard.GetCardTypeEnum,
            //   _instance._artSO.IconCollection.GetSprite(cardCache.GetSetCard.GetBodyPartEnum));
            
            _instance._CraftingSlotsUIArr[index].InitPlaceHolder(cardCache.GetSetCard.GetCardType);
        }
        //public void PlaceOnPlaceHolder(PlaceHolderSlotUI interactedSlot, Cards.Card cardCache)
        //{
        //    interactedSlot.InitCraftSlot(_artSO.UIColorPalette, cardCache.GetSetCard.GetCardTypeEnum,
        //        _artSO.DefaultSlotSO.GetBackground, _artSO.DefaultSlotSO.GetDecor, _artSO.IconCollection.GetSprite(cardCache.GetSetCard.GetBodyPartEnum));

        //    CardUIManager.Instance.GetClickedCardUI.SetActive(false);

        //    CardUIManager.Instance.GetClickedCardUI = null;

        //    InputManager.Instance.RemoveObjectFromTouch();
        //}
    }
}

