using System;
using System.Collections.Generic;

using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class ArenaData
    {
        private int homeArena;
        private int characterID;
        private int skin;
        private DeckData deck;
        private int loses;
        private int wins;

        public int HomeArena { get => homeArena; set => homeArena = value; }
        public int CharacterID { get => characterID; set => characterID = value; }
        public int Skin { get => skin; set => skin = value; }
        public DeckData Deck { get => deck; set => deck = value; }
        public int Loses { get => loses; set => loses = value; }
        public int Wins { get => wins; set => wins = value; }
    }
    [Serializable]
    public class AccountResources
    {
        private int _gold;
        private int _diamonds;
        private int _tickets;

        public int Gold { get => _gold; set => _gold = value; }
        public int Diamonds { get => _diamonds; set => _diamonds = value; }
        public int Tickets { get => _tickets; set => _tickets = value; }
    }
    //[Serializable]
    //public class AccountCards : ILoadFirstTime
    //{
    //    public static Action<CardInstanceID> OnUpgrade;
    //    #region Fields
    //    [SerializeField]
    //    List<CardInstanceID> _cardList = new List<CardInstanceID>();
    //    #endregion
    //    #region Properties
    //    public List<CardInstanceID> CardList => _cardList;

    //    #endregion
    //    #region PublicMethods
    //    public void AddCard(CardInstanceID core)
    //        => _cardList.Add(core);
    //    public void AddCard(Cards.Card card)
    //    => AddCard(card.CardCoreInfo);

    //    public bool RemoveCard(int instanceId)
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
    //        var so = factory.CharacterFactoryHandler.GetCharactersSO(Battles.CharacterTypeEnum.Player);
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
