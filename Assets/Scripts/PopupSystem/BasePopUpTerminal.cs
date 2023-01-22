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

        protected PopUp _currentActivePopUp;

        [SerializeField]
        protected PopUpScreenLocation[] _popUpScreenLocations;

        public abstract IPopUpTransition<AlphaData> TransitionAlphaIn { get; }
        public abstract IPopUpTransition<AlphaData> TransitionAlphaOut { get; }
        public abstract IPopUpTransition<TransitionData> TransitionIn { get; }
        public abstract IPopUpTransition<TransitionData> TransitionOut { get; }

        protected virtual void Awake()
        {
            PopUpManager.OnCloseAllPopUps += ResetPopUp;
        }

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
            _currentActivePopUp = current;
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

        private void OnDestroy()
        {
            ResetPopUp();

            if(_basePopUps!=null && _basePopUps.Count > 0)
            for (int i = 0; i < _basePopUps.Count; i++)
                _basePopUps[i].OnDisposed -= RemoveFromActiveList;
        }
    }
}