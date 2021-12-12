using Rewards;
using UnityEngine;

public class CardUpgradeCostSO : ScriptableObject
{
    [SerializeField]
    ushort[] _upgradecostInMainMenu;


    public ushort NextCardValue(Cards.Card card, ResourceEnum resourceType)
    {
        return 0;
      //  for (int i = 0; i < _resources.Length; i++)
      //  {
      //      if (_resources[i].ResourceEnum == resourceType)
      //      {
      //          return _resources[i].Costs[card.CardLevel];
      //
      //      }
      //  }
      //
      //  throw new System.Exception("CardUpgradeCosts: Resources type was not assigned!\nResource Type: " + resourceType);
    }




#if UNITY_EDITOR
    public void Init(string[] type, string[] data)
    {
   //    ResourcesCost[] resources = new ResourcesCost[2];
   //   
   //
   //        ushort[] _costs = new ushort[data.Length];
   //        for (int i = 0; i < data.Length; i++)
   //        {
   //            if (ushort.TryParse(data[i], out ushort value))
   //                _costs[i] = value;
   //            else
   //                throw new System.Exception("UpgradeCardCostSO: CSV Error -> Upgrade values are not a valid number\nValue is: " + data[i]);
   //        }
   //        resources[j] = new ResourcesCost(j== 0 ? ResourceEnum.Chips : ResourceEnum.Gold, _costs);
   //    }
   //    _resources = resources;
    }
#endif
}

