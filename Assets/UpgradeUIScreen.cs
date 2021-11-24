using Battles.UI;
using Map.UI;
using UI;
using UnityEngine;

namespace Meta.UI
{

    public class UpgradeUIScreen : MonoBehaviour
    {
        [SerializeField]
        CardUI _selectedCardUI;

        [SerializeField]
        CardUI _upgradedVersion;

        [SerializeField]
        MetaCardUIFilterScreen _collectionFilterHandler;
    }



}
namespace Meta
{
    public static class UpgradeHandler
    {
        public static Cards.Card GetUpgradedCardVersion(Cards.Card card)
        {
            if (card.CardLevel == card.CardSO.CardsMaxLevel)
                return null;

            return new Cards.Card(card.CardID, card.CardSO, (byte)(card.CardLevel + 1));
        }

        public static bool UpgradeCard(Cards.Card card)
         => card.LevelUpCard();
    }

}