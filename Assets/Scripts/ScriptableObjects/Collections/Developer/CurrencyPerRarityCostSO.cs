using Account.GeneralData;
using CardMaga.Card;
using CardMaga.Rewards;
using CardMaga.Rewards.Bundles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurrencyPerRarityCostSO : ScriptableObject
{
    [SerializeField]
    RarityLevelCostsPerCurrency[] _rarityLevelCostsPerCurrencies;

    private RarityLevelCostsPerCurrency GetRarityLevelCostByCurrency(CurrencyType currencyType)
        => _rarityLevelCostsPerCurrencies.First(x => x.CurrencyType == currencyType);

    private RarityLevelCosts GetRarityLevelCost(RarityLevelCostsPerCurrency rarityLevelCostsPerCurrency, RarityEnum rarity)
    => rarityLevelCostsPerCurrency.RarityCosts.First(x => x.Rarity == rarity);

    private int GetCurrentCost(RarityLevelCosts rarityLevelCosts, int level)
        => rarityLevelCosts.Costs[level];

    public ResourcesCost GetCardCostPerCurrencyAndCardCore(CardCore cardCore, CurrencyType currencyType)
    {
        RarityLevelCostsPerCurrency levelCostsByCurrency = GetRarityLevelCostByCurrency(currencyType);
        var currentCostByLevel = GetRarityLevelCost(levelCostsByCurrency, cardCore.CardSO.Rarity);
        return new ResourcesCost(currencyType, GetCurrentCost(currentCostByLevel, cardCore.Level));
    }



#if UNITY_EDITOR
    public void Init(string[] csv)
    {
        const int CHIP_COST = 0;
        const int GOLD_COST = 1;
        List<RarityLevelCostsPerCurrency> master = new List<RarityLevelCostsPerCurrency>();
        List<RarityLevelCosts> goldLevelCosts = new List<RarityLevelCosts>();
        List<RarityLevelCosts> chipLevelCosts = new List<RarityLevelCosts>();
        List<int> goldCosts = new List<int>();
        List<int> chipCosts = new List<int>();


        for (int i = 1; i < csv.Length; i++)
        {
            string[] rarity = csv[i].Split('&');
            for (int j = 0; j < rarity.Length; j++)
            {
                string[] currencyTypes = rarity[j].Split('^');
         
                    if (!int.TryParse(currencyTypes[CHIP_COST], out int chipCost))
                        throw new System.Exception($"Upgrade/Dismental: Error while trying to parsing string\nInput {currencyTypes[CHIP_COST]}");

                    if (!int.TryParse(currencyTypes[GOLD_COST], out int goldCost))
                        throw new System.Exception($"Upgrade/Dismental: Error while trying to parsing string\nInput {currencyTypes[GOLD_COST]}");

                    chipCosts.Add(chipCost);
                    goldCosts.Add(goldCost);
                
            }

            var currentRarity = (RarityEnum)i;
            goldLevelCosts.Add(new RarityLevelCosts(currentRarity, goldCosts.ToArray()));
            chipLevelCosts.Add(new RarityLevelCosts(currentRarity, chipCosts.ToArray()));

        }

        master.Add(new RarityLevelCostsPerCurrency(CurrencyType.Chips, chipLevelCosts.ToArray()));
        master.Add(new RarityLevelCostsPerCurrency(CurrencyType.Gold, goldLevelCosts.ToArray()));

        _rarityLevelCostsPerCurrencies = master.ToArray();
    }


#endif

}

[System.Serializable]
public class RarityLevelCosts
{
    [SerializeField]
    private RarityEnum _rarity;

    [SerializeField]
    int[] _costs;

    public RarityLevelCosts(RarityEnum rarity, int[] cost)
    {
        this._rarity = rarity;
        _costs = cost;
    }

    public RarityEnum Rarity { get => _rarity; }
    public int[] Costs { get => _costs; }
}

[System.Serializable]
public class RarityLevelCostsPerCurrency
{
    [SerializeField]
    private CurrencyType _currencyType;
    [SerializeField]
    private RarityLevelCosts[] _rarityCosts;

    public RarityLevelCostsPerCurrency(CurrencyType currencyType, RarityLevelCosts[] rarityLevelCosts)
    {
        _currencyType = currencyType;
        _rarityCosts = rarityLevelCosts;
    }

    public CurrencyType CurrencyType { get => _currencyType; }
    public RarityLevelCosts[] RarityCosts { get => _rarityCosts; }
}