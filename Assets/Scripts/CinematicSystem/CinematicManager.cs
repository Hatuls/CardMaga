using CardMaga.UI;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.CinematicSystem
{
    public class CinematicManager : MonoBehaviour, IInitializable
    {
        #region Events

        public event Action OnCinematicEnd;
        public event Action OnCinematicStart;
        public event Action OnCinematicPause;
        public event Action OnCinematicResume;

        [SerializeField, EventsGroup] private UnityEvent OnCinematicSequenceStart;
        [SerializeField, EventsGroup] private UnityEvent OnCinematicSequencePause;
        [SerializeField, EventsGroup] private UnityEvent OnCinematicSequenceResume;
        [SerializeField, EventsGroup] private UnityEvent OnCinematicSequenceEnd;

        #endregion

        #region Fields

        [SerializeField] private ClickHelper _clickHelper;
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

        public void StartCinematicSequence(ITokenReceiver tokenReciever)
        {
            _token = tokenReciever?.GetToken();
            StartCinematicSequence();
        }

        public void StartCinematicSequence()
        {
            OnCinematicSequenceStart?.Invoke();
            OnCinematicStart?.Invoke();
            _isPause = false;
            StartFirstCinematic();
        }

        [ContextMenu("Resume Sequence")]
        public void ResumeCinematicSequence()
        {
            OnCinematicSequenceResume?.Invoke();
            OnCinematicResume?.Invoke();
            _isPause = false;
            if (_currentCinematic.IsCompleted)
                StartNextCinematic();
            else
                _currentCinematic.ResumeCinematic();
        }
        [ContextMenu("Reset All Cinematic")]
        public void ResetAll()
        {
            if (_currentRunningCinematic != null)
                StopCoroutine(_currentRunningCinematic);

            for (int i = 0; i < _cinematic.Length; i++)
                _cinematic[i].Reset();
        }
        [ContextMenu("Pause Sequence")]
        public void PauseCinematicSequence()
        {
            if (_isPause)
                return;
            _isPause = true;
            StopCoroutine(Pause());
            StartCoroutine(Pause());
            //if (_clickHelper != null)
            //    _clickHelper.Open();

            IEnumerator Pause()
            {
                
                yield return null;
                OnCinematicSequencePause?.Invoke();
                OnCinematicPause?.Invoke();
                _currentCinematic.PauseCinematic();
                _clickHelper.Open(ResumeCinematicSequence);
            }
        }

        [ContextMenu("Skip Current Cinematic")]
        public void SkipCurrentCinematic()
        {
            _currentCinematic.SkipCinematic();
        }

        #endregion

        #region Private Function

        private void StartNextCinematic()
        {
            if (_isPause)
                return;

            _currentCinematicIndex++;

            if (_currentCinematicIndex >= _cinematic.Length)
            {
                FinishedCinematic();
                return;
            }

            RunCinematic();
        }

        private void StartFirstCinematic()
        {
            _currentCinematicIndex = 0;
            RunCinematic();
        }

        private void RunCinematic()
        {
            _currentCinematic = _cinematic[_currentCinematicIndex];
            _clickHelper.Open(SkipCurrentCinematic);
            _currentRunningCinematic = _currentCinematic.StartCinematic();
        }

        private void CinematicComplete(CinematicHandler cinematicHandler)
        {
            if (cinematicHandler.IsPuseCinematicSequenceOnEnd || _isPause)
            {
                PauseCinematicSequence();
            }
            else
                StartNextCinematic();
        }

        private void FinishedCinematic()
        {
            if (_token != null)
                _token.Dispose();

            OnCinematicEnd?.Invoke();
            OnCinematicSequenceEnd?.Invoke();
            Debug.Log("End");

        }




        #endregion
    }
}
