using UnityEngine;
using TMPro;
using Account.GeneralData;

namespace UI.Meta.PlayScreen
{
    public class ShowCardsScreen: MonoBehaviour
    {
        #region Fields
        [SerializeField]
        TextMeshProUGUI _title;
        [SerializeField]
        CardUIRow[] _cards;
        #endregion

        #region Public Methods
        public void Init(AccountDeck deck)
        {
            gameObject.SetActive(true);

            ResetCardsShown();

            ShowCards(deck);
            _title.text = deck.DeckName;
        }
        public void ResetShowCardsScreen()
        {
            ResetCardsShown();
            gameObject.SetActive(false);
        }
        private void ResetCardsShown()
        {
            for (int i = 0; i < _cards.Length; i++)
            {
                _cards[i].gameObject.SetActive(false);
            }
        }
        #endregion
        private void ShowCards(AccountDeck deck)
        {
            var factoryCardCollection = Factory.GameFactory.Instance.CardFactoryHandler;
            for (int i = 0; i < deck.Cards.Length; i++)
            {
                _cards[i].gameObject.SetActive(true);
                _cards[i].Init(factoryCardCollection.GetCard(deck.Cards[i].ID), deck.Cards[i].Level);
            }
        }
    }
}
