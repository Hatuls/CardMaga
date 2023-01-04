using Account.GeneralData;
using CardMaga.Card;
using CardMaga.MetaData.AccoutData;
using UnityEngine;

public class DismentalCostsSO : ScriptableObject
{
    [SerializeField]
    private RarirtyLevelCosts[] _dismentalCosts;

    public RarirtyLevelCosts[] DismentalCosts { get => _dismentalCosts; }

    public int GetCardDismentalCost(BattleCardData battleCard)
    {
        for (int i = 0; i < _dismentalCosts.Length; i++)
        {
            if (_dismentalCosts[i].Rarity == battleCard.CardSO.Rarity)
                return _dismentalCosts[i].Costs[battleCard.CardLevel];
        }
        throw new System.Exception($"DismentalCosts: Rarity Was Not Found: {battleCard.CardSO.Rarity}");
    }
    
    public int GetCardDismentalCost(CardInstance cardInstance)
    {
        for (int i = 0; i < _dismentalCosts.Length; i++)
        {
            if (_dismentalCosts[i].Rarity == cardInstance.CardSO.Rarity)
                return _dismentalCosts[i].Costs[cardInstance.Level];
        }
        throw new System.Exception($"DismentalCosts: Rarity Was Not Found: {cardInstance.CardSO.Rarity}");
    }

#if UNITY_EDITOR
    public void Init(string[] csv)
    {
        _dismentalCosts = new RarirtyLevelCosts[csv.Length];
        for (int i = 0; i < csv.Length; i++)
        {
            string[] costs = csv[i].Split('&');
            int[] costsValue = new int[costs.Length];
            for (int j = 0; j < costs.Length; j++)
            {
                if (!int.TryParse(costs[j], out costsValue[j]))
                    throw new System.Exception($"DismentalCostSO: Cost is not valid number !\nRarity: {((RarityEnum)i + 1)} value: {costs[j]}");
            }
            _dismentalCosts[i] = new RarirtyLevelCosts(((RarityEnum)i + 1), costsValue);

        }
    }


#endif
    [System.Serializable]
    public class RarirtyLevelCosts
    {
        [SerializeField]
        private RarityEnum rarity;

        [SerializeField]
        int[] _costs;

        public RarirtyLevelCosts(RarityEnum rarity, int[] cost)
        {
            this.rarity = rarity;
            _costs = cost;
        }

        public RarityEnum Rarity { get => rarity;  }
        public int[] Costs { get => _costs; }
    }
}

