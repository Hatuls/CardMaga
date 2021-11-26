using Battles;
using Cards;
using Rei.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Map.UI
{
    [System.Serializable]
    public class SortEvent : UnityEvent<ISort<Card>> { }
    public class ShowAllCards : SortAbst<Card>
    {
        [SerializeField]
        SortEvent _event;


        public override void SortRequest() => _event?.Invoke(this);
        public override IEnumerable<Card> Sort()
        {

            if (SceneHandler.CurrentScene == SceneHandler.ScenesEnum.MainMenuScene)
                return Factory.GameFactory.Instance.CardFactoryHandler.CreateDeck(Account.AccountManager.Instance.AccountCards.CardList.ToArray());
            else
            {
                return BattleData.Player.CharacterData.CharacterDeck;
            }


        }
    }
}