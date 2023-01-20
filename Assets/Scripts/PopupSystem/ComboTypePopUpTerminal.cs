using Account.GeneralData;
using CardMaga.UI.Combos;
using CardMaga.UI.Visuals;
using System;
using UnityEngine;
namespace CardMaga.UI.PopUp
{
    public class ComboTypePopUpTerminal : BasePopUpTerminal
    {

        [SerializeField]
        private TransitionBuilder[] _transitionIn;
        [SerializeField]
        private TransitionBuilder[] _transitionOut;
        private IPopUpTransition<TransitionData> _popUpTransitionIn;
        private IPopUpTransition<TransitionData> _popUpTransitionOut;
        private IPopUpTransition<AlphaData> _popUpAlphaTransitionIn;

        public override IPopUpTransition<AlphaData> TransitionAlphaIn => _popUpAlphaTransitionIn;

        public override IPopUpTransition<AlphaData> TransitionAlphaOut =>null;

        public override IPopUpTransition<TransitionData> TransitionIn => _popUpTransitionIn;

        public override IPopUpTransition<TransitionData> TransitionOut => _popUpTransitionOut;

        protected override Vector2 GetStartLocation() => PopUpManager.Instance.GetPosition(_startLocation);

        private void Awake()
        {
            BattleComboUI.OnComboTypePopUpSelected += ShowComboPopup;
            BattleComboUI.OnComboTypeRelease += HideComboPopup;
            _popUpTransitionIn = new BasicTransition(GenerateTransitionData(_transitionIn));
            _popUpTransitionOut = new BasicTransition(GenerateTransitionData(_transitionOut));
            _popUpAlphaTransitionIn = new AlphaTransition(GenerateAlphaTransitionData(_transitionIn));
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