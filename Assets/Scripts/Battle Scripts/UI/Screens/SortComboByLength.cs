﻿using Battle.Combo;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CardMaga.UI
{
    public class SortComboByLength : SortAbst<Combo>
    {
        [SerializeField] int length;

        [SerializeField] bool toUseAccountData;
        // Need To be Re-Done
        public override IEnumerable<Combo> Sort()
        {
            //var combos = toUseAccountData ? Factory.GameFactory.Instance.ComboFactoryHandler.CreateCombo(Account.AccountManager.Instance.AccountCombos.ComboList.ToArray()) :
            //            Account.AccountManager.Instance.BattleData.Player.CharacterData.ComboRecipe;
            //return combos.Where(x => x.ComboSO().ComboSequance.Length == length);
            return null;
        }

        public override void SortRequest()
        {
            _comboEvent?.Invoke(this);
        }
    }
}