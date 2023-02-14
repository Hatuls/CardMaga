using CardMaga.Battle.Players;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.UI.PopUp
{
    public abstract class BasePopUpTerminal : MonoBehaviour
    {
        [SerializeField]
        protected PopUpSO _popUpSO;

        private List<PopUp> _basePopUps = new List<PopUp>();

        [SerializeField]
        protected LocationTagSO _startLocation;
        [SerializeField]
        private float _startingScale;
        [SerializeField, Range(0, 1f)]
        private float _startingAlpha;


        [SerializeField]
        protected TransitionBuilder[] _transitionIn;
        [SerializeField]
        protected TransitionBuilder[] _transitionOut;


        protected IPopUpTransition<TransitionData> _popUpTransitionIn;
        protected IPopUpTransition<TransitionData> _popUpTransitionOut;
        protected IPopUpTransition<AlphaData> _popUpAlphaTransitionIn;
        protected IPopUpTransition<AlphaData> _popUpAlphaTransitionOut;
        protected PopUp _currentActivePopUp;

        [SerializeField]
        protected PopUpScreenLocation[] _popUpScreenLocations;

        public virtual IPopUpTransition<AlphaData> TransitionAlphaIn => _popUpAlphaTransitionIn;
        public virtual IPopUpTransition<AlphaData> TransitionAlphaOut => _popUpAlphaTransitionOut;
        public virtual IPopUpTransition<TransitionData> TransitionIn => _popUpTransitionIn;
        public virtual IPopUpTransition<TransitionData> TransitionOut => _popUpTransitionOut;
        protected virtual Transform Parent => transform;
        protected virtual void Start()
        {
            PopUpManager.OnCloseAllPopUps += ResetPopUp;
            _popUpTransitionIn = new BasicTransition(GenerateTransitionData(_transitionIn));
            _popUpTransitionOut = new BasicTransition(GenerateTransitionData(_transitionOut));
            _popUpAlphaTransitionIn = new AlphaTransition(GenerateAlphaTransitionData(_transitionIn));
            _popUpAlphaTransitionOut = new AlphaTransition(GenerateAlphaTransitionData(_transitionOut));
        }
        [ContextMenu("Show Pop Up")]
        protected virtual void ShowPopUp()
        {
            if (_popUpSO.IsStackable == false && _basePopUps.Count > 0)
                ResetPopUp();

            _currentActivePopUp = PopUpManager.Instance.PopUpPool.Pull(_popUpSO);
            _basePopUps.Add(_currentActivePopUp);
            _currentActivePopUp.OnDisposed += RemoveFromActiveList;
            _currentActivePopUp.gameObject.SetActive(true);
            SetMovementTransitions(_currentActivePopUp.PopUpTransitionHandler);
            SetAlphaTransitions(_currentActivePopUp.PopUpAlphaHandler);
            _currentActivePopUp.transform.localScale = GetStartScale();
            _currentActivePopUp.transform.SetParent(Parent);
            _currentActivePopUp.Enter();
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
                {
                    if (_startLocation == tagSO)
                        cache[i] = new TransitionData(transitionBuilder[i].TransitionPackSO, GetStartLocation);
                    else
                    {
                        for (int j = 0; j < _popUpScreenLocations.Length; j++)
                        {
                            if (_popUpScreenLocations[j].ContainTag(tagSO))
                                cache[i] = new TransitionData(transitionBuilder[i].TransitionPackSO, _popUpScreenLocations[j].Location);
                        }
                    }
                }

            }
            return cache;
        }


        protected abstract Vector2 GetStartLocation();
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
            {
                _basePopUps[i].Dispose();
   
            }

            _basePopUps.Clear();
        }

        protected virtual void OnDestroy()
        {
            ResetPopUp();

            if(_basePopUps!=null && _basePopUps.Count > 0)
            for (int i = 0; i < _basePopUps.Count; i++)
                _basePopUps[i].OnDisposed -= RemoveFromActiveList;
        }
    }
}