using Battles;
using Battles.UI;
using Cards;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map.UI
{
    public class CardUIFilterScreen : MonoBehaviour
    {
        [SerializeField] GameObject _cardUIPrefab;
        [SerializeField] List<CardUI> _deckCardsUI;

        private void CreateCards()
        {
            var deck = BattleData.Player.CharacterData.CharacterDeck;
            while (deck.Length > _deckCardsUI.Count)
            {
                var card = Instantiate(_cardUIPrefab, this.transform).GetComponent<CardUI>();
                _deckCardsUI.Add(card);
            }
        }
     
        public void SortByCards(ISort<Card> sortby)
        {
            CreateCards();
            int length = _deckCardsUI.Count;
            var sortedDeck = sortby.Sort();

            int sortedDeckLength = sortedDeck.Count();

            var artSO = Factory.GameFactory.Instance.ArtBlackBoard;

            for (int i = 0; i < length; i++)
            {
                if (i < sortedDeckLength && sortedDeck.ElementAt(i) != null)
                {
                    if (_deckCardsUI[i].gameObject.activeSelf == false)
                        _deckCardsUI[i].gameObject.SetActive(true);
                    _deckCardsUI[i].GFX.SetCardReference(sortedDeck.ElementAt(i), artSO);
                }
                else
                {
                    if (_deckCardsUI[i].gameObject.activeSelf == true)
                        _deckCardsUI[i].gameObject.SetActive(false);
                }
            }
        }
    }

}