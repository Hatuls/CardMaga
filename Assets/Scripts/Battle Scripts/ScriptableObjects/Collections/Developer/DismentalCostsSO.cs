using Cards;
using UnityEngine;

public class DismentalCostsSO : ScriptableObject
{
    [SerializeField]
    private DismentalCost[] _dismentalCosts;

    public DismentalCost[] DismentalCosts { get => _dismentalCosts; }



#if UNITY_EDITOR
    public void Init(string[] csv)
    {
        _dismentalCosts = new DismentalCost[csv.Length];
        for (int i = 0; i < csv.Length; i++)
        {
            string[] costs = csv[i].Split('&');
            ushort[] costsValue = new ushort[costs.Length];
            for (int j = 0; j < costs.Length; j++)
            {
                if (!ushort.TryParse(costs[j], out costsValue[j]))
                    throw new System.Exception($"DismentalCostSO: Cost is not valid number !\nRarity: {((RarityEnum)i + 1)} value: {costs[j]}");
            }
            _dismentalCosts[i] = new DismentalCost(((RarityEnum)i + 1), costsValue);

        }
    }


#endif
    [System.Serializable]
    public class DismentalCost
    {
        [SerializeField]
        private RarityEnum rarity;

        [SerializeField]
        ushort[] _dismentalCosts;

        public DismentalCost(RarityEnum rarity, ushort[] dismentalCosts)
        {
            this.rarity = rarity;
            _dismentalCosts = dismentalCosts;
        }

        public RarityEnum Rarity { get => rarity;  }
        public ushort[] DismentalCosts { get => _dismentalCosts; }
    }
}

