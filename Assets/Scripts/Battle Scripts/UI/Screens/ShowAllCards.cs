using Cards;
using Rei.Utilities;
using System.Collections.Generic;
using Battle.Combo;
using UnityEngine.Events;
using UnityEngine;

namespace CardMaga.UI
{
    [System.Serializable]
    public class SortComboEvent : UnityEvent<ISort<Combo>> { }
    [System.Serializable]
    public class SortCardEvent : UnityEvent<ISort<Card>> { }
    public class ShowAllCards : CardSort
    {
        public override IEnumerable<Card> Sort()
         => GetCollection();
    }


    public abstract  class CardSort : SortAbst<Card>
    {
        [SerializeField]
        protected SortCardEvent _cardEvent;
        public override void SortRequest() => _cardEvent?.Invoke(this);
    }

    public abstract class ComboSort : SortAbst<Combo>
    {
        [SerializeField]
        protected SortComboEvent _comboEvent;
        public override void SortRequest() => _comboEvent?.Invoke(this);

    }
}