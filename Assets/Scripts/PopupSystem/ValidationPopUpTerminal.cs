using CardMaga.Battle.Players;
using CardMaga.ValidatorSystem;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    public class ValidationPopUpTerminal : BasePopUpTerminal
    {



        [SerializeField]
        private LocationTagSO _middleScreenPosition;

        public override IPopUpTransition<AlphaData> TransitionAlphaOut => null;



        protected override Vector2 GetStartLocation() => PopUpManager.Instance.GetPosition(_middleScreenPosition);


        protected override void Start()
        {
            if (PopUpManager.Instance == null) return;


            base.Start();
            Validator.OnCriticalError += ShowPopUp;
        }
        private void OnDestroy()
        {
            Validator.OnCriticalError -= ShowPopUp;
        }
        private void ShowPopUp(IValidFailedInfo valid)
        {
            HidePopUp();
            ShowPopUp();
            _currentActivePopUp.GetComponent<ValidationPopUpHandler>().AssignVisuals(valid, ClosePopUp);
        }

        private void ClosePopUp()
        {
            HidePopUp();
            _currentActivePopUp.Dispose();
        }

    }


}