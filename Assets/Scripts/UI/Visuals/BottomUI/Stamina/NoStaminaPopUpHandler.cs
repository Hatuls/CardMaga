using System.Collections;
using UnityEngine;


namespace CardMaga.UI.PopUp
{

    public class NoStaminaPopUpHandler : BasePopUpTerminal
    {
        [SerializeField]
        private HandUI _handUI;
        [SerializeField, Range(0, 10f)]
        private float _duration;
        private WaitForSeconds _waitForSeconds;


        protected override Vector2 GetStartLocation() => PopUpManager.Instance.GetPosition(_startLocation);


        #region Monobehaviour Callbacks
        protected override  void Start()
        {
            if (PopUpManager.Instance == null)
                    return;
            PopUpManager.OnCloseAllPopUps += HidePopUp;
            _handUI.OnCardExecutionFailed += ShowPopUp;
            _handUI.OnCardExecutionSuccess += HidePopUp;
            _waitForSeconds = new WaitForSeconds(_duration);
            base.Start();
        }

        private void OnDestroy()
        {
            PopUpManager.OnCloseAllPopUps -= HidePopUp;
            _handUI.OnCardExecutionFailed -= ShowPopUp;
            _handUI.OnCardExecutionSuccess -= HidePopUp;
        }
        #endregion

        protected override void ShowPopUp()
        {
            base.ShowPopUp();
            _currentActivePopUp.PopUpTransitionHandler.TransitionOut.OnTransitionComplete += _currentActivePopUp.Dispose;
            StopAllCoroutines();
            StartCoroutine(Delay());
        }
        private IEnumerator Delay()
        {
            yield return _waitForSeconds;
            HidePopUp();
        }
        protected override void RemoveFromActiveList(PopUp obj)
        {
            obj.PopUpTransitionHandler.TransitionOut.OnTransitionComplete -= _currentActivePopUp.Dispose;
            base.RemoveFromActiveList(obj);
            StopAllCoroutines();
        }
    }


}