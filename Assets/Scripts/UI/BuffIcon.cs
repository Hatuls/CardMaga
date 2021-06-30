using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    BuffIconSettingsSO _buffIconSettingsSO;

    [SerializeField]
    Image _icon;

[SerializeField]    RectTransform _rectTransform;

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

    private void SetText(string Text)
        => _iconText.text = Text;

    public void SetAmount(int amount)
    {
        SetText(amount.ToString());
        if (amount != 0)
          TweenOnUpdateText();
    }
    private void ShowIcon()
    {
        gameObject.SetActive(true);
        TweenExitEntrance(true);
    }

    private void TweenOnUpdateText()
    {
        if (gameObject.activeSelf)
        {
        LeanTween.scale(_rectTransform, Vector3.one * 1.2f, _buffIconSettingsSO.ScaleEntranceTime).setEase(_buffIconSettingsSO.EntranceTypeTweenType).setOnComplete(() =>
        LeanTween.scale(_rectTransform, Vector3.one, _buffIconSettingsSO.ScaleExitTime).setEase(_buffIconSettingsSO.ExitTypeTweenType)
            );
        }
    }

    private void TweenExitEntrance(bool isEntering)
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
