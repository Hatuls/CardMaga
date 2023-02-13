using CardMaga.Input;
using CardMaga.Rewards;
using CardMaga.Rewards.Bundles;
using CardMaga.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.MetaUI.Shop
{


    public interface IDealable
    {
        IEnumerable<string> ProductsName { get; }
        IReadOnlyList<BundleRewardFactorySO> Products { get; }
        IReadOnlyList<ResourcesCost> Costs { get; }
    }
    public class PackDealUI : MonoBehaviour , IDealable
    {
        public static event Action<PackDealUI> OnPackInfoPopupRequired;
        public static event Action OnPackInfoPopupFinished;
        public static event Action<IDealable> OnTryBuy;


        [SerializeField]
        private BundleRewardFactorySO[] _deals;
        [SerializeField]
        private Button _buyMeButton;

        [SerializeField,Sirenix.OdinInspector.ReadOnly]
        private RarityChanceCardContainer[] _packRewards;
        private ShopButtonVisualizer _shopButtonVisualizer;


        public IReadOnlyList<BundleRewardFactorySO> Products => _deals;
        public RarityChanceCardContainer[] RarirtyContainer => _packRewards;
        public IReadOnlyList<ResourcesCost> Costs { get; private set; }
        private IEnumerable<ResourcesCost> DealsCost
        {
            get
            {
                for (int i = 0; i < _deals.Length; i++)
                    yield return _deals[i].ResourcesCost;
            }
        }

        public IEnumerable<string> ProductsName
        { 
            get
            {
                for (int i = 0; i < Products.Count; i++)
                    yield return Products[i].Name;
            }
        }

       

        public void Awake()
        {
            Init();
            SetText();
        }

        private void Init()
        {
            _shopButtonVisualizer = new ShopButtonVisualizer(_buyMeButton);
            Costs = GetCostsFromBundle();
        }
        public void HidePopUp()
            => OnPackInfoPopupFinished?.Invoke();
        public void ShowInfoPopup()
        => OnPackInfoPopupRequired?.Invoke(this);
        public void TryBuy()
        {
            OnTryBuy?.Invoke(this);
        }

        private IReadOnlyList<ResourcesCost> GetCostsFromBundle()
        {
            List<ResourcesCost> resourcesCosts = new List<ResourcesCost>();

            foreach (var resource in DealsCost)
            {
                if (ContainCost(resource.CurrencyType, out ResourcesCost cost))
                    cost.AddAmount(resource.Amount);

                else
                    resourcesCosts.Add(new ResourcesCost(resource.CurrencyType, resource.Amount));
            }

            return resourcesCosts;

            bool ContainCost(CurrencyType type, out ResourcesCost cost)
            {
                int count = resourcesCosts.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (resourcesCosts[i].CurrencyType == type)
                        {
                            cost = resourcesCosts[i];
                            return true;
                        }
                    }
                }
                cost = null;
                return false;
            }
        }

        [Sirenix.OdinInspector.Button]
        private void SetText()
        {
#if UNITY_EDITOR
            Init();
#endif

            _shopButtonVisualizer.AssignText(GetCostsFromBundle());
        }


#if UNITY_EDITOR
        [SerializeField,Header("Editor:"),Sirenix.OdinInspector.OnValueChanged("SetStatistic"),Tooltip("Will set the statistic of the pack rewards")]
        private CardsPackRewardFactorySO _packRewardsInfo;
        private void SetStatistic()
        {
            if (_packRewardsInfo == null)
                _packRewards = null;
            else
                _packRewards = _packRewardsInfo.RarirtyContainer;
        }
#endif
    }


    public class ShopButtonVisualizer
    {
        private readonly Button button;
        public ShopButtonVisualizer(Button button)
        {
            this.button = button;
        }

        public void AssignText(IReadOnlyList<ResourcesCost> dealsCost)
        {
            IReadOnlyList<ResourcesCost> resourcesCosts = dealsCost;
            string finalText = string.Empty;
            for (int i = 0; i < resourcesCosts.Count; i++)
            {
                string amount = Mathf.RoundToInt(resourcesCosts[i].Amount).ToString();
                finalText += amount.AddImageInFrontOfText(((int)resourcesCosts[i].CurrencyType) - 1);
            }

            button.SetButtonText(finalText);

        }

    }
}