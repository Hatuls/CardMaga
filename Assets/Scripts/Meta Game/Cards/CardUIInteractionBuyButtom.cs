using UnityEngine;
using UnityEngine.UI;

public class CardUIInteractionBuyButtom : CardUIInteractionButton
{
    [SerializeField]
    Image _icon;
    public void SetImage(Sprite img)
    {
        _icon.sprite = img;
    }
#if UNITY_EDITOR
    protected override void AssingParams()
    {
        _icon = GetComponentInChildren<Image>();
        base.AssingParams();
    }
#endif
}
