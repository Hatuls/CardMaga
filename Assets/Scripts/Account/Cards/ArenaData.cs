using CardMaga.Rewards;
using CardMaga.Rewards.Bundles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Account.GeneralData
{
    [Serializable]
    public class ArenaData
    {
        [NonSerialized]
        public const string PlayFabKeyName = "ArenaData";


        public int HomeArena;
        public int CharacterID;
        public int Skin;
        public DeckData Deck;
        public int Loses;
        public int Wins;

        internal bool IsValid()
        {
            // Will need to do valid checks
            return true;
        }
    }
    [Serializable]
    public class AccountResources
    {
        [NonSerialized] public const string PlayFabKeyName = "ResourcesData";


        public int Gold;
        public int Diamonds;
        public int Tickets;
        public int Chips;

        public AccountResources()
        {
            Chips = 0;
            Gold = 0;
            Diamonds = 0;
            Tickets = 0;
        }
        public void AddResource(ResourcesCost resourcesCost)
            => AddResource(resourcesCost.CurrencyType, (int)resourcesCost.Amount);
        public void AddResource(CurrencyType currencyType, int amount)
        {
            switch (currencyType)
            {

                case CurrencyType.Gold:
                    Gold += amount;
                    break;
                case CurrencyType.Diamonds:
                    Diamonds += amount;
                    break;
                case CurrencyType.Chips:
                    Chips += amount;
                    break;

            }
        }

        public bool HasEnoughAmount(IReadOnlyList<ResourcesCost> resourcesCosts)
        {
            bool hasEnough = true;
            for (int i = 0; i < resourcesCosts.Count; i++)
            {
                hasEnough &= HasEnoughAmount(resourcesCosts[i]);
                if (!hasEnough)
                    break;
            }
            return hasEnough;
        }
        public bool HasEnoughAmount(ResourcesCost resourcesCost)
            => HasEnoughAmount(resourcesCost.CurrencyType, (int)resourcesCost.Amount);
        
        public bool HasEnoughAmount(CurrencyType currencyType, int amount)
        {
            switch (currencyType)
            {
                case CurrencyType.Gold:
                    return Gold >= amount;

                case CurrencyType.Diamonds:
                    return Diamonds >= amount;

                case CurrencyType.Chips:
                    return Chips >= amount;

                case CurrencyType.Free:
                    return true;
            }
            throw new Exception("Currency requested is not valid\n CurrencyType is " + currencyType);
        }
        public bool TryReduceAmount(IReadOnlyList<ResourcesCost> resourcesCost)
        {
            bool canReduceAll = true;
            int count = resourcesCost.Count;
            for (int i = 0; i < count; i++)
            {
                canReduceAll &= HasEnoughAmount(resourcesCost[i]);
                if (!canReduceAll)
                    return false;
            }
            for (int i = 0; i < count; i++)
            {
                ReduceAmount(resourcesCost[i]);
            }
               

            return canReduceAll;
        }
        public bool TryReduceAmount(ResourcesCost resourcesCost)
            => TryReduceAmount(resourcesCost.CurrencyType, (int) resourcesCost.Amount);
        public bool TryReduceAmount(CurrencyType currencyType, int amount)
        {
            if (!HasEnoughAmount(currencyType, amount))
                return false;

            ReduceAmount(currencyType, amount);

            return true;
        }
        private void ReduceAmount(ResourcesCost resourcesCost)
            => ReduceAmount(resourcesCost.CurrencyType, Mathf.RoundToInt(resourcesCost.Amount));
        private void ReduceAmount(CurrencyType currencyType,int amount)
        {
            switch (currencyType)
            {

                case CurrencyType.Gold:
                    Gold -= amount;
                    break;
                case CurrencyType.Diamonds:
                    Diamonds -= amount;
                    break;
                case CurrencyType.Chips:
                    Chips -= amount;
                    break;
                case CurrencyType.Free:
                    break;
                default:
                    break;
            }
        }
        
        internal bool IsValid()
        {
            return Gold >= 0 && Diamonds >= 0 && Tickets >= 0;
        }
    }


}
