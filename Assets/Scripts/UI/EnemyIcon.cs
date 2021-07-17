using Cards;

public class EnemyIcon : BuffIcon
{
    public override void InitIconData(Card card, ArtSO artSO)
    {
        _decor.sprite = artSO.EnemyIcon.DecorateImage;

        _background.sprite = artSO.EnemyIcon.BackGroundImage;

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