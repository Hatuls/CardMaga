using Account;
using Cards;
using Battle;
using Rewards;
using Battle.Combo;

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
        public static Combo GetUpgradedComboVersion(Combo combo)
        {
            var ComboSO = combo.ComboSO;
            if (combo.Level == ComboSO.CraftedCard.CardsMaxLevel)
                return null;

            return Factory.GameFactory.Instance.ComboFactoryHandler.CreateCombo(ComboSO, combo.Level + 1);
        }
        public static bool TryUpgradeCombo(CardUpgradeCostSO upgrade, Combo combo, ResourceEnum resourceenum)
        {
            return true;
            //Characters.Character battleData = Account.AccountManager.Instance.BattleData.Player;
            //int gold = battleData.CharacterData.CharacterStats.Gold;

            //int Cost = upgrade.NextCardValue(combo.ComboSO().CraftedCard, combo.Level);
            //if (gold >= Cost)
            //{
            //    combo.LevelUp();

            //    SendComboDataAnalyticEvent(combo);

            //    battleData.CharacterData.CharacterStats.Gold -= Cost;
            //    return true;
            //}
            //return false;
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
           //var chips = account.AccountGeneralData.AccountResourcesData.Chips;
           // int Cost = cardUpgradeCostSO.NextCardValue(card, resourceEnum);
           // if (chips.Value >= Cost)
           // {
           //     SendMetaGameUpgradeCardAnalyticEvent(card);
           //     account.AccountCards.UpgradeCard(card.CardInstanceID);
           //     chips.ReduceValue(Cost);
           //     return true;
           // }

            return true;
        }
        private static bool TryUpgradeInMap(CardUpgradeCostSO cardUpgradeCostSO, Card card, ResourceEnum resourceEnum, AccountManager account)
        {
            return true;
            //var player = account.BattleData.Player;
            //int gold = player.CharacterData.CharacterStats.Gold;
            //int Cost = cardUpgradeCostSO.NextCardValue(card, resourceEnum);
            //if (gold >= Cost)
            //{
                
            //    SendInMapUpgradeCardAnalyticEvent(card);
            //    card.CardCoreInfo.Level ++;
            //    player.CharacterData.CharacterStats.Gold -= Cost;
            //    return true;
            //}
            //return false;
        }

        #region Analytics Events
        private static void SendInMapUpgradeCardAnalyticEvent(Card card)
        {
            UnityAnalyticHandler.SendEvent("card_upgraded_in_dojo", new System.Collections.Generic.Dictionary<string, object>()
                    {
                        {"card", card.CardSO.CardName.Replace(' ', '_')},
                        {"level", card.CardLevel},
                    });
            FireBaseHandler.SendEvent(
                "card_upgraded_in_map",
                new Firebase.Analytics.Parameter("card_name", card.CardSO.CardName.Replace(' ', '_')),
                new Firebase.Analytics.Parameter("card_level", card.CardLevel)
                );
        }
        private static void SendMetaGameUpgradeCardAnalyticEvent(Card card)
        {
            UnityAnalyticHandler.SendEvent("card_upgraded_in_meta_game", new System.Collections.Generic.Dictionary<string, object>()
                    {
                        {"card", card.CardSO.CardName.Replace(' ', '_')},
                        {"level", card.CardLevel},
                    });
            FireBaseHandler.SendEvent(
                "card_upgraded_in_meta_game",
                new Firebase.Analytics.Parameter("card_name", card.CardSO.CardName.Replace(' ', '_')),
                new Firebase.Analytics.Parameter("card_level", card.CardLevel)
                );
        }

        private static void SendComboDataAnalyticEvent(Combo combo)
        {
            UnityAnalyticHandler.SendEvent("combo_upgraded_in_dojo", new System.Collections.Generic.Dictionary<string, object>()
                    {
                        {"combo_name", combo.ComboSO.ComboName.Replace(' ', '_')},
                        {"combo_level", combo.Level},
                    });
            FireBaseHandler.SendEvent("combo_upgraded_in_dojo",
                new Firebase.Analytics.Parameter("combo_name", combo.ComboSO.ComboName.Replace(' ', '_')),
                new Firebase.Analytics.Parameter("combo_level", combo.Level)
                );
        }

        #endregion
    }
}