using System.Collections;
using UnityEngine;


namespace CardMaga.UI.PopUp
{

    public class HandUIPopUpHandler :MonoBehaviour
    {
        [SerializeField]
        private HandUI _handUI;
        [SerializeField, Range(0, 10f)]
        private float _duration;

        [SerializeField] PopUpSO _popUpSO;
        [SerializeField] TransitionPackSO[] packSO;
        private TransitionData[] transitionData; 



        private Coroutine _timerCoroutine;
        #region Monobehaviour Callbacks
        private void Awake()
        {
            _handUI.OnCardExecutionFailed += ShowPopUp;
            transitionData = new TransitionData[packSO.Length];
            for (int i = 0; i < transitionData.Length; i++)
            {
                transitionData[i] = new TransitionData(packSO[i], PopUpManager.Instance.ScreenMiddleLocation);
            }
        }

        private void Start()
        {
            
        }

        private void OnDestroy()
        {
            _handUI.OnCardExecutionFailed -= ShowPopUp;
            //_handUI.OnCardExecutionSuccess -= ForceFadeOutStaminaPopUp;
            StopAllCoroutines();
        }
        #endregion

        private void ShowPopUp()
        {
            //basicFadeInPopup.Enter();
            //if (_timerCoroutine != null)
            //    StopCoroutine(_timerCoroutine);

            //_timerCoroutine = StartCoroutine(Timer());
            BasePopUp popUp = PopUpManager.Instance.PoolHandler.Pull(_popUpSO);
            for (int i = 0; i < transitionData.Length; i++)
            {
                popUp.PopUpTransitionHandler.AddTransitionData(true, transitionData[i]);
            }
            popUp.Enter();
        }

        //private IEnumerator Timer()
        //{
        //    float counter = 0;
        //    while (counter <= _duration)
        //    {
        //        yield return null;
        //        counter += Time.deltaTime;
        //    }
        //    FadeOutStaminaPopUp();
        //}
        //private void ForceFadeOutStaminaPopUp()
        //{
        //    if (basicFadeInPopup.IsActive())
        //    {
        //        if (_timerCoroutine != null)
        //            StopCoroutine(_timerCoroutine);

        //        FadeOutStaminaPopUp();
        //    }
        //}
        //private void FadeOutStaminaPopUp() => basicFadeInPopup.Close();
    }
}