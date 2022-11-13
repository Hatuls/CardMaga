using CardMaga.UI;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.CinematicSystem
{
    public class CinematicManager : MonoBehaviour, IInitializable
    {
        #region Events
        [SerializeField] private ClickHelper _clickHelper;
        [SerializeField, EventsGroup] private UnityEvent OnCinematicSequenceStart;
        [SerializeField, EventsGroup] private UnityEvent OnCinematicSequencePause;
        [SerializeField, EventsGroup] private UnityEvent OnCinematicSequenceResume;
        [SerializeField, EventsGroup] private UnityEvent OnCinematicSequenceEnd;

        #endregion

        #region Fields

        [SerializeField] private CinematicHandler[] _cinematic;

        private Coroutine _currentRunningCinematic;
        private CinematicHandler _currentCinematic;
        private int _currentCinematicIndex;
        private bool _isPause;
        private IDisposable _token;

        public event Action OnInitializable;
        #endregion

        #region Prop

        public double GetTheCurrentCinematicTime
        {
            get => _currentCinematic.CinematicTime;
        }

        #endregion

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            _currentCinematicIndex = -1;

            _clickHelper.LoadAction(StartNextCinematic);

            for (int i = 0; i < _cinematic.Length; i++)
            {
                _cinematic[i].OnCinematicCompleted += CinematicComplete;
                _cinematic[i].Init(i, this);
            }
            OnInitializable?.Invoke();
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _cinematic.Length; i++)
            {
                _cinematic[i].OnCinematicCompleted -= CinematicComplete;
            }
        }

        #region Public Function
        public void StartCinematicSequence(ITokenReciever tokenReciever)
        {
            _token = tokenReciever?.GetToken();
            StartCinematicSequence();
        }
        public void StartCinematicSequence()
        {
            OnCinematicSequenceStart?.Invoke();
            _isPause = false;
            StartNextCinematic();
        }

        [ContextMenu("Resume Sequence")]
        public void ResumeCinematicSequence()
        {
            OnCinematicSequenceResume?.Invoke();
            _isPause = false;
            StartNextCinematic();
        }

        [ContextMenu("Pause Sequence")]
        public void PauseCinematicSequence()
        {
            if (_isPause)
                return;

            OnCinematicSequencePause?.Invoke();
            _isPause = true;
            SkipCurrentCinematic();
        }

        [ContextMenu("Skip Current Cinematic")]
        public void SkipCurrentCinematic()
        {
            _cinematic[_currentCinematicIndex].SkipCinematic();
        }

        public void StartCinematicByIndex(int index)
        {
            PauseCinematicSequence();

            _currentCinematic = _cinematic[index];
            _currentRunningCinematic = _currentCinematic.StartCinematic();
        }

        #endregion

        #region Private Function

        private void StartNextCinematic()
        {
            _currentCinematicIndex++;
            _currentCinematic = _cinematic[_currentCinematicIndex];
            _currentRunningCinematic = _currentCinematic.StartCinematic();
        }

        private void CinematicComplete(CinematicHandler cinematicHandler)
        {
            if (_currentCinematicIndex == _cinematic.Length - 1)
            {
                FinishedCinematic();
                return;
            }

            if (cinematicHandler.IsPuseCinematicSequenceOnEnd || _isPause)
            {
                PauseCinematicSequence();
                _clickHelper.Open();
            }
            else
                StartNextCinematic();
        }

        private void FinishedCinematic()
        {
            if (_token != null)
                _token.Dispose();

            OnCinematicSequenceEnd?.Invoke();
            Debug.Log("End");

        }

        #endregion
    }
}
