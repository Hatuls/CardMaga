using CardMaga.Input;
using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonGenaric<T> : TouchableItem<T> where T : MonoBehaviour
{
    [SerializeField] private InputIdentificationSO _inputID;
    [SerializeField] private Image _ButtonImage;

    private Color _baseColor;
    private Color _pressColor;

    protected override void Awake()
    {
        base.Awake();
        _baseColor = _ButtonImage.color;
        _pressColor = new Color(_baseColor.r, _baseColor.g, _baseColor.b, 0.5f);
    }

    public override InputIdentificationSO InputIdentification
    {
        get => _inputID;
    }

    protected override void PointDown()
    {
        base.PointDown();
        _ButtonImage.color = _pressColor;
    }

    protected override void PointUp()
    {
        base.PointUp();
        _ButtonImage.color = _baseColor;
    }
}
