using UnityEngine;

[CreateAssetMenu(fileName = "UIIconOS", menuName = "ScriptableObjects/UI/UIIconSO")]

public class UIIconSO : ScriptableObject
{
    #region Fields
    [SerializeField]
    Sprite _icon;
    [SerializeField]
    Color _detectedBackGround;
    [SerializeField]
    Sprite _background;
    [SerializeField]
    Sprite _decor;
    [SerializeField]
    Color _iconColor;
    [SerializeField]
    Color _decorColor;
    [SerializeField]
    Color _backgroundColor;
    #endregion
    #region Properties
    public Sprite GetIcon => _icon;
    public Sprite GetBackground => _background;
    public Sprite GetDecor => _decor;
    public Color GetIconColor => _iconColor;
    public Color GetDecorColor => _decorColor;
    public Color GetBackgroundColor => _backgroundColor;
    public Color GetDetectedBackgroundColor => _detectedBackGround;
    #endregion
}
