using Account;
using Battle;
using Rewards;
using Battle.Combo;
using CardMaga.Card;

namespace Meta
{
    public static class UpgradeHandler
    {
        public static CardData GetUpgradedCardVersion(CardData card)
        {
            if (card.CardLevel == card.CardSO.CardsMaxLevel)
                return null;

            return Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(card.CardSO, (byte)(card.CardLevel + 1));
        }
        public static ComboData GetUpgradedComboVersion(ComboData comboData)
        {
            var ComboSO = comboData.ComboSO;
            if (comboData.Level == ComboSO.CraftedCard.CardsMaxLevel)
                return null;

            return Factory.GameFactory.Instance.ComboFactoryHandler.CreateCombo(ComboSO, comboData.Level + 1);
        }
        public static bool TryUpgradeCombo(CardUpgradeCostSO upgrade, ComboData comboData, ResourceEnum resourceenum)
        {
            return true;
            //Characters.Character battleData = Account.AccountManager.Instance.BattleData.LeftPlayer;
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
        public static bool TryUpgradeCard(CardUpgradeCostSO cardUpgradeCostSO, CardData card, ResourceEnum resourceEnum)
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

        private static bool TryUpgradeInMetaGame(CardUpgradeCostSO cardUpgradeCostSO, CardData card, ResourceEnum resourceEnum, AccountManager account)
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
        private static bool TryUpgradeInMap(CardUpgradeCostSO cardUpgradeCostSO, CardData card, ResourceEnum resourceEnum, AccountManager account)
        {
            return true;
            //var player = account.BattleData.LeftPlayer;
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
        private static void SendInMapUpgradeCardAnalyticEvent(CardData card)
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
        private static void SendMetaGameUpgradeCardAnalyticEvent(CardData card)
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

        private static void SendComboDataAnalyticEvent(ComboData comboData)
        {
            UnityAnalyticHandler.SendEvent("combo_upgraded_in_dojo", new System.Collections.Generic.Dictionary<string, object>()
                    {
                        {"combo_name", comboData.ComboSO.ComboName.Replace(' ', '_')},
                        {"combo_level", comboData.Level},
                    });
            FireBaseHandler.SendEvent("combo_upgraded_in_dojo",
                new Firebase.Analytics.Parameter("combo_name", comboData.ComboSO.ComboName.Replace(' ', '_')),
                new Firebase.Analytics.Parameter("combo_level", comboData.Level)
                );
        }

        #endregion
    }
}