using Account.GeneralData;
using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public enum ChooseDeckScreenUIEnum
    {
        CardsScreen = 0,
        ComboScreen = 1,
    }
    public class ChooseDeckScreen: MonoBehaviour
    {
        #region Fields
        [SerializeField]
        ShowCardsScreen _showCardsScreen;
        [SerializeField]
        ShowComboScreen _showComboScreen;
        [SerializeField]
        DeckUI[] _availableDecks;
        CharacterData _currentCharacter;

        #endregion
        #region Public Methods
        public void InitChooseDeckScreen()
        {
            PlayScreenUI playScreenUI = PlayScreenUI.Instance;
            Debug.Log("Initializing Choose Deck Panel");
            ResetChooseDeckScreen();
            ChooseDeckSetActiveState(true);
            playScreenUI.BGPanelSetActiveState(true);
            InitDecksData(playScreenUI.playPackage.CharacterData);
        }
        public void TransitionToScreen(ChooseDeckScreenUIEnum chooseDeckScreenUIEnum)
        {

        }
        public void ResetChooseDeckScreen()
        {
            Debug.Log("Reseting Choose Deck Screen");
            for (int i = 0; i < _availableDecks.Length; i++)
            {
                _availableDecks[i].gameObject.SetActive(false);
            }
            ChooseDeckSetActiveState(false);
        }
        public void ResetShowCardsScreen()
        {
            _showCardsScreen.ResetShowCardsScreen();
        }
        public void ChooseDeckSetActiveState(bool toState)
        {
            gameObject.SetActive(toState);
        }
        private void InitDecksData(CharacterData characterData)
        {
            Debug.Log("Initializing Decks Data");
            for (int i = 0; i < characterData.Decks.Length; i++)
            {
                AccountDeck deck = characterData.Decks[i];
                ushort firstCardID = deck.Cards[0].CardID;
                Cards.CardSO firstCard =  Factory.GameFactory.Instance.CardFactoryHandler.CardCollection.GetCard(firstCardID);
                _availableDecks[i].gameObject.SetActive(true);
                _availableDecks[i].Init(firstCard.CardSprite,deck);
            }
        }
        public void DeckStats(AccountDeck deck)
        {
            _showCardsScreen.Init(deck);
        }
        public void ShowComboCards(CharacterData character)
        {
            _showComboScreen.Init(character);
        }
        #endregion
    }
}
