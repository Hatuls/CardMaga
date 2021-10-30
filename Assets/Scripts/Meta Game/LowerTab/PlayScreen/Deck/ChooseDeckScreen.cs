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
        DeckUI[] _avilableDecks;
        CharacterData _currentCharacter;

        #endregion
        #region Public Methods
        public void InitLoadOutScreen(CharacterData characterData)
        {
            Debug.Log("Initializing Choose Deck Panel");
            ResetChooseDeckScreen();
            ChooseDeckSetActiveState(true);
            PlayScreenUI.Instance.BGPanelSetActiveState(true);
            InitDecksData(characterData);

        }
        public void TransitionToScreen(ChooseDeckScreenUIEnum chooseDeckScreenUIEnum)
        {

        }
        public void SelectDeck()
        {

        }
        public void ResetChooseDeckScreen()
        {
            Debug.Log("Reseting Choose Deck Screen");
            for (int i = 0; i < _avilableDecks.Length; i++)
            {
                _avilableDecks[i].gameObject.SetActive(false);
            }
            ChooseDeckSetActiveState(false);
        }
        public void ChooseDeckSetActiveState(bool toState)
        {
            gameObject.SetActive(toState);
        }
        private void InitDecksData(CharacterData characterData)
        {
            Debug.Log("Initializing Decks Data");
            _avilableDecks = new DeckUI[characterData.Decks.Length];
            for (int i = 0; i < characterData.Decks.Length; i++)
            {
                AccountDeck deck = characterData.Decks[i];
                ushort firstCardID = deck.Cards[0].CardID;
                Cards.CardSO firstCard =  Factory.GameFactory.Instance.CardFactoryHandler.CardCollection.GetCard(firstCardID);
                _avilableDecks[i].init(firstCard.CardSprite,deck.DeckName);
            }
        }
        #endregion
    }
}
