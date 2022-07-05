using Battle.Combo;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CardMaga.UI
{
    public class SortComboByLength : ComboSort
    {
        [SerializeField] int length;
        public override IEnumerable<Combo> Sort()
        { 
            return GetCollection().Where(x=>x.ComboSequence.Length == length);
        }
    }
}