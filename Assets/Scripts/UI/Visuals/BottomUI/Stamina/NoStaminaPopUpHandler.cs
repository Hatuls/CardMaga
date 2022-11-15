using System.Collections;
using UnityEngine;


namespace CardMaga.UI.PopUp
{

    public class NoStaminaPopUpHandler :MonoBehaviour
    {
        [SerializeField]
        private HandUI _handUI;
        [SerializeField, Range(0, 10f)]
        private float _duration;

        [SerializeField]
        private NoStaminaPopUp _noStaminaPopUp;

        private Coroutine _timerCoroutine;
        #region Monobehaviour Callbacks
        private void Awake()
        {
            _noStaminaPopUp.Hide();
            _handUI.OnCardExecutionFailed += ShowPopUp;
            _handUI.OnCardExecutionSuccess += ForceFadeOutStaminaPopUp;
        }

        private void OnDestroy()
        {
            _handUI.OnCardExecutionFailed -= ShowPopUp;
            _handUI.OnCardExecutionSuccess -= ForceFadeOutStaminaPopUp;
            StopAllCoroutines();
        }
        #endregion

        private void ShowPopUp()
        {
            _noStaminaPopUp.Enter();
            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);

            _timerCoroutine = StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            float counter = 0;
            while (counter <= _duration)
            {
                yield return null;
                counter += Time.deltaTime;
            }
            FadeOutStaminaPopUp();
        }
        private void ForceFadeOutStaminaPopUp()
        {
            if (_noStaminaPopUp.IsActive())
            {
                if (_timerCoroutine != null)
                    StopCoroutine(_timerCoroutine);

                FadeOutStaminaPopUp();
            }
        }
        private void FadeOutStaminaPopUp() => _noStaminaPopUp.Close();
    }
}