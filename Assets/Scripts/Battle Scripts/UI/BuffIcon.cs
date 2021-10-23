using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Keywords;
public class BuffIcon : MonoBehaviour
{
    #region Fields
    [SerializeField] KeywordTypeEnum _name;

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




    #endregion
    #region Properties
    public KeywordTypeEnum GetSetName { get => _name; set => _name = value; }
    #endregion

    public virtual void InitIconData(Cards.Card card)
    {
        var _cardTypePalette = ArtSettings.CardTypePalette;
        _icon.sprite = ArtSettings.CardIconCollectionSO.GetSprite(card.CardSO.BodyPartEnum);

        _background.color = ArtSettings.BuffUIPalette.CardDefaultBackground;

        Color clr = _cardTypePalette.GetIconBodyPartColorFromEnum(card.CardSO.CardTypeEnum);
        _icon.color = clr;


        _decor.color = _cardTypePalette.GetDecorationColorFromEnum(card.CardSO.CardTypeEnum);
        SetText(card.CardSO.CardSOKeywords[0].GetAmountToApply.ToString());

        _iconText.color = clr;
    }
    public void InitIconData(UIIconSO iconData, int amount , KeywordTypeEnum buffIcons)
    {
        if (iconData == null)
        {
            Debug.Log("Error in set Colors");
            return;
        }

        ShowIcon();
        var buffUIPalette = ArtSettings.BuffUIPalette;
        _background.sprite = iconData.GetBackground;
        // _background.color = iconData.GetBackgroundColor;
        _background.color = buffUIPalette.CardDefaultBackground;

        _decor.sprite = iconData.GetDecor;
        //_decor.color = iconData.GetDecorColor;
        _decor.color = buffUIPalette.CardDefaultDecorateColor;

        _icon.sprite = iconData.GetIcon;
        //_icon.color = iconData.GetIconColor;
        _icon.color = buffUIPalette.GetBuffIconFromColor(buffIcons);
   

        SetText(amount.ToString(), buffUIPalette.CardDefaultTextColor);
    }
    public void ResetEnumType()
    {
        GetSetName =  KeywordTypeEnum.None;
        TweenExitEntrance(false);
    }

    protected void SetText(string Text, Color? clr = null)
    {
         _iconText.text = Text;

        //_iconText.color = (clr.HasValue)
        //    ? clr.GetValueOrDefault() :  _buffUIPalette.CardDefaultTextColor ;



        _iconText.color = ArtSettings.BuffUIPalette.CardDefaultTextColor;


    }

    public void SetAmount(int amount)
    {
        SetText(amount.ToString());
        if (amount != 0)
            TweenOnUpdateText();
        else if (_name != KeywordTypeEnum.Shield)
            TweenExitEntrance(false);
    }
    protected void ShowIcon()
    {
        gameObject.SetActive(true);
        TweenExitEntrance(true);
    }

    protected void TweenOnUpdateText()
    {
        if (gameObject  != null&& gameObject.activeSelf)
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
