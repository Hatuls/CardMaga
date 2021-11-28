using System;
using System.Collections.Generic;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class AccountCards : ILoadFirstTime
    {
        public static Action<CardCoreInfo> OnUpgrade;
        #region Fields
        [SerializeField]
        List<CardCoreInfo> _cardList = new List<CardCoreInfo>();
        #endregion
        #region Properties
        public List<CardCoreInfo> CardList => _cardList;

        #endregion
        #region PublicMethods
        public void AddCard(CardCoreInfo core)
            => _cardList.Add(core);
        public void AddCard(Cards.Card card)
        => AddCard(card.CardCoreInfo);

        public bool RemoveCard(uint instanceId)
        {
            int length = CardList.Count;
            for (int i = 0; i < length; i++)
            {
                if (_cardList[i].InstanceID == instanceId)
                {
                    return _cardList.Remove(_cardList[i]);
                }
            }
            return false;
        }

        public void UpgradeCard(uint instanceID)
        {
            int length = _cardList.Count;
            for (int i = 0; i < length; i++)
            {
                if (_cardList[i].InstanceID == instanceID)
                {
                    _cardList[i].Level++;
                    OnUpgrade?.Invoke(_cardList[i]);
                }
            }
        }

        public void NewLoad()
        {
            var factory = Factory.GameFactory.Instance;
            var currentLevel = AccountManager.Instance.AccountGeneralData.AccountLevelData.Level.Value;
            var so = factory.CharacterFactoryHandler.GetCharactersSO(Battles.CharacterTypeEnum.Player);
            foreach (var item in so)
            {
                if (currentLevel >= item.UnlockAtLevel)
                {
                          var cards = factory.CardFactoryHandler.CreateDeck(item.Deck);

                    for (int i = 0; i < cards.Length; i++)
                    {
                        AddCard(cards[i]);
                    }
                }
            }


        }
        #endregion
    }
}
