﻿using UnityEngine;
using UnityEngine.UI;
public class PlaceHolderSlotUI : MonoBehaviour
{
    //icon moves to new icon position
    #region Events
    [SerializeField] Unity.Events.PlaceHolderSlotUIEvent _setCardUI;
    #endregion
    #region Fields

    [SerializeField] Image _iconImage;
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _decorImage;
    [SerializeField] int _SlotID;
    [SerializeField] RectTransform _iconHolder;
    Vector3 slotPos;
       [HideInInspector]
    [SerializeField] RectTransform _rectTransform;
    #endregion
    #region Properties
    public int SlotID { get => _SlotID; set => _SlotID = value; }

    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }
    public RectTransform GetIconHolderRectTransform => _iconHolder;

    #endregion
    private void Start()
    {
        Debug.Log($"{_decorImage}, {_iconImage}, {_backgroundImage},{SlotID }");
        slotPos = GetIconHolderRectTransform.anchoredPosition3D;
    }

   
    public void InitPlaceHolder( UIColorPaletteSO uiColorPalette,Cards.CardTypeEnum cardType,Sprite background,
        Sprite decor, Sprite icon)
    {
        SetDecorImage(decor);
        SetBackgroundImage(background);
        SetIconImage(icon);
        SetColors(ref uiColorPalette,cardType);
    }
    void SetDecorImage(Sprite img)
    {
        if (img != null)
        {
            _decorImage.sprite = img;
        }
    }
    void SetBackgroundImage(Sprite img)
    {
        if (img != null)
        {
            _backgroundImage.sprite = img;
        }
    }
    void SetIconImage(Sprite img)
    {
        if (img != null)
        {
            _iconImage.sprite = img;
        }
    }
    public void ResetSlotUI( UIColorPaletteSO palette)
    {
        if(palette == null)
        {
            Debug.LogError("Error in ResetSlot");
            return;
        }
        var color = palette.GetBackgroundColor;
        color.a = 0;
        _iconImage.sprite = null;
        _iconImage.color = color;

        color = palette.GetDefaultSlotColor;
        color.a = palette.GetFullOpacity;
        _decorImage.color = color;

        color= palette.GetBackgroundColor;
        color.a = palette.GetSlotsOpacity / 100;
        _backgroundImage.color = color;
    }
    void SetColors(ref UIColorPaletteSO palette,Cards.CardTypeEnum cardType)
    {
        if (palette == null)
        {
            Debug.LogError("Error in SetSlotData");
            return;
        }
        var color = palette.GetBackgroundColor;
        color.a = palette.GetSlotsOpacity /100;
        _backgroundImage.color = color;

        var colorPalette = palette.GetCardColorType(cardType);
        color = colorPalette.GetTopColor;
        color.a = palette.GetFullOpacity/100;
        _iconImage.color = color;

        _decorImage.color = color;
    }
    public void MovePlaceHolderSlot(RectTransform moveTo, float time)
    {
        GetIconHolderRectTransform.anchoredPosition3D = moveTo.anchoredPosition3D;
    }
    public void MoveDown(float time) =>  LeanTween.move(GetIconHolderRectTransform, slotPos, time);
    public void Appear(float time ,  UIColorPaletteSO palette)
    {
        LeanTween.alpha(_iconImage.rectTransform, 0, 0.001f);
        LeanTween.alpha(_decorImage.rectTransform, 0, 0.001f);
        LeanTween.alpha(_backgroundImage.rectTransform, 0, 0.001f);


        LeanTween.alpha(_iconImage.rectTransform, palette.GetFullOpacity / 100, time);
        LeanTween.alpha(_decorImage.rectTransform, palette.GetFullOpacity / 100, time);
        LeanTween.alpha(_backgroundImage.rectTransform, palette.GetSlotsOpacity / 100, time);

       
    }
    public void Disapear(float time, UIColorPaletteSO palette)
    {

        LeanTween.alpha(_iconImage.rectTransform, palette.GetFullOpacity / 100, 0.001f);
        LeanTween.alpha(_decorImage.rectTransform, palette.GetFullOpacity / 100, 0.001f);
        LeanTween.alpha(_backgroundImage.rectTransform, palette.GetSlotsOpacity / 100, 0.001f);


        LeanTween.alpha(_iconImage.rectTransform, 0, _iconImage.sprite != null ? time : 0.001f);

        LeanTween.alpha(_decorImage.rectTransform, 0, time);
        LeanTween.alpha(_backgroundImage.rectTransform, 0, time);
    }


    /*
     *     [SerializeField] Image _iconImage;
             [SerializeField] Image _backgroundImage;
            [SerializeField] Image _decorImage;
     * 
     * 
     * 
     */
    //public void OnPointClick()
    //{
    //    _onClickEvent?.Raise(this);
    //}

    //public void BeginDrag()
    //{
    //    if (IsHoldingCard == false)
    //        return;

    // //   CardUIManager.Instance.SetCardUI(this);
    //    _setCardUI?.Raise(this);
    //}
    //public void EndDrag()
    //{

    //}

    //public void OnDrop(PointerEventData eventData)
    //{
    //    if (CardUIManager.Instance == null)
    //    {
    //        Debug.LogError("CardUIManager is Null");
    //        return;
    //    }
    //    Debug.Log("SlotUI was Touched");

    //  //  CardUIManager.Instance.OnSlotInteract(this);
    //    _onSlotInteract?.Raise(this);
    //    //CardUIManager.Instance.IsTryingToPlace = true;
    //    //_holderManager.CurrentSlot = this; 

    //}
}