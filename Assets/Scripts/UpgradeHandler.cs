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
        public static bool TryUpgradeCombo(CardUpgradeCostSO upgrade, Combo.Combo combo , Rewards.ResourceEnum resourceenum)
        {
            Characters.Character battleData = Battles.BattleData.Player;
            int gold = battleData.CharacterData.CharacterStats.Gold;
          
            ushort Cost = upgrade.NextCardValue(combo.ComboSO.CraftedCard,combo.Level);
            if (gold >= Cost)
            {
                combo.LevelUp();
                battleData.CharacterData.CharacterStats.Gold -= Cost;
                return true;
            }
            return false;
        }
        public static bool TryUpgradeCard(CardUpgradeCostSO cardUpgradeCostSO, Cards.Card card, Rewards.ResourceEnum resourceEnum)
        {
            if (resourceEnum == Rewards.ResourceEnum.Chips)
            {

                Account.AccountManager account = Account.AccountManager.Instance;
                Account.GeneralData.UshortStat chips = account.AccountGeneralData.AccountResourcesData.Chips;
                ushort Cost = cardUpgradeCostSO.NextCardValue(card, resourceEnum);
                if (chips.Value >= Cost)
                {
                    account.AccountCards.UpgradeCard(card.CardInstanceID);
                    chips.ReduceValue(Cost);
                    return true;
                }
            }
            else if (resourceEnum == Rewards.ResourceEnum.Gold)
            {
                Characters.Character account = Battles.BattleData.Player;
                int gold = account.CharacterData.CharacterStats.Gold;
                ushort Cost = cardUpgradeCostSO.NextCardValue(card, resourceEnum);
                if (gold >= Cost)
                {
                    account.RemoveCardFromDeck(card.CardInstanceID);
                    account.AddCardToDeck(card.CardSO, (byte)(card.CardLevel + 1));
                    account.CharacterData.CharacterStats.Gold -= Cost;
                    return true;
                }
            }
            return false;
        }

    }

}