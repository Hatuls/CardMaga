
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
    public class SortCardEvent : UnityEvent<ISort<CardMaga.Card.CardData>> { }
    public class ShowAllCards : CardSort
    {
        public override IEnumerable<CardMaga.Card.CardData> Sort()
         => GetCollection();
    }


    public abstract  class CardSort : SortAbst<CardMaga.Card.CardData>
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