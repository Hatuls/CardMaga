using System;
using CardMaga.Input;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : TouchableItem
{
    public event Action<bool> OnValueChanage;
    
    [SerializeField] private InputIdentificationSO _inputIdentification;
    [Header("Toggle configuration")]
    [SerializeField] private bool _startState;

    private bool _isOn;
    
    public bool ButtonState
    {
        get => _isOn;
    }    

    public override InputIdentificationSO InputIdentification
    {
        get => _inputIdentification;
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
    }

    private void SetToOffState()
    {
        _isOn = false;
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
