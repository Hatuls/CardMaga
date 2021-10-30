using Account.GeneralData;
using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public enum LoadOutScreenUIEnum
    {
        DeckScreen = 0,
        ComboScreen = 1,
    }
    public class ChooseDeckScreen: MonoBehaviour
    {
        #region Fields
        [SerializeField]
        ShowDeckScreen _showDeckScreen;
        [SerializeField]
        ShowComboScreen _showComboScreen;
        CharacterData _currentCharacter;
        #endregion
        #region Public Methods
        public void InitLoadOutScreen(CharacterData characterData)
        {
            ChooseDeckSetActiveState(true);
        }
        public void TransitionToScreen(LoadOutScreenUIEnum loadOutScreenUIEnum)
        {

        }
        public void ConfirmLoadout()
        {

        }
        public void RestLoadOutScreen()
        {

        }
        public void ChooseDeckSetActiveState(bool toState)
        {
            _showDeckScreen.gameObject.SetActive(toState);
        }
        #endregion
    }
}
