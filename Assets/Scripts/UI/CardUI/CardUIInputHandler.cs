using CardMaga.UI.Card;
using UnityEngine;

namespace CardMaga.Input
{
    public class CardUIInputHandler : TouchableItem<CardUI>
    {
        [SerializeField] private InputIdentificationSO _inputIdentificationSo;
        
        public override InputIdentificationSO InputIdentification
        {
            get => _inputIdentificationSo;
        }
    }
}