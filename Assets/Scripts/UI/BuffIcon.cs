using UnityEngine;
using UnityEngine.UI;
using TMPro;


[RequireComponent    (typeof(SpriteRenderer)    , typeof(TextMeshProUGUI)    )]
public class BuffIcon : MonoBehaviour
{
    #region Fields
    BuffIcons? _name;

    [SerializeField] 
    TextMeshProUGUI _iconText;

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
    public void InitIconData(UIIconSO iconData, int amount)
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

        AddAmount(amount);
    }
    public void ResetEnumType()
    {
   
        GetSetName = null;
        gameObject.SetActive(false);
    }

    private void SetText(string Text)
        => _iconText.text = Text;

    public void AddAmount(int amount)
    {
        SetText(amount.ToString());
    }
}
