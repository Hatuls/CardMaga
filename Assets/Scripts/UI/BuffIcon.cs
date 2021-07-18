using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Art;
public class BuffIcon : MonoBehaviour
{
    #region Fields
    BuffIcons? _name;

    [SerializeField]
    protected TextMeshProUGUI _iconText;

    [SerializeField]
   protected Image _background;

    [SerializeField]
    protected Image _decor;

    [SerializeField]
    protected BuffIconSettingsSO _buffIconSettingsSO;

    [SerializeField]
    protected Image _icon;

[SerializeField] protected RectTransform _rectTransform;

    #region Art
    static BuffUIPalette _buffUIPalette;
    static CardIconCollectionSO _cardIconCollection;
    static CardTypePalette _cardTypePalette;
    #endregion


    #endregion
    #region Properties
    public BuffIcons? GetSetName { get => _name; set => _name = value; }
    #endregion
    private void Start()
    {
     _cardIconCollection = ArtSettings.ArtSO.GetSpriteCollections<CardIconCollectionSO>();
       _cardTypePalette = ArtSettings.ArtSO.GetPallette<CardTypePalette>();
        _buffUIPalette = ArtSettings.ArtSO.GetPallette<BuffUIPalette>();
    }


    public virtual void InitIconData(Cards.Card card)
    {

        _icon.sprite = _cardIconCollection.GetSprite(card.GetSetCard.GetBodyPartEnum);

        _background.color = _buffUIPalette.CardDefaultBackground;

        Color clr = _cardTypePalette.GetIconBodyPartColorFromEnum(card.GetSetCard.GetCardTypeEnum);
        _icon.color = clr;


        _decor.color = _cardTypePalette.GetDecorationColorFromEnum(card.GetSetCard.GetCardTypeEnum);
        SetText(card.GetSetCard.GetCardsKeywords[0].GetAmountToApply.ToString());

        _iconText.color = clr;
    }
    public void InitIconData(UIIconSO iconData, int amount)
    {
        if (iconData == null)
        {
            Debug.Log("Error in set Colors");
            return;
        }

        ShowIcon();

        _background.sprite = iconData.GetBackground;
        _background.color = iconData.GetBackgroundColor;

        _decor.sprite = iconData.GetDecor;
        _decor.color = iconData.GetDecorColor;

        _icon.sprite = iconData.GetIcon;
        _icon.color = iconData.GetIconColor;

        SetText(amount.ToString());
    }
    public void ResetEnumType()
    {
             GetSetName = null;
        TweenExitEntrance(false);
    }

    protected void SetText(string Text)
        => _iconText.text = Text;

    public void SetAmount(int amount)
    {
        SetText(amount.ToString());
        if (amount != 0)
          TweenOnUpdateText();
    }
    protected void ShowIcon()
    {
        gameObject.SetActive(true);
        TweenExitEntrance(true);
    }

    protected void TweenOnUpdateText()
    {
        if (gameObject.activeSelf)
        {
        LeanTween.scale(_rectTransform, Vector3.one * _buffIconSettingsSO.ScaleAmount, _buffIconSettingsSO.ScaleEntranceTime).setEase(_buffIconSettingsSO.EntranceTypeTweenType).setOnComplete(() =>
        LeanTween.scale(_rectTransform, Vector3.one, _buffIconSettingsSO.ScaleExitTime).setEase(_buffIconSettingsSO.ExitTypeTweenType)
            );
        }
    }

    protected void TweenExitEntrance(bool isEntering)
    {
        if (isEntering)
        {
            LeanTween.alpha(_rectTransform, 1, _buffIconSettingsSO.ScaleEntranceTime).setEase(_buffIconSettingsSO.EntranceTypeTweenType);
            LeanTween.scale(_rectTransform, Vector3.one, _buffIconSettingsSO.ScaleEntranceTime).setEase(_buffIconSettingsSO.EntranceTypeTweenType);
        }
        else
        {
           LeanTween.alpha(_rectTransform, 0, _buffIconSettingsSO.AlphaExitTime).setEase(_buffIconSettingsSO.ExitTypeTweenType);
           LeanTween.scale(_rectTransform, Vector3.zero, _buffIconSettingsSO.ScaleExitTime).setEase(_buffIconSettingsSO.ExitTypeTweenType).setOnComplete(() => gameObject.SetActive(false));
        }
    }
}
