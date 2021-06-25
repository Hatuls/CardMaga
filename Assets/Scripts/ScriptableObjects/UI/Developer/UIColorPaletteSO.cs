using UnityEngine;
[CreateAssetMenu(fileName = "UIColorPalette",menuName = "ScriptableObjects/UI/UIColorPalette")]
public class UIColorPaletteSO : ScriptableObject
{
    #region Fields
    [SerializeField]
    [Tooltip("Attack 0, Defence 1, Utility 2")]
    ColorUI[] _colorPalettes;
    [Space]
    [SerializeField]
    Color _backgroundColor;
    [SerializeField]
    Color _defaultSlotColor;
    [SerializeField]
    float _cardAreaOpacity;
    [SerializeField]
    float _cardStripesOpacity;
    [SerializeField]
    float _cardEarsOpacity;
    [SerializeField]
    float _fullOpacity;
    [SerializeField]
    float _slotsOpacity;
    [SerializeField]
    float _buffsOpacity;
    [SerializeField]
    float _relicsOpacity;
    #endregion
    #region Propetries
    public float GetCardAreaOpacity => _cardAreaOpacity;
    public float GetCardStripesOpacity => _cardStripesOpacity;
    public float GetCardEarsOpacity => _cardEarsOpacity;
    public float GetFullOpacity => _fullOpacity;
    public float GetSlotsOpacity => _slotsOpacity;
    public float GetBuffsOpacity => _buffsOpacity;
    public float GetRelicOpacity => _relicsOpacity;
    public Color GetBackgroundColor => _backgroundColor;
    public Color GetDefaultSlotColor => _defaultSlotColor;
    #endregion
    public ColorUI GetCardColorType(Cards.CardTypeEnum cardType)
    {
        if(_colorPalettes == null|| _colorPalettes.Length != 3)
        {
            Debug.LogError("Error in GetCardColorType");
            return null;
        }
        switch (cardType)
        {
            default:
            case Cards.CardTypeEnum.Attack:
                return _colorPalettes[0];
            case Cards.CardTypeEnum.Defend:
                return _colorPalettes[1];
            case Cards.CardTypeEnum.Utility:
                return _colorPalettes[2];
        }
    }
}
[System.Serializable]
public class ColorUI
{
    #region Fields
    [SerializeField]
    Color _middleColor;
    [SerializeField]
    Color _topColor;
    #endregion
    #region Properties
    public Color GetMiddleColor => _middleColor;
    public Color GetTopColor => _topColor;
    #endregion
}
