using System.Collections;
using UnityEngine;


namespace CardMaga.UI.PopUp
{

    public class HandUIPopUpHandler :MonoBehaviour
    {
        [SerializeField]
        private HandUI _handUI;

        [SerializeField] private PopUpSO _popUpSO; //The ID of the popUp
        [SerializeField] private TransitionPackSO[] _enterPackSO;
        [SerializeField] private TransitionPackSO[] _exitPackSO;
        private TransitionData[] _enterTransitionData;
        private TransitionData[] _exitTransitionData;

        private BasePopUp _currentBasePopUp;

        #region Monobehaviour Callbacks
        private void Awake()
        {
            _handUI.OnCardExecutionFailed += ShowNoStaminaPopUp;
            
        }

        private void OnDestroy()
        {
            _handUI.OnCardExecutionFailed -= ShowNoStaminaPopUp;
        }

        private void InitNoStaminaTransitionData()
        {
            _enterTransitionData = new TransitionData[_enterPackSO.Length];
            for (int i = 0; i < _enterTransitionData.Length; i++)
            {
                _enterTransitionData[i] = new TransitionData(_enterPackSO[i], PopUpManager.Instance.ScreenMiddleLocation);
            }

            for (int i = 0; i < _enterTransitionData.Length; i++)
            {
                _currentBasePopUp.PopUpTransitionHandler.AddTransitionData(true, _enterTransitionData[i]);
            }



            _exitTransitionData = new TransitionData[_exitPackSO.Length];
            for (int i = 0; i < _exitTransitionData.Length; i++)
            {
                _exitTransitionData[i] = new TransitionData(_exitPackSO[i], PopUpManager.Instance.ScreenMiddleLocation);
            }

            for (int i = 0; i < _exitTransitionData.Length; i++)
            {
                _currentBasePopUp.PopUpTransitionHandler.AddTransitionData(false, _exitTransitionData[i]);
            }
        }
        #endregion

        private void ShowNoStaminaPopUp()
        {
            if(_currentBasePopUp==null)
            {
                _currentBasePopUp = PopUpManager.Instance.PoolHandler.Pull(_popUpSO);
                _currentBasePopUp.OnDisposed += ResertPopUp;
                InitNoStaminaTransitionData();
                _currentBasePopUp.Enter();
            }
        }

        private void ResertPopUp(BasePopUp basePopUp)
        {
            basePopUp.OnDisposed -= ResertPopUp;
            _currentBasePopUp = null;
        }
    }
}