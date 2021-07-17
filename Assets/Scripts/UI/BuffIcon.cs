using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cards;

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

    #endregion
    #region Properties
    public BuffIcons? GetSetName { get => _name; set => _name = value; }
    #endregion

    public virtual void InitIconData(Cards.Card card , ArtSO artSO)
    {
        _decor.sprite = artSO.DefaultSlotSO.GetDecor;

        _background.sprite = artSO.DefaultSlotSO.GetBackground;

        _icon.sprite = artSO.IconCollection.GetSprite(card.GetSetCard.GetBodyPartEnum);

        var uiColorPalette = artSO.UIColorPalette;
        var color = uiColorPalette.GetBackgroundColor;
        color.a = uiColorPalette.GetSlotsOpacity / 100;
        _background.color = color;

        var colorPalette = uiColorPalette.GetCardColorType(card.GetSetCard.GetCardTypeEnum);
        color = colorPalette.GetTopColor;
        color.a = uiColorPalette.GetFullOpacity / 100;
        _icon.color = color;

        _decor.color = color;
        SetText(card.GetSetCard.GetCardsKeywords[0].GetAmountToApply.ToString());
        _iconText.color = color;
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
public class EnemyIcon : BuffIcon
{
    public override void InitIconData(Card card, ArtSO artSO)
    {
        _decor.sprite = artSO.DefaultSlotSO.GetDecor;

        _background.sprite = artSO.DefaultSlotSO.GetBackground;

        _icon.sprite = artSO.IconCollection.GetSprite(card.GetSetCard.GetBodyPartEnum);

        var uiColorPalette = artSO.UIColorPalette;
        var color = uiColorPalette.GetBackgroundColor;
        color.a = uiColorPalette.GetSlotsOpacity / 100;
        _background.color = color;

        var colorPalette = uiColorPalette.GetCardColorType(card.GetSetCard.GetCardTypeEnum);
        color = colorPalette.GetTopColor;
        color.a = uiColorPalette.GetFullOpacity / 100;
        _icon.color = color;

        _decor.color = color;
        SetText(card.GetSetCard.GetCardsKeywords[0].GetAmountToApply.ToString());
        _iconText.color = color;
    }
}