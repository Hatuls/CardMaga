namespace Meta
{
    public static class UpgradeHandler
    {
        public static Cards.Card GetUpgradedCardVersion(Cards.Card card)
        {
            if (card.CardLevel == card.CardSO.CardsMaxLevel)
                return null;

            return Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(card.CardSO, (byte)(card.CardLevel + 1));
        }
        public static bool TryUpgradeCard(CardUpgradeCostSO cardUpgradeCostSO,Cards.Card card)
        {
            var account = Account.AccountManager.Instance;
            var chips = account.AccountGeneralData.AccountResourcesData.Chips;
            var Cost = cardUpgradeCostSO.NextCardValue(card);
            if (chips.Value >= Cost )
            {
                account.AccountCards.UpgradeCard(card.CardInstanceID);
                chips.ReduceValue(Cost);
                return true;
            }
            else
                return false;
        }
   
    }

}