using CardMaga.Card;
using System;
using UnityEngine;
namespace Account.GeneralData
{



    [Serializable]
    public class CardInstanceID : IEquatable<CardInstanceID>, IDisposable
    {
        private static int _uniqueID =0;
        private static int UniqueID => _uniqueID++;
        #region Fields
        [SerializeField]
        private CardCore _coreData;
        [SerializeField]
        private int _instanceID;

        #endregion

        #region Properties
        public int ID => _coreData.ID;
        public int InstanceID { get => _instanceID; set => _instanceID = value; }
        public int Level { get => _coreData.Level; set => _coreData.SetLevel (value); }
        public int Exp { get => _coreData.EXP; set => _coreData.SetEXP(value); }

        public CardInstanceID(CardCore card)
        {
            _coreData = card;
            _instanceID = UniqueID;
        }

        public bool Equals(CardInstanceID other)
        {
            return other.InstanceID == _instanceID;
        }

   
        public CardCore GetCardCore()
          => _coreData;

        public void Dispose()
        {
            _coreData.Dispose();
        }
        #endregion


    }
    [Serializable]
    public class CardCore : IDisposable
    {
        [SerializeField] private int _iD;
        [SerializeField] private int _level;
        [SerializeField] private int _expierence;

        public int ID { get => _iD; private set => _iD = value; }
        public int Level { get => _level; private set => _level = value; }
        public int EXP { get => _expierence; private set => _expierence = value; }

        public CardCore(int iD, int level = 1, int expierence = 0)
        {
            EXP = expierence;
            ID = iD;
            Level = level;
            Factory.GameFactory.CardFactory.Register(this);
        }

        public void SetLevel(int amount)
            => _level = amount;
        public void SetEXP(int amount)
            => _expierence = amount;

        public void Dispose()
        {
            Factory.GameFactory.CardFactory.Remove(this);
        }

        public CardCore(CardInstanceID cardInstanceID) : this(cardInstanceID.ID, cardInstanceID.Level, cardInstanceID.Exp) { }

    }
    public static class CardHelper
    {
        public static CardInstanceID CreateInstance(this CardCore core)
        => new CardInstanceID(core);
        public static CardSO CardSO(this CardInstanceID card)
    => CardSO(card.ID);
        public static CardSO CardSO(this CardCore card)
            => CardSO(card.ID);
            public static CardSO CardSO(int id)
          => Factory.GameFactory.Instance.CardFactoryHandler.GetCard(id);
    }
}
