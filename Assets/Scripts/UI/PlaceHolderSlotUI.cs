using UnityEngine;
using UnityEngine.UI;
using Battles.UI;
using UnityEngine.EventSystems;
public class PlaceHolderSlotUI : MonoBehaviour ,IDropHandler
{
    #region Events
    [SerializeField] Unity.Events.PlaceHolderSlotUIEvent _onClickEvent;
    [SerializeField] Unity.Events.PlaceHolderSlotUIEvent _setCardUI;
    [SerializeField] Unity.Events.PlaceHolderSlotUIEvent _onSlotInteract;
    [SerializeField] Unity.Events.SoundsEvent _playSound;
    #endregion
    #region Fields

    [SerializeField] Image _iconImage;
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _decorImage;
    bool _holdCard;
    [SerializeField] int _SlotID;

    [HideInInspector]
    [SerializeField] RectTransform _rectTransform;
    #endregion
    #region Properties
    public int GetSlotID => _SlotID;
    public RectTransform RectTransform => _rectTransform;

    public bool IsHoldingCard { get => _holdCard; set => _holdCard = value; }
    #endregion
    private void Start()
    {
        Debug.Log($"{_decorImage}, {_iconImage}, {_backgroundImage},{GetSlotID }");
        _rectTransform = GetComponent<RectTransform>();
    }
    public void InitCard( UIColorPaletteSO uiColorPalette,Cards.CardTypeEnum cardType,Sprite background,
        Sprite decor, Sprite icon)
    {
        IsHoldingCard = true;
        SetDecorImage(decor);
        SetBackgroundImage(background);
        SetIconImage(icon);
        SetColors(ref uiColorPalette,cardType);
        _playSound?.Raise(SoundsNameEnum.PlaceCard);
      
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
    public void ResetSlot( UIColorPaletteSO palette)
    {
        if(palette == null)
        {
            Debug.LogError("Error in ResetSlot");
            return;
        }
        IsHoldingCard = false;

        var color = palette.GetBackgroundColor;
        color.a = 0;
        _iconImage.sprite = null;
        _iconImage.color = color;

        color = palette.GetDefaultSlotColor;
        color.a = palette.GetFullOpacity;
        _decorImage.color = color;
    }
    void SetColors(ref UIColorPaletteSO palette,Cards.CardTypeEnum cardType)
    {
        if (palette == null)
        {
            Debug.LogError("Error in SetSlotData");
            return;
        }
        var color = palette.GetBackgroundColor;
        color.a = palette.GetSlotsOpacity/100;
        _backgroundImage.color = color;

        var colorPalette = palette.GetCardColorType(cardType);
        color = colorPalette.GetTopColor;
        color.a = palette.GetFullOpacity/100;
        _iconImage.color = color;

        _decorImage.color = color;
    }
    public void OnPointClick()
    {
        _onClickEvent?.Raise(this);
    }

    public void BeginDrag()
    {
        if (IsHoldingCard == false)
            return;

     //   CardUIManager.Instance.SetCardUI(this);
        _setCardUI?.Raise(this);
    }
    public void EndDrag()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (CardUIManager.Instance == null)
        {
            Debug.LogError("CardUIManager is Null");
            return;
        }
        Debug.Log("SlotUI was Touched");

      //  CardUIManager.Instance.OnSlotInteract(this);
        _onSlotInteract?.Raise(this);
        //CardUIManager.Instance.IsTryingToPlace = true;
        //_holderManager.CurrentSlot = this; 

    }
}