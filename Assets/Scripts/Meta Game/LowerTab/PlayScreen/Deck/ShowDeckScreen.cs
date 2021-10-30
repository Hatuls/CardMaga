using UnityEngine;
using TMPro;
using Account.GeneralData;

namespace UI.Meta.PlayScreen
{
    public class ShowDeckScreen: MonoBehaviour
    {
        #region Fields


        GameObject _chooseDecksPanel;
        [SerializeField]
        TextMeshProUGUI _title;
        DeckUI[] _decksUI;
        #endregion

        #region Public Methods
        public void Init(CharacterData characterData)
        {
            _chooseDecksPanel.gameObject.SetActive(true);
        }
        public void EnableDecksPanel(bool toEnable)
        {

        }
        public void EnableCardsPanel(bool toEnable)
        {

        }
        public void EnableDeckScreenPanel(bool toEnable)
        {

        }
        public void SelectDeckUI(int index)
        {

        }
        public void ConfirmDeck(bool toConfirm)
        {
            
        }
        #endregion
    }
}
