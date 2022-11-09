
using Rei.Utilities;
using System.Collections.Generic;
using Battle.Combo;
using UnityEngine.Events;
using UnityEngine;

namespace CardMaga.UI
{
    [System.Serializable]
    public class SortComboEvent : UnityEvent<ISort<ComboData>> { }
    [System.Serializable]
    public class SortCardEvent : UnityEvent<ISort<CardMaga.Card.BattleCardData>> { }
    public class ShowAllCards : CardSort
    {
        public override IEnumerable<CardMaga.Card.BattleCardData> Sort()
         => GetCollection();
    }


    public abstract  class CardSort : SortAbst<CardMaga.Card.BattleCardData>
    {
        [SerializeField]
        protected SortCardEvent _cardEvent;
        public override void SortRequest() => _cardEvent?.Invoke(this);
    }

    public abstract class ComboSort : SortAbst<ComboData>
    {
        [SerializeField]
        protected SortComboEvent _comboEvent;
        public override void SortRequest() => _comboEvent?.Invoke(this);

    }
}