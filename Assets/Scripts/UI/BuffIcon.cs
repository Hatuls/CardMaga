using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    #region Fields
    BuffIcons? _name;

    [SerializeField]
    Image _background;

    [SerializeField]
    Image _decor;

    [SerializeField]
    Image _icon;
    #endregion
    #region Properties
    public BuffIcons? GetSetName { get => _name; set => _name = value; }
    #endregion
    public void InitIconData(UIIconSO iconData)
    {
        if (iconData == null)
        {
            Debug.Log("Error in set Colors");
            return;
        }
        _background.sprite = iconData.GetBackground;
        _background.color = iconData.GetBackgroundColor;

        _decor.sprite = iconData.GetDecor;
        _decor.color = iconData.GetDecorColor;

        _icon.sprite = iconData.GetIcon;
        _icon.color = iconData.GetIconColor;
    }
    public void ResetEnumType()
    {
        GetSetName = null;
        gameObject.SetActive(false);
    }
}
