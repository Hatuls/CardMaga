using System;
using CardMaga.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : TouchableItem
{
    public event Action<bool> OnValueChanage;
    
    [Header("Toggle configuration")]
    [SerializeField] private bool _startState;

    [Header("Visual configuration")]
    [SerializeField] private Image _buttonImage;
    [SerializeField] TextMeshProUGUI _buttonText;

    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;

    [SerializeField] private string _onText;
    [SerializeField] private string _offText;

    private bool _isOn;
    
    public bool ButtonState
    {
        get => _isOn;
    }

    protected override void Awake()
    {
        base.Awake();
        SetVisual(_startState);
    }

    private void SetVisual(bool state)
    {
        if (state)
            SetToOnState();
        else
            SetToOffState();
    }

    private void SetToOnState()
    {
        _isOn = true;
        _buttonText.text = _onText;
        _buttonImage.sprite = _onSprite;
    }

    private void SetToOffState()
    {
        _isOn = false;
        _buttonText.text = _offText;
        _buttonImage.sprite = _offSprite;
    }

    private void ToggleState()
    {
        if (_isOn)
            SetToOffState();
        else
            SetToOnState();
        
        OnValueChanage?.Invoke(_isOn);
    }


    public void SetToggleState(bool state)
    {
        _isOn = state;
        SetVisual(state);
    }
    public override void Click()
    {
        base.Click();
        ToggleState();
    }
}
