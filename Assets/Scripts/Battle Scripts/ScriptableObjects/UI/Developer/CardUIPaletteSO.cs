using UnityEngine;
using Sirenix.OdinInspector;
using CardMaga.Card;

[CreateAssetMenu(fileName = "Card UI Paletta", menuName = "ScriptableObjects/Art/UI/Card UI")]

public class CardUIPaletteSO : ScriptableObject
{

    #region Fields

    [TitleGroup("Card UI Colors", "", TitleAlignments.Centered, BoldTitle = true)]

    [TabGroup("Card UI Colors/Colors", "Default Colors")]

    [SerializeField]
    Color _backgroundColor;
    [TabGroup("Card UI Colors/Colors", "Default Colors")]
    [SerializeField]
    Color _defaultSlotColor;

    [TabGroup("Card UI Colors/Colors", "Colors Per Card")]
    [SerializeField]
    [Tooltip("Attack 0, Defence 1, Utility 2")]
    ColorUI[] _colorPalettes;



    [TabGroup("Card UI Colors/Colors", "Opacity On Images")]
    [SerializeField]
    [Range(0, 1)]
    float _backgroundOpacity;

    [TabGroup("Card UI Colors/Colors", "Opacity On Images")]
    [SerializeField]
    [Range(0, 1)]
    float _decorationOpacity;


      [TabGroup("Card UI Colors/Colors", "Opacity On Images")]
    [SerializeField]
    [Range(0, 1)]
    float _fullOpacity;



    [TabGroup("Card UI Colors/Colors", "Opacity On Images")]
    [SerializeField]
    [Range(0, 1)]
    float _itemOpacity;

    [TabGroup("Card UI Colors/Colors", "Opacity On Images")]
    [SerializeField]
    [Range(0, 1)]
    float _cardStripesOpacity;

    [TabGroup("Card UI Colors/Colors", "Opacity On Images")]
    [SerializeField]
    [Range(0, 1)]
    float _cardEarsOpacity;
    #endregion

    #region Properties
    public Color BackGroundColor => _backgroundColor;
    public float GetCardStripesOpacity => _cardStripesOpacity;
    public float BackGroundOpacity => _backgroundOpacity;
    public float GetCardEarsOpacity => _cardEarsOpacity;
    public float GetFullOpacity => _fullOpacity;
    #endregion

    #region Functions
    public ColorUI GetCardColorType(CardTypeEnum cardType)
    {
        if (_colorPalettes == null || _colorPalettes.Length != 3)
        {
            Debug.LogError("Error in GetCardColorType");
            return null;
        }
        switch (cardType)
        {
            default:
            case CardTypeEnum.Attack:
                return _colorPalettes[0];
            case CardTypeEnum.Defend:
                return _colorPalettes[1];
            case CardTypeEnum.Utility:
                return _colorPalettes[2];
        }
    }
    #endregion

}