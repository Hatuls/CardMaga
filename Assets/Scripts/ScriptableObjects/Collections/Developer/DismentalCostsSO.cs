using CardMaga.Card;
using UnityEngine;

public class DismentalCostsSO : ScriptableObject
{
    [SerializeField]
    private RarirtyLevelCosts[] _dismentalCosts;

    public RarirtyLevelCosts[] DismentalCosts { get => _dismentalCosts; }

    public ushort GetCardDismentalCost(CardData card)
    {
        for (int i = 0; i < _dismentalCosts.Length; i++)
        {
            if (_dismentalCosts[i].Rarity == card.CardSO.Rarity)
                return _dismentalCosts[i].Costs[card.CardLevel];
        }
        throw new System.Exception($"DismentalCosts: Rarity Was Not Found: {card.CardSO.Rarity}");
    }

#if UNITY_EDITOR
    public void Init(string[] csv)
    {
        _dismentalCosts = new RarirtyLevelCosts[csv.Length];
        for (int i = 0; i < csv.Length; i++)
        {
            string[] costs = csv[i].Split('&');
            ushort[] costsValue = new ushort[costs.Length];
            for (int j = 0; j < costs.Length; j++)
            {
                if (!ushort.TryParse(costs[j], out costsValue[j]))
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
        ushort[] _costs;

        public RarirtyLevelCosts(RarityEnum rarity, ushort[] cost)
        {
            this.rarity = rarity;
            _costs = cost;
        }

        public RarityEnum Rarity { get => rarity;  }
        public ushort[] Costs { get => _costs; }
    }
}

