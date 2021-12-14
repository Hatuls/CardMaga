using Battles;
using Cards;
using Rei.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Map.UI
{
    [System.Serializable]
    public class SortComboEvent : UnityEvent<ISort<Combo.Combo>> { }
    [System.Serializable]
    public class SortCardEvent : UnityEvent<ISort<Card>> { }
    public class ShowAllCards : SortAbst<Card>
    {

        public override void SortRequest() => _cardEvent?.Invoke(this);
        public override IEnumerable<Card> Sort()
        {

            if (SceneHandler.CurrentScene == SceneHandler.ScenesEnum.MainMenuScene)
                return Factory.GameFactory.Instance.CardFactoryHandler.CreateDeck(Account.AccountManager.Instance.AccountCards.CardList.ToArray());
            else
            {
                return Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterDeck;
            }


        }
    }


}