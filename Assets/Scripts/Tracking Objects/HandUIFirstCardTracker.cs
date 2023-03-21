using Battle;
using CardMaga.UI.Card;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.Trackers
{
    public class HandUIFirstCardTracker : Tracker
    {
        [SerializeField] private UI.HandUI _handUI;
        private IReadOnlyList<BattleCardUI> _cards;
        public override RectTransform RectTransform => FirstCardRectTransform();

        private RectTransform FirstCardRectTransform()
        {
            _cards = _handUI.GetCardUIFromHand();
            return _cards[0].RectTransform;
        }
    }
}