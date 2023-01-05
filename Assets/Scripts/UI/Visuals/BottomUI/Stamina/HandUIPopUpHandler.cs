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
            //_handUI.OnCardExecutionSuccess += ForceFadeOutStaminaPopUp;
            transitionData = new TransitionData[]
            {
                new TransitionData(packSO[0],PopUpManager.Instance.ScreenLeftLocation),
                new TransitionData(packSO[1],PopUpManager.Instance.ScreenRightLocation),
            };
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
            popUp.PopUpTransitionHandler.AddTransitionData(true, transitionData[0]);
            popUp.PopUpTransitionHandler.AddTransitionData(true, transitionData[1]);
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