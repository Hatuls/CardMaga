using CardMaga.Battle.Players;
using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] TransitionBuilder[] _enterPack;
        [SerializeField] TransitionBuilder[] _exitPack;

        private IPopUpTransition<TransitionData> _enterTransition;
        private IPopUpTransition<TransitionData> _exitTransition;
        private IPopUpTransition<AlphaData> _enterAlphaTransition;
        private IPopUpTransition<AlphaData> _exitAlphaTransition;

        public override IPopUpTransition<TransitionData> TransitionIn => _enterTransition;
        public override IPopUpTransition<TransitionData> TransitionOut => _exitTransition;

        public override IPopUpTransition<AlphaData> TransitionAlphaIn => _enterAlphaTransition;

        public override IPopUpTransition<AlphaData> TransitionAlphaOut => _exitAlphaTransition;

        protected override Vector3 GetStartLocation() => PopUpManager.Instance.GetPosition(_startLocation);


        #region Monobehaviour Callbacks
        private void Awake()
        {
            _handUI.OnCardExecutionFailed += ShowPopUp;
            _handUI.OnCardExecutionSuccess += HidePopUp;

            _waitForSeconds = new WaitForSeconds(_duration);
            _enterAlphaTransition = new AlphaTransition(GenerateAlphaTransitionData(_enterPack));
            _exitAlphaTransition = new AlphaTransition(GenerateAlphaTransitionData(_exitPack));
            _enterTransition = new BasicTransition(GenerateTransitionData(_enterPack));
            _exitTransition = new BasicTransition(GenerateTransitionData(_exitPack));
        }

        private void OnDestroy()
        {
            _handUI.OnCardExecutionFailed -= ShowPopUp;
            _handUI.OnCardExecutionSuccess -= HidePopUp;
        }
        #endregion

        protected override void ShowPopUp()
        {
            base.ShowPopUp();
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
            base.RemoveFromActiveList(obj);
            StopAllCoroutines();
        }
        protected TransitionData[] GenerateTransitionData(TransitionBuilder[] transitionBuilder)
        {
            var popUpManager = PopUpManager.Instance;
            int length = transitionBuilder.Length;

            TransitionData[] cache = new TransitionData[length];
            for (int i = 0; i < length; i++)
            {
                if (transitionBuilder[i].TagSO == null || transitionBuilder[i].TransitionPackSO == null)
                    continue;

                TagSO tagSO = transitionBuilder[i].TagSO;

                if (popUpManager.ContainTag(tagSO))
                    cache[i] = new TransitionData(transitionBuilder[i].TransitionPackSO, () => popUpManager.GetPosition(tagSO));
                else
                    cache[i] = new TransitionData(transitionBuilder[i].TransitionPackSO, GetObjectsDestination);

            }
            return cache;
        }
        protected AlphaData[] GenerateAlphaTransitionData(TransitionBuilder[] transitionBuilders)
        {
            int length = transitionBuilders.Length;
            AlphaData[] cache = new AlphaData[length];
            for (int i = 0; i < length; i++)
            {
                TransitionBuilder currnet = transitionBuilders[i];
                cache[i] = new AlphaData(currnet.Alpha, currnet.Duration, currnet.AlphaCurve);
            }
            return cache;
        }



        protected virtual Vector2 GetObjectsDestination() => transform.position;

  
    }

    public abstract class BasePopUpTerminal : MonoBehaviour
    {
        [SerializeField]
        protected PopUpSO _popUpSO;

        private List<PopUp> _basePopUps = new List<PopUp>();

        [SerializeField]
        protected TagSO _startLocation;
        [SerializeField]
        private float _startingScale;
        [SerializeField, Range(0, 1f)]
        private float _startingAlpha;

        

        public abstract IPopUpTransition<AlphaData> TransitionAlphaIn { get; }
        public abstract IPopUpTransition<AlphaData> TransitionAlphaOut { get; }
        public abstract IPopUpTransition<TransitionData> TransitionIn { get; }
        public abstract IPopUpTransition<TransitionData> TransitionOut { get; }

        protected virtual void ShowPopUp()
        {
            if (_popUpSO.IsStackable == false && _basePopUps.Count > 0)
                ResetPopUp();

            var current = PopUpManager.Instance.PopUpPool.Pull(_popUpSO);
            _basePopUps.Add(current);
            current.OnDisposed += RemoveFromActiveList;
            current.gameObject.SetActive(true);
            SetMovementTransitions(current.PopUpTransitionHandler);
            SetAlphaTransitions(current.PopUpAlphaHandler);
            current.transform.localScale = GetStartScale();
            current.transform.SetParent(transform);
            current.Enter();

        }

        private void SetAlphaTransitions(PopUpAlphaHandler popUpAlphaHandler)
        {
            popUpAlphaHandler.SetStartingAlpha(GetStartAlpha());
            popUpAlphaHandler.EnterAlphaTransitions = TransitionAlphaIn;
            popUpAlphaHandler.ExitAlphaTransitions = TransitionAlphaOut;
        }

        private void SetMovementTransitions(PopUpTransitionHandler popUpTransitionHandler)
        {
            popUpTransitionHandler.AssignStartLocation(GetStartLocation());
            popUpTransitionHandler.TransitionIn = TransitionIn;
            popUpTransitionHandler.TransitionOut = TransitionOut;
        }

        protected virtual void HidePopUp()
        {
            for (int i = 0; i < _basePopUps.Count; i++)
                _basePopUps[i].Close();
        }
        protected abstract Vector3 GetStartLocation();
        protected float GetStartAlpha() => _startingAlpha;
        protected Vector3 GetStartScale() => _startingScale * Vector3.one;
        protected virtual void RemoveFromActiveList(PopUp obj)
        {
            _basePopUps.Remove(obj);
            obj.OnDisposed -= RemoveFromActiveList;
        }
        public void ResetPopUp()
        {
            for (int i = 0; i < _basePopUps.Count; i++)
                _basePopUps[i].Dispose();

            _basePopUps.Clear();
        }
    }
}