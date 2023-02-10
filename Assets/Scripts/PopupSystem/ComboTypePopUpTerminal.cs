using Account.GeneralData;
using CardMaga.UI.Combos;
using CardMaga.UI.Visuals;
using System;
using UnityEngine;
namespace CardMaga.UI.PopUp
{
    public class ComboTypePopUpTerminal : BasePopUpTerminal
    {


        public override IPopUpTransition<AlphaData> TransitionAlphaOut =>null;


        protected override Vector2 GetStartLocation() => PopUpManager.Instance.GetPosition(_startLocation);

        protected override  void Start()
        {
            if (PopUpManager.Instance == null)
                return;
            base.Start();
            BattleComboUI.OnComboTypePopUpSelected += ShowComboPopup;
            BattleComboUI.OnComboTypeRelease += HideComboPopup;
        }

        private void OnDestroy()
        {
            BattleComboUI.OnComboTypePopUpSelected -= ShowComboPopup;
            BattleComboUI.OnComboTypeRelease       -= HideComboPopup;
        }
        private void HideComboPopup()
        {
            HidePopUp();
            _currentActivePopUp?.Dispose();
        }

        private void ShowComboPopup(ComboCore obj)
        {
            base.ShowPopUp();
            _currentActivePopUp.GetComponent<ComboPopUpAssigner>().SetVisual(obj);
        }
    }
}