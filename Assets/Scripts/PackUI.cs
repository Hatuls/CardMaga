using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System;

namespace Rewards.Packs
{

    public class PackUI : MonoBehaviour
    {
     
        PackRewardSO _packRewardSO;
        public static Action<PackRewardSO> OnPackRewardClicked;
            
            
            [TitleGroup("Pack")]

        [TabGroup("Pack/Components", "Images")]
        [SerializeField] Image _backgroundImage;
        [TabGroup("Pack/Components", "Images")]
        [SerializeField] Image _packImage;
        [TabGroup("Pack/Components", "Images")]
        [SerializeField] Image _currencyImage;


        [TabGroup("Pack/Components", "Texts")]
        [SerializeField] TextMeshProUGUI _openText;
        [TabGroup("Pack/Components", "Texts")]
        [SerializeField] TextMeshProUGUI _costText;
        [TabGroup("Pack/Components", "Texts")]
        [SerializeField] TextMeshProUGUI _packNameText;

        public void Init(PackRewardSO packRewardSO)
        {
            _packNameText.text = packRewardSO.PackName;
            _packRewardSO = packRewardSO;
            _costText.text = string.Concat("X", packRewardSO.PurchaseCosts[0].Price);
            _openText.text = "Open X1";
        }


        public void OnOpen()
        {
            OnPackRewardClicked?.Invoke(_packRewardSO);
        }
    }

}