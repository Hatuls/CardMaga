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
            ChooseDeckSetActiveState(true);
            ResetChooseDeckScreen();

            for (int i = 0; i < _avilableDecks.Length; i++)
            {
                _avilableDecks[i].init();
            }
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
            
        }
        public void ChooseDeckSetActiveState(bool toState)
        {
            _showCardsScreen.gameObject.SetActive(toState);
        }
        #endregion
    }
}
