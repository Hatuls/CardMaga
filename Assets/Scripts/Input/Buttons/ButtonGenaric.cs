using System.Collections;
using System.Collections.Generic;
using CardMaga.Input;
using UnityEngine;

public class ButtonGenaric : TouchableItem<ButtonGenaric>
{
    [SerializeField] private InputIdentificationSO _inputID;

    public override InputIdentificationSO InputIdentification
    {
        get => _inputID;
    }
}
