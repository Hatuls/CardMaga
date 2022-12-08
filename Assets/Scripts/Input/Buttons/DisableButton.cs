using UnityEngine;
using UnityEngine.UI;
using Button = CardMaga.Input.Button;

public class DisableButton : Button
{
    [SerializeField] private Image _renderer;
    [Header("Sprites")]
    [SerializeField] private Sprite _onPress;
    [SerializeField] private Sprite _onIdle;
    [SerializeField] private Sprite _onDisable;

    protected override void Awake()
    {
        base.Awake();
        OnPointDown += PointerDown;
        OnPointUp += PointerUp;
    }

    private void PointerDown()
    {
        _renderer.sprite = _onPress;
    }
    
    private void PointerUp()
    {
        _renderer.sprite = _onIdle;
    }
    [ContextMenu("Disable")]
    public void Disable()
    {
        Lock();
        _renderer.sprite = _onDisable;
    }
    [ContextMenu("Enable")]
    public void Enable()
    {
        UnLock();
        _renderer.sprite = _onIdle;
    }
}
