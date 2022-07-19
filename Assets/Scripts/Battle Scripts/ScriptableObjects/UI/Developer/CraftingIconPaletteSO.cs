using UnityEngine;
using Sirenix.OdinInspector;
using CardMaga.Card;

[CreateAssetMenu(fileName = "Crafting_Icon_Paletta", menuName = "ScriptableObjects/Art/UI/Crafting Icon")]

public class CraftingIconPaletteSO : ScriptableObject
{

    #region Fields

    [TitleGroup("Slots Colors","", TitleAlignments.Centered,BoldTitle = true)]

    [TabGroup("Slots Colors/Colors", "Default Colors")]
  
    [SerializeField]
    Color _backgroundColor;
    [TabGroup("Slots Colors/Colors", "Default Colors")]
    [SerializeField]
    Color _defaultSlotColor;

    [TabGroup("Slots Colors/Colors", "Colors Per Card")]
    [SerializeField]
    [Tooltip("Attack 0, Defence 1, Utility 2")]
    ColorUI[] _colorPalettes;



    [TabGroup("Slots Colors/Colors", "Opacity On Images")]
    [SerializeField]
    [Range(0,1)]
    float _backgroundOpacity;

    [TabGroup("Slots Colors/Colors", "Opacity On Images")]
    [SerializeField]

    [Range(0,1)]
    float _decorationOpacity;

    [TabGroup("Slots Colors/Colors", "Opacity On Images")]

    [SerializeField]
    [Range(0,1)]
    float _itemOpacity;

    #endregion

    #region Properties
    public Color BackGroundColor => _backgroundColor;
    public float BackGroundOpacity => _backgroundOpacity;
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
