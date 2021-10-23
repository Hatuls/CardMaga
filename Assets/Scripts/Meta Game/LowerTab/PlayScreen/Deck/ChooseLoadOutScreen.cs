using Account.GeneralData;
using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public enum LoadOutScreenUIEnum
    {
        DeckScreen = 0,
        ComboScreen = 1,
    }
    public class ChooseLoadOutScreen
    {
        #region Fields
        DecksScreen _totalDeckScreen;
        ComboScreen _totalComboScreen;
        CharacterData _currentCharacter;
        #endregion
        #region Public Methods
        public void InitLoadOutScreen(CharacterEnum characterEnum)
        {

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
        #endregion
    }
}
