using System;
using CardMaga.Input;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : TouchableItem
{
    public event Action<bool> OnValueChanage;
    
    [Header("Toggle configuration")]
    [SerializeField] private bool _startState;

    [Header("Visual configuration")] [SerializeField]
    private Image _button;
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;

    private bool _isOn;
    
    public bool ButtonState
    {
        get => _isOn;
    }

    protected override void Awake()
    {
        base.Awake();
        
        if (_startState)
            SetToOnState();
        else
            SetToOffState();
    }

    private void SetToOnState()
    {
        _isOn = true;

        _button.sprite = _onSprite;
    }

    private void SetToOffState()
    {
        _isOn = false;

        _button.sprite = _offSprite;
    }

    private void ToggleState()
    {
        if (_isOn)
            SetToOffState();
        else
            SetToOnState();
        
        OnValueChanage?.Invoke(_isOn);
    }

    protected override void Click()
    {
        base.Click();
        ToggleState();
    }
}
