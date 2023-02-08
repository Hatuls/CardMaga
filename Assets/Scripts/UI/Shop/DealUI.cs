using CardMaga.Input;
using CardMaga.Rewards;
using CardMaga.Rewards.Bundles;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.MetaUI.Shop
{

public class DealUI : MonoBehaviour
{
    [SerializeField]
    private BundleRewardFactorySO[] _deals;
        [SerializeField]
        private Button _buyMeButton;

        public IEnumerable<ResourcesCost> DealsCost
    {
        get
        {
            for (int i = 0; i < _deals.Length; i++)
                yield return _deals[i].ResourcesCost;
        }
    }

    public void Init()
    {
   //  new ShopButtonVisualizer(_buyMeButton, DealsCost)
    }

}

    
    public class ShopButtonVisualizer
    {
        private readonly Button button;
        private readonly IEnumerable<ResourcesCost> dealsCost;

        public ShopButtonVisualizer(Button button, IEnumerable<ResourcesCost> dealsCost)
        {
            this.button = button;
            this.dealsCost = dealsCost;
        }

        public void AssignText()
        {




//            button.SetButtonText();


        }
    }
}