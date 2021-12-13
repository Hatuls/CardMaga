using Cards;
using Rewards;
using UnityEngine;

public class CardUpgradeCostSO : ScriptableObject
{
    [SerializeField]
    ushort[] _upgradecostInMainMenu;
    [SerializeField]
    Collections.CardsCollectionSO.RarityCards[] _rarityCards;
    public ushort NextCardValue(Cards.Card card, ResourceEnum resourceType)
    {
        if (resourceType == ResourceEnum.Chips)
            return _upgradecostInMainMenu[card.CardLevel];
        else
        {
            return NextCardValue(card.CardSO, card.CardLevel);
        }
        throw new System.Exception($"CardUpgradeCostSO : Rarity was not valid!");
    }

    public ushort NextCardValue(CardSO card, byte level)
    {

        for (int i = 0; i < _rarityCards.Length; i++)
        {
            if (_rarityCards[i].Rarity == card.Rarity)
            {
                return _rarityCards[i].CardsID[level];
            }
        }
        throw new System.Exception($"CardUpgradeCostSO: Next Card Value Was not found!");
    }


#if UNITY_EDITOR
    public void Init(string[] type, string[] data)
    {
        const int RarityTypes = 5;
        _rarityCards = new Collections.CardsCollectionSO.RarityCards[RarityTypes];

        //battle upgrades
        for (int i = 0; i < RarityTypes; i++)
        {

            string[] seperations = type[i].Split('&');
            ushort[] costs = new ushort[seperations.Length];

            for (int j = 0; j < seperations.Length; j++)
            {
                if (ushort.TryParse(seperations[j], out ushort result))
                {
                    costs[j] = result;
                }
                else
                    throw new System.Exception($"Reward Gold is not a valid number - " + seperations[j]);
            }
            _rarityCards[i] = new Collections.CardsCollectionSO.RarityCards(costs, (Cards.RarityEnum)(i + 1));
        }

        //Main Menu upgrades
        ushort[] _costs = new ushort[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            if (ushort.TryParse(data[i], out ushort value))
                _costs[i] = value;
            else
                throw new System.Exception("UpgradeCardCostSO: CSV Error -> Upgrade values are not a valid number\nValue is: " + data[i]);
        }
        _upgradecostInMainMenu = _costs;
    }
#endif

}

