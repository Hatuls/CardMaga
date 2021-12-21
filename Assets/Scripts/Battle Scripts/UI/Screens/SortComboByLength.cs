using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Map.UI
{
    public class SortComboByLength : SortAbst<Combo.Combo>
    {
        [SerializeField] int length;

        [SerializeField] bool toUseAccountData;
        public override IEnumerable<Combo.Combo> Sort()
        {
            var combos = toUseAccountData ? Factory.GameFactory.Instance.ComboFactoryHandler.CreateCombo(Account.AccountManager.Instance.AccountCombos.ComboList.ToArray()) :
                        Account.AccountManager.Instance.BattleData.Player.CharacterData.ComboRecipe;
            return combos.Where(x => x.ComboSO.ComboSequance.Length == length);
        }

        public override void SortRequest()
        {
            _comboEvent?.Invoke(this);
        }
    }
}