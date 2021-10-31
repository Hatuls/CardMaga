using UnityEngine;
using TMPro;
using Account.GeneralData;

namespace UI.Meta.PlayScreen
{
    public class ShowComboScreen : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        TextMeshProUGUI _title;
        [SerializeField]
        ComboRowUI[] _comboUI;
        #endregion
        #region Public Method
        public void Init(CharacterData character)
        {
            gameObject.SetActive(true);

            ResetCombosShown();

            ShowCombos(character);
        }
        public void ResetShowComboScreen()
        {
            ResetCombosShown();
            gameObject.SetActive(false);
        }
        private void ResetCombosShown()
        {
            for (int i = 0; i < _comboUI.Length; i++)
            {
                _comboUI[i].gameObject.SetActive(false);
            }
        }
        private void ShowCombos(CharacterData character)
        {
            var factorycomboCollection = Factory.GameFactory.Instance.ComboFactoryHandler.ComboCollection;
            for (int i = 0; i < character.CharacterCombos.Length; i++)
            {
                _comboUI[i].gameObject.SetActive(true);
                _comboUI[i].Init(factorycomboCollection.GetCombo(character.CharacterCombos[i].ID), character.CharacterCombos[i].Level);
            }
        }
        #endregion
    }
}
