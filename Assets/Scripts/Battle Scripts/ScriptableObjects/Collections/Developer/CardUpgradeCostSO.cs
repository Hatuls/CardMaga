using UnityEngine;

public class CardUpgradeCostSO : ScriptableObject
{
    [SerializeField]
    ushort[] _costs;


    public ushort NextCardValue(Cards.Card card)
    {
        try
        {
            return _costs[card.CardLevel];
        }
        catch (System.Exception)
        {
            throw;
        }
 
    }
    

#if UNITY_EDITOR
    public void Init(string[] data)
    {
        _costs = new ushort[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            if (ushort.TryParse(data[i], out ushort value))
                _costs[i] = value;
            else
                throw new System.Exception("UpgradeCardCostSO: CSV Error -> Upgrade values are not a valid number\nValue is: " + data[i]);
        }
    }
#endif
}

