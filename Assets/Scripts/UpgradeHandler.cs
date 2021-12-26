﻿using Account;
using Cards;
using Rewards;

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
        public static Combo.Combo GetUpgradedComboVersion(Combo.Combo combo)
        {
            if (combo.Level == combo.ComboSO.CraftedCard.CardsMaxLevel)
                return null;

            return Factory.GameFactory.Instance.ComboFactoryHandler.CreateCombo(combo.ComboSO, (byte)(combo.Level + 1));
        }
        public static bool TryUpgradeCombo(CardUpgradeCostSO upgrade, Combo.Combo combo, Rewards.ResourceEnum resourceenum)
        {
            Characters.Character battleData = Account.AccountManager.Instance.BattleData.Player;
            int gold = battleData.CharacterData.CharacterStats.Gold;

            ushort Cost = upgrade.NextCardValue(combo.ComboSO.CraftedCard, combo.Level);
            if (gold >= Cost)
            {
                combo.LevelUp();

                SendComboDataAnalyticEvent(combo);

                battleData.CharacterData.CharacterStats.Gold -= Cost;
                return true;
            }
            return false;
        }
        public static bool TryUpgradeCard(CardUpgradeCostSO cardUpgradeCostSO, Card card, ResourceEnum resourceEnum)
        {
            AccountManager account = AccountManager.Instance;
            if (resourceEnum == ResourceEnum.Chips)
            {
                return TryUpgradeInMetaGame(cardUpgradeCostSO, card, resourceEnum, account);
            }
            else if (resourceEnum == ResourceEnum.Gold)
            {
                return TryUpgradeInMap(cardUpgradeCostSO, card, resourceEnum, account);
            }
            return false;
        }

        private static bool TryUpgradeInMetaGame(CardUpgradeCostSO cardUpgradeCostSO, Card card, ResourceEnum resourceEnum, AccountManager account)
        {
            Account.GeneralData.UshortStat chips = account.AccountGeneralData.AccountResourcesData.Chips;
            ushort Cost = cardUpgradeCostSO.NextCardValue(card, resourceEnum);
            if (chips.Value >= Cost)
            {
                SendMetaGameUpgradeCardAnalyticEvent(card);
                account.AccountCards.UpgradeCard(card.CardInstanceID);
                chips.ReduceValue(Cost);
                return true;
            }

            return false;
        }
        private static bool TryUpgradeInMap(CardUpgradeCostSO cardUpgradeCostSO, Card card, ResourceEnum resourceEnum, AccountManager account)
        {
            var player = account.BattleData.Player;
            int gold = player.CharacterData.CharacterStats.Gold;
            ushort Cost = cardUpgradeCostSO.NextCardValue(card, resourceEnum);
            if (gold >= Cost)
            {
                SendInMapUpgradeCardAnalyticEvent(card);

                player.RemoveCardFromDeck(card.CardInstanceID);
                player.AddCardToDeck(card.CardSO, (byte)(card.CardLevel + 1));
                player.CharacterData.CharacterStats.Gold -= Cost;
                return true;
            }
            return false;
        }

        #region Analytics Events
        private static void SendInMapUpgradeCardAnalyticEvent(Card card)
        {
            AnalyticsHandler.SendEvent("Card Upgraded In Map", new System.Collections.Generic.Dictionary<string, object>()
                    {
                        {"Card", card.CardSO.CardName},
                        {"Level", card.CardLevel},
                    });
            FireBaseHandler.SendEvent(
                "Card Upgraded In Map",
                new Firebase.Analytics.Parameter("Card Name", card.CardSO.CardName),
                new Firebase.Analytics.Parameter("Card Level From", card.CardLevel)
                );
        }
        private static void SendMetaGameUpgradeCardAnalyticEvent(Card card)
        {
            AnalyticsHandler.SendEvent("Card Upgraded In Meta Game", new System.Collections.Generic.Dictionary<string, object>()
                    {
                        {"Card", card.CardSO.CardName},
                        {"Level", card.CardLevel},
                    });
            FireBaseHandler.SendEvent(
                "Card Upgraded In Meta Game",
                new Firebase.Analytics.Parameter("Card Name", card.CardSO.CardName),
                new Firebase.Analytics.Parameter("Card Level From", card.CardLevel)
                );
        }

        private static void SendComboDataAnalyticEvent(Combo.Combo combo)
        {
            AnalyticsHandler.SendEvent("Combo Upgraded In Map", new System.Collections.Generic.Dictionary<string, object>()
                    {
                        {"Combo", combo.ComboSO.ComboName},
                        {"From Level", combo.Level},
                    });
            FireBaseHandler.SendEvent("Combo Upgraded In Map",
                new Firebase.Analytics.Parameter("Combo Name", combo.ComboSO.ComboName),
                new Firebase.Analytics.Parameter("From Level", combo.Level)
                );
        }

        #endregion
    }
}