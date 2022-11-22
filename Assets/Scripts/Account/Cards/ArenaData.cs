using CardMaga.Rewards;
using System;

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
        internal bool IsValid()
        {
            return Gold >= 0 && Diamonds >= 0 && Tickets >= 0;
        }
    }
    //[Serializable]
    //public class AccountCards : ILoadFirstTime
    //{
    //    public static Action<InstanceID> OnUpgrade;
    //    #region Fields
    //    [SerializeField]
    //    List<InstanceID> _cardList = new List<InstanceID>();
    //    #endregion
    //    #region Properties
    //    public List<InstanceID> CardList => _cardList;

    //    #endregion
    //    #region PublicMethods
    //    public void AddCard(InstanceID core)
    //        => _cardList.Add(core);
    //    public void AddCard(Cards.BattleCard battleCard)
    //    => AddCard(battleCard.InstanceID);

    //    public bool TryRemoveCombo(int instanceId)
    //    {
    //        int length = CardList.Count;
    //        for (int i = 0; i < length; i++)
    //        {
    //            if (_cardList[i].InstanceID == instanceId)
    //            {
    //                return _cardList.Remove(_cardList[i]);
    //            }
    //        }
    //        return false;
    //    }

    //    public void UpgradeCard(int instanceID)
    //    {
    //        int length = _cardList.Count;
    //        for (int i = 0; i < length; i++)
    //        {
    //            if (_cardList[i].InstanceID == instanceID)
    //            {
    //                _cardList[i].Level++;
    //                OnUpgrade?.Invoke(_cardList[i]);
    //            }
    //        }
    //    }

    //    public void NewLoad()
    //    {
    //        var factory = Factory.GameFactory.Instance;
    //        var currentLevel = AccountManager.Instance.AccountGeneralData.AccountLevelData.Level.Value;
    //        var so = factory.CharacterFactoryHandler.GetCharactersSO(Battles.CharacterTypeEnum.LeftPlayer);
    //        foreach (var item in so)
    //        {
    //            if (currentLevel >= item.UnlockAtLevel)
    //            {
    //                      var cards = factory.CardFactoryHandler.CreateDeck(item.Deck);

    //                for (int i = 0; i < cards.Length; i++)
    //                {
    //                    AddCard(cards[i]);
    //                }
    //            }
    //        }


    //    }

    //    public bool IsCorrupted()
    //    {
    //        const int _firstDeckAmount = 8;
    //        return _cardList.Count < _firstDeckAmount;
    //    }


    //    #endregion
    //}
}
