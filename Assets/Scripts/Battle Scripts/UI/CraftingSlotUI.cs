using UnityEngine;
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
    Vector3 originalPos;

    [SerializeField] RectTransform _rectTransform;

    [SerializeField] float crossFadeAnimationTime;
    [SerializeField] Animator _anim;

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
    public float GetIconImageOpacity => _iconImage.color.a;
    public float GetBackGroundOpacity => _backgroundImage.color.a;
    public float GetDecorationOpacity => _decorImage.color.a;
    #endregion
    private void Start()
    {
        originalPos = _rectTransform.localPosition;
    }
    public void PlayAnimation(int animation) => _anim.CrossFade(animation, crossFadeAnimationTime);
    public void InitPlaceHolder(Cards.CardTypeData cardType)
    {
        if (cardType.BodyPart == Cards.BodyPartEnum.Empty)
        {

        }
        if (cardType.BodyPart == Cards.BodyPartEnum.None)
        {

        }

        if (cardType == null )
        {
            ResetSlotUI();

        }
    
        else
        InitPlaceHolder(
                cardType.CardType,
                        Factory.GameFactory.Instance.ArtBlackBoard.GetSpriteCollections<CardIconCollectionSO>().GetSprite(cardType.BodyPart)                     
                      );
        _iconImage.gameObject.SetActive(cardType.BodyPart != Cards.BodyPartEnum.Empty);

    }
    public void InitPlaceHolder(Cards.CardTypeEnum cardType, Sprite icon )
    {
        SetIconImage(icon);
        SetColors(cardType);


    }
    public void ActivateGlow(bool toActivate)
    {
        if (_glowImage != null && _glowImage.gameObject.activeSelf != toActivate)
        {
            _glowImage.gameObject.SetActive(toActivate);
        }
    }
    void SetIconImage(Sprite img)
    {
        if (img != null)
            _iconImage.sprite = img;
    }
    public void ResetSlotUI()
    {
        var artBoard = Factory.GameFactory.Instance.ArtBlackBoard;
        if (_glowImage.gameObject.activeSelf)
            _glowImage.gameObject.SetActive(false);

        var craftingUIPalete = artBoard.GetPallette<CraftingUIPalette>();
        _iconImage.color = Color.clear;
        _decorImage.color = craftingUIPalete.SlotDecorationColor;
        _backgroundImage.color = craftingUIPalete.SlotBackgroundColor;

        _iconImage.sprite = null;
    }
    void SetColors(Cards.CardTypeEnum cardType)
    {
        var artBoard = Factory.GameFactory.Instance.ArtBlackBoard;
       
        _backgroundImage.color = artBoard.GetPallette<CraftingUIPalette>().SlotBackgroundColor;
        var cardTypePallete = artBoard.GetPallette<CardTypePalette>();

        _iconImage.color = cardTypePallete.GetIconBodyPartColorFromEnum(cardType);
        _decorImage.color = cardTypePallete.GetDecorationColorFromEnum(cardType);
    }

    public void MoveLocation(Vector2 startPosition, float leantweenTime)
    {
        _rectTransform.localPosition = startPosition;
        LeanTween.move(_rectTransform, originalPos, leantweenTime);

    }
}


