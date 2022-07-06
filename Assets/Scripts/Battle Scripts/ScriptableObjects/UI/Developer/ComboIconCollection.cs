using Battle.Deck;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "Combo Icons", menuName = "ScriptableObjects/Art/Combo/Go To After Executions Icon ")]
public class ComboIconCollection : ScriptableObject
{


    [SerializeField]
    ColorPerType _backgroundImage;
    [SerializeField]
    ColorPerType _decorImage;
    [InfoBox("0 - AutoActivate\n1 - Hand\n2 - Draw Pile\n3 - Discard Pile")]
    [SerializeField]
    ColorPerType[] _gotoIcons;

    public ColorPerType GetBackGroundImage() => _backgroundImage;
    public ColorPerType GetDecorImage() => _decorImage;
    public ColorPerType GetInnerImage(DeckEnum deck)
    {
        switch (deck)
        {
            case DeckEnum.AutoActivate:
                return _gotoIcons[0];
            case DeckEnum.Hand:
                return _gotoIcons[1];
            case DeckEnum.PlayerDeck:
                return _gotoIcons[2];
            case DeckEnum.Disposal:
                return _gotoIcons[3];
        }
       throw new System.Exception($"ComboIcon Collection : Deck Was not found in the icons\nValue: {deck}");
    }

}
[System.Serializable]
public class ColorPerType
{
    [SerializeField]
    public Sprite Icon;

    [SerializeField]
    [InfoBox("0 - Utility\n1 - Defend\n2 - Attack")]
    private Color[] _colorVariations = new Color[3];


    public Color GetColor(Cards.CardTypeEnum cardType)
    {
        switch (cardType)
        {
            case Cards.CardTypeEnum.Utility:
                return _colorVariations[0];
            case Cards.CardTypeEnum.Defend:
                return _colorVariations[1];
            case Cards.CardTypeEnum.Attack:
                return _colorVariations[2];
            default:
                throw new System.Exception($"Card Type is Not Valid!");
                
        }
    }
}