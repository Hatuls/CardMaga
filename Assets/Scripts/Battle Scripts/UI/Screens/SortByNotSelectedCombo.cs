using Combo;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rei.Utilities
{
    public class SortByNotSelectedCombo : SortAbst<Combo.Combo>
    {
        [SerializeField]
        bool toUseAccoundCombo;
        [SerializeField]
        MetaComboUIFilterScreen filterHandler;

        public int? ID { get; set; }

        // re-do
        public override IEnumerable<Combo.Combo> Sort()
        {
            //var accountCards = toUseAccoundCombo ?Factory.GameFactory.Instance.ComboFactoryHandler.CreateCombo( Account.AccountManager.Instance.AccountCombos.ComboList.ToArray()) :
            //    Account.AccountManager.Instance.BattleData.Player.CharacterData.ComboRecipe;

            //var sortedDeck = accountCards.Where(x => x.Level < x.ComboSO().CraftedCard.CardsMaxLevel - 1);

            //if (ID == null)
            //    return sortedDeck;
            // sortedDeck = sortedDeck.Where(x => x.ID != ID);
            //Debug.Log(sortedDeck.Count());
            //return sortedDeck;
            return null;
        }

        public override void SortRequest()
        {
            filterHandler.SortBy(this);
        }
    }
}