using CardMaga.Input;
using UnityEngine;

public class Button : TouchableItem
{
    [SerializeField] private InputIdentificationSO _inputID;
    
    public override InputIdentificationSO InputIdentification
    {
        get => _inputID;
    }
    
}
