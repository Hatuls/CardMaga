using Art;
using CardMaga.Card;
using Keywords;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    [SerializeField]
    UnityEvent OnGainArmor;

    #endregion


    #region Properties
    public KeywordTypeEnum KeywordType { get => _name; set => _name = value; }
    #endregion
 
    public virtual void InitIconData(CardData card)
    {
        var art = Factory.GameFactory.Instance.ArtBlackBoard;
        var _cardTypePalette = art.GetPallette<CardTypePalette>();
        _icon.sprite = art.GetSpriteCollections<CardIconCollectionSO>().GetSprite(card.CardSO.BodyPartEnum);

        _background.color = art.GetPallette<BuffUIPalette>().CardDefaultBackground;

        Color clr = _cardTypePalette.GetIconBodyPartColorFromEnum(card.CardSO.CardTypeEnum);
        _icon.color = clr;


        _decor.color = _cardTypePalette.GetDecorationColorFromEnum(card.CardSO.CardTypeEnum);
        SetText(card.CardSO.CardSOKeywords[0].GetAmountToApply.ToString());

        _iconText.color = clr;
    }
    public void InitIconData(UIIconSO iconData, int amount, KeywordTypeEnum buffIcons)
    {


        ShowIcon();
        var art = Factory.GameFactory.Instance.ArtBlackBoard;
        //var buffUIPalette = art.GetPallette<BuffUIPalette>();
        _background.sprite = iconData?.GetBackground;
        // _background.color = iconData.GetBackgroundColor;
        //_background.color = buffUIPalette.CardDefaultBackground;

        _decor.sprite = iconData?.GetDecor;
        //_decor.color = iconData.GetDecorColor;
        //_decor.color = buffUIPalette.CardDefaultDecorateColor;

        _icon.sprite = iconData?.GetIcon;
        //_icon.color = iconData.GetIconColor;
        //_icon.color = buffUIPalette.GetBuffIconFromColor(buffIcons);

        if (iconData.ToShowAmount)
        {
            SetText(amount.ToString());
            _iconText.gameObject.SetActive(true);
        }
        else
        {
            _iconText.gameObject.SetActive(false);
        }
    }
    public void ResetEnumType()
    {
        KeywordType = KeywordTypeEnum.None;
        TweenExitEntrance(false);
    }

    protected void SetText(string Text)
    {
        _iconText.text = Text;

        //_iconText.color = (clr.HasValue)
        //    ? clr.GetValueOrDefault() :  _buffUIPalette.CardDefaultTextColor ;


        //var art = Factory.GameFactory.Instance.ArtBlackBoard;
        //_iconText.color = art.GetPallette<BuffUIPalette>().CardDefaultTextColor;


    }

    public void SetAmount(int amount)
    {
        SetText(amount.ToString());
        if (amount != 0)
        {

            TweenOnUpdateText();
        }
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
        if (gameObject != null && gameObject.activeSelf)
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
