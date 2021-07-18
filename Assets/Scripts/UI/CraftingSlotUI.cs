﻿using UnityEngine;
using UnityEngine.UI;
using Art;
public class CraftingSlotUI : MonoBehaviour
{
    //icon moves to new icon position
    #region Events
    [SerializeField] Unity.Events.PlaceHolderSlotUIEvent _setCardUI;
    #endregion
    #region Fields
    [SerializeField] Image _glowImage;
    [SerializeField] Image _iconImage;
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _decorImage;
    [SerializeField] int _SlotID;
    [SerializeField] RectTransform _iconHolder;
    Vector3 slotPos;
       [HideInInspector]
    [SerializeField] RectTransform _rectTransform;



    #region Art
    [SerializeField]
    ArtSO art;

    static CraftingUIPalette _craftingUIPalette;
    static CardTypePalette _cardTypePalette;
    static CardIconCollectionSO _cardIconCollection;
    #endregion

    #endregion
    #region Properties
    public int SlotID { get => _SlotID; set => _SlotID = value; }

    public ref RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return ref _rectTransform;
        }
    }
    public RectTransform GetIconHolderRectTransform => _iconHolder;
    public float GetIconImageOpacity => _iconImage.color.a;
    public float GetBackGroundOpacity => _backgroundImage.color.a;
    public float GetDecorationOpacity => _decorImage.color.a;
    #endregion
    private void Start()
    {
        Debug.Log($"{_decorImage}, {_iconImage}, {_backgroundImage},{SlotID }");
        slotPos = GetIconHolderRectTransform.anchoredPosition3D;
      
       _craftingUIPalette = ArtSettings.ArtSO.GetPallette<CraftingUIPalette>();
       _cardTypePalette = ArtSettings.ArtSO.GetPallette<CardTypePalette>();
       _cardIconCollection = ArtSettings.ArtSO._iconCollection;
    }

   public void InitPlaceHolder( Cards.CardType cardType)
    {
        InitPlaceHolder(
                cardType._cardType,
                      _cardIconCollection.GetSprite(cardType._bodyPart)
                 );
    }
    public void InitPlaceHolder(Cards.CardTypeEnum cardType,Sprite icon)
    {

        SetIconImage(icon);
        SetColors(cardType);
    }

    public void ActivateGlow (bool toActivate)
    {
        if (_glowImage != null && _glowImage.gameObject.activeSelf != toActivate)
        {
            _glowImage.gameObject.SetActive(toActivate);
        }
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
    public void SetGlowImageColor()
    {
     //   _glowImage.color = _craftingUIPalette.SlotGlowColor;
    }
    public void ResetSlotUI()
    {

        if (_craftingUIPalette == null)
        {
            Debug.LogError("Error in ResetSlot");
            return;
        }

        if (_glowImage.gameObject.activeSelf)
            _glowImage.gameObject.SetActive(false);


        _iconImage.color = Color.clear;
        _decorImage.color = _craftingUIPalette.SlotDecorationColor;
        _backgroundImage.color = _craftingUIPalette.SlotBackgroundColor;

        _iconImage.sprite = null;

    }
    void SetColors(Cards.CardTypeEnum cardType)
    {
        if (_craftingUIPalette == null || _cardTypePalette == null)
        {
            Debug.LogError("Error in SetSlotData");
            return;
        }

        _backgroundImage.color = _craftingUIPalette.SlotBackgroundColor;

        //var colorPalette = cardTypePalette.GetCardColorType(cardType);
        //color = colorPalette.GetTopColor;
        //color.a = palette.GetFullOpacity/100;

        _iconImage.color = _cardTypePalette.GetIconBodyPartColorFromEnum(cardType);
        _decorImage.color = _cardTypePalette.GetDecorationColorFromEnum(cardType);
    }
    public void MovePlaceHolderSlot(ref RectTransform moveTo, float offset)
    {
        Vector3 v3 = moveTo.rect.center;
        v3.x += moveTo.rect.width;
        //v3.y = moveTo.anchoredPosition3D.y;
        //v3.x = 0;
        //v3.z = 0;
        GetIconHolderRectTransform.anchoredPosition3D = v3;
    }
    public void MoveDown(float time) =>  LeanTween.move(GetIconHolderRectTransform, slotPos, time);
    public void Appear(float time ,Cards.CardTypeEnum type )
    {




        LeanTween.alpha(_iconImage.rectTransform, 0, 0.001f);
        LeanTween.alpha(_decorImage.rectTransform, 0, 0.001f);
        LeanTween.alpha(_backgroundImage.rectTransform, 0, 0.001f);


        LeanTween.alpha(_iconImage.rectTransform, _cardTypePalette.GetIconBodyPartColorFromEnum(type).a, time);
        LeanTween.alpha(_decorImage.rectTransform, _cardTypePalette.GetDecorationColorFromEnum(type).a, time);
        LeanTween.alpha(_backgroundImage.rectTransform, _cardTypePalette.GetBackgroundColorFromEnum(type).a, time);


    }
    public void Disapear(float time, Cards.CardTypeEnum type)
    {
        LeanTween.alpha(_iconImage.rectTransform, _cardTypePalette.GetIconBodyPartColorFromEnum(type).a, 0.001f);
        LeanTween.alpha(_decorImage.rectTransform, _cardTypePalette.GetDecorationColorFromEnum(type).a, 0.001f);
        LeanTween.alpha(_backgroundImage.rectTransform, _cardTypePalette.GetBackgroundColorFromEnum(type).a, 0.001f);


        LeanTween.alpha(_decorImage.rectTransform, 0, time);
        LeanTween.alpha(_backgroundImage.rectTransform, 0, time);
        LeanTween.alpha(_iconImage.rectTransform, 0, _iconImage.sprite != null ? time : 0.001f);
    }
    public void Disapear(float time)
    {   
        _iconImage.color = Color.clear;
        _decorImage.color = _craftingUIPalette.SlotDecorationColor;
        _backgroundImage.color = _craftingUIPalette.SlotBackgroundColor;
        LeanTween.alpha(_iconImage.rectTransform, _iconImage.color.a, 0.001f);
        LeanTween.alpha(_decorImage.rectTransform, _decorImage.color.a, 0.001f);
        LeanTween.alpha(_backgroundImage.rectTransform, _backgroundImage.color.a, 0.001f);


        LeanTween.alpha(_decorImage.rectTransform, 0, time);
        LeanTween.alpha(_backgroundImage.rectTransform, 0, time);
        LeanTween.alpha(_iconImage.rectTransform, 0, _iconImage.sprite != null ? time : 0.001f);
    }



}

