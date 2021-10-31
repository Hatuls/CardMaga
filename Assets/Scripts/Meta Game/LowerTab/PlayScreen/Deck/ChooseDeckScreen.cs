using Account.GeneralData;
using System;
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
        [SerializeField]
        Art.ArtSO _artSO;

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
        public void TransitionToScreen(int screenID)
        {
            if(screenID == 0)
            {
                ShowDeckCards(PlayScreenUI.Instance.playPackage.Deck);
            }
            else
            {
                ShowCombos(PlayScreenUI.Instance.playPackage.CharacterData);
            }
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
        public void ResetShowComboScreen()
        {
            _showComboScreen.ResetShowComboScreen();
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
        public void ShowDeckCards(AccountDeck deck)
        {
            if (_artSO == null)
            {
                throw new Exception("ChooseDeckScreen ArtSO is NULL");
            }
            PlayScreenUI.Instance.BGPanelSetActiveState(true);
            _showComboScreen.ResetShowComboScreen();
            _showCardsScreen.Init(deck, _artSO);
        }
        public void ShowCombos(CharacterData character)
        {
            if (_artSO == null)
            {
                throw new Exception("ChooseDeckScreen ArtSO is NULL");
            }
            PlayScreenUI.Instance.BGPanelSetActiveState(true);
            _showCardsScreen.ResetShowCardsScreen();
            _showComboScreen.Init(character,_artSO);
        }
        #endregion
    }
}
