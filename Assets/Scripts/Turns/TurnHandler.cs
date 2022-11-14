using CardMaga.Battle;
using CardMaga.Battle.Combo;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Turns
{

    public enum GameTurnType { EnterBattle, ExitBattle, LeftPlayerTurn, RightPlayerTurn }
    public class TurnHandler : IDisposable
    {
        public event Action OnGameTurnFinished;
        public event Action OnGameTurnStarted;
        public event Action<int> OnTurnCountChange;
        public event Action<TurnHandler> OnTurnHandlerDestroy;

        private Dictionary<GameTurnType, GameTurn> _gameTurnsDictionary;
        private int _turnCount;
        private TokenMachine _turnStarterTurnMachine;
        private bool _canChangeTurn;
        private bool _isLeftPlayerStart = false;
        private GameTurnType _currentTurn = GameTurnType.EnterBattle;

        public GameTurnType CurrentTurn { get => _currentTurn; private set => _currentTurn = value; } 
        public int TurnCount => _turnCount;
        public bool IsLeftCharacterTurn => CurrentTurn == GameTurnType.LeftPlayerTurn;
        public bool IsRightCharacterTurn => CurrentTurn == GameTurnType.RightPlayerTurn;
        public bool IsLeftPlayerStart => _isLeftPlayerStart;
        public ITokenReciever TurnChangeTokenMachine => _turnStarterTurnMachine;

        public TurnHandler(GameTurnType startGameTurnType) //will need to enter turn logic here 
        {
            _isLeftPlayerStart = startGameTurnType == GameTurnType.LeftPlayerTurn;

            _gameTurnsDictionary = new Dictionary<GameTurnType, GameTurn>()
            {
                { GameTurnType.EnterBattle,     new GameTurn(new NextTurn(startGameTurnType, 0)) },
                { GameTurnType.LeftPlayerTurn,  new GameTurn(new NextTurn( GameTurnType.RightPlayerTurn, 0))},
                { GameTurnType.RightPlayerTurn, new GameTurn(new NextTurn( GameTurnType.LeftPlayerTurn, 0)) },
                { GameTurnType.ExitBattle,      new GameTurn(null) },
            };
            _turnStarterTurnMachine = new TokenMachine(LockTurnChanging, UnlockTurnChanging);

            CurrentTurn = GameTurnType.EnterBattle;
            _gameTurnsDictionary[GameTurnType.ExitBattle].OnTurnActive += FinishGame;
            _gameTurnsDictionary[GameTurnType.EnterBattle].OnTurnActive += MoveToNextTurn;
            _gameTurnsDictionary[GameTurnType.LeftPlayerTurn].OnTurnEnter += AddTurnCount;
            _gameTurnsDictionary[GameTurnType.RightPlayerTurn].OnTurnEnter += AddTurnCount;

        }
        public GameTurn GetTurn(GameTurnType type) => _gameTurnsDictionary[type];
        public GameTurn GetCharacterTurn(bool isLeftCharacter) => GetTurn(isLeftCharacter ? GameTurnType.LeftPlayerTurn : GameTurnType.RightPlayerTurn);
        public void MoveToNextTurn()
        {
            if (!_canChangeTurn)
                return;

            var currentTurn = GetTurn(CurrentTurn);

            var nextTurn = GetTurn(currentTurn.GetNextTurn());

            if (nextTurn == null)
                currentTurn?.Exit(null);
            else
            {
                CurrentTurn = currentTurn.GetNextTurn();
                currentTurn?.Exit(EnterNextState);
            }
        }


        public void Start()
        {
            UnlockTurnChanging();
            GetTurn(CurrentTurn).Enter(TurnChangeTokenMachine);
        }


        public void Dispose()
        {
            OnTurnHandlerDestroy?.Invoke(this);

            foreach (var item in _gameTurnsDictionary)
                item.Value.Dispose();

            _gameTurnsDictionary[GameTurnType.EnterBattle].OnTurnActive -= MoveToNextTurn;
            _gameTurnsDictionary[GameTurnType.ExitBattle].OnTurnActive -= FinishGame;
            _gameTurnsDictionary[GameTurnType.LeftPlayerTurn].OnTurnEnter -= AddTurnCount;
            _gameTurnsDictionary[GameTurnType.RightPlayerTurn].OnTurnEnter -= AddTurnCount;

            _gameTurnsDictionary.Clear();
        }

        private void EnterNextState() => GetTurn(CurrentTurn).Enter(_turnStarterTurnMachine);
        private void FinishGame() => OnGameTurnFinished?.Invoke();
        private void LockTurnChanging() => _canChangeTurn = false;
        private void UnlockTurnChanging() => _canChangeTurn = true;
        private void AddTurnCount()
        {
            _turnCount++;
            OnTurnCountChange?.Invoke(_turnCount);
        }
    }
    public class NextTurn : IComparable<NextTurn>
    {
        private GameTurnType _gameTurnType;
        private int _weight;

        public int Weight => _weight;
        public GameTurnType GameTurnType => _gameTurnType;

        public NextTurn(GameTurnType gameTurnType, int weight)
        {
            _gameTurnType = gameTurnType;
            _weight = weight;
        }

        public int CompareTo(NextTurn other)
        {
            if (Weight < other.Weight)
                return -1;
            else if (Weight > other.Weight)
                return 1;
            else
                return 0;
        }
    }




    public class GameTurn : IDisposable
    {
        public static event Action OnTurnStarted;
        public static event Action OnTurnFinished;
        public event Action OnTurnEnter;
        public event Action OnTurnActive;
        public event Action OnTurnExit;



        private CardMaga.SequenceOperation.SequenceHandler _startTurnOperations;
        private SequenceHandler _endTurnOperations;
        private List<NextTurn> _nextTurn;
        private NextTurn _defaultNextTurn;

        protected TokenMachine _tokenMachine;
        public SequenceHandler StartTurnOperations { get => _startTurnOperations; }
        public SequenceHandler EndTurnOperations { get => _endTurnOperations; }

        public GameTurn(NextTurn defaultNextTurn)
        {
            _defaultNextTurn = defaultNextTurn;
            _startTurnOperations = new SequenceHandler();
            _endTurnOperations = new SequenceHandler();
            _nextTurn = new List<NextTurn>();
        }

        public GameTurnType GetNextTurn()
        {
            NextTurn nextTurn = _defaultNextTurn;
            if (_nextTurn.Count >= 0)
            {
                for (int i = 0; i < _nextTurn.Count; i++)
                {
                    if (nextTurn.Weight < _nextTurn[i].Weight)
                        nextTurn = _nextTurn[i];
                }
            }
            return nextTurn.GameTurnType;
        }


        public void AddNextTurn(NextTurn nextTurn)
        {
            _nextTurn.Add(nextTurn);
            _nextTurn.Sort();
        }
        public void RemoveNextTurn(NextTurn nextTurn) => _nextTurn.Remove(nextTurn);
        public virtual void Enter(ITokenReciever tokenMachine)
        {
            IDisposable token = tokenMachine.GetToken();
            OnTurnEnter?.Invoke();
            OnTurnStarted?.Invoke();

            _tokenMachine = new TokenMachine(TurnActive);
            IDisposable t = _tokenMachine.GetToken();
            using (t)
            {
                StartTurnOperations.ExecuteTask(_tokenMachine);
                token.Dispose();
            }
        }

        public virtual void Exit(Action OnComplete)
        {
            OnTurnExit?.Invoke();
            _tokenMachine = new TokenMachine(Complete);
            EndTurnOperations.ExecuteTask(_tokenMachine);

            void Complete()
            {

                OnTurnFinished?.Invoke();
                OnComplete?.Invoke();
                ResetNextTurns();
            }
        }

        private void TurnActive() => OnTurnActive?.Invoke();
        private void ResetNextTurns() => _nextTurn.Clear();

        public void Dispose()
        {
            StartTurnOperations.OnDestroy();
            EndTurnOperations.OnDestroy();
        }
    }


    public class EndTurnHandler : IDisposable
    {
        public event Func<bool> IsCharacterPlayingIdleAnimation;
        public event Func<bool> IsFinishedVisualAnimationCommands;
        private MonoBehaviour _sceneObject;
        private readonly TokenMachine _endTurnTokenMachine;
        private readonly IPlayer _player;
        private readonly ComboManager _comboManager;
        private IDisposable _endTurnToken;
        private Coroutine _staminaCoroutine;
        public EndTurnHandler(IPlayer player, IBattleManager ibattleManager)
        {

            _comboManager = ibattleManager.ComboManager;
            _player = player;
            _endTurnTokenMachine = new TokenMachine(ibattleManager.TurnHandler.MoveToNextTurn);
            _sceneObject = ibattleManager.MonoBehaviour;
            _player.MyTurn.OnTurnActive += StartTurn;
            _player.StaminaHandler.OnStaminaDepleted += StaminaIsEmpty;
        }
        private void StartTurn()
        {
            _endTurnToken = _endTurnTokenMachine.GetToken();
            if (_staminaCoroutine != null)
                _sceneObject.StopCoroutine(_staminaCoroutine);
        }
        private void ForceEndTurn()
        {
            _endTurnToken?.Dispose();
            if (_staminaCoroutine != null)
                _sceneObject.StopCoroutine(_staminaCoroutine);
        }
        private void StaminaIsEmpty()
        {
            if (_staminaCoroutine != null)
                _sceneObject.StopCoroutine(_staminaCoroutine);

            _sceneObject.StartCoroutine(CheckStaminaEndTurn());
        }

        public void EndTurnPressed()
        {
            _sceneObject.StartCoroutine(EndTurnCoroutine());
        }
        private IEnumerator EndTurnCoroutine()
        {
            yield return null;

            while (!IsExecutionAquiring && IsFinishedDetectingCombo && !IsAnimationFinished)
            {
                yield return null;
            }
            //   yield return new WaitForSeconds(1f);
            ForceEndTurn();
        }
        private IEnumerator CheckStaminaEndTurn()
        {
            bool check = true;

            yield return null;

            do
            {
                check = IsStaminaIsZero;
                if (!check)
                    yield break;

                yield return null;

                check &= !IsExecutionAquiring && IsFinishedDetectingCombo && IsAnimationFinished;

            } while (!check);
            yield return new WaitForSeconds(.35f);
            ForceEndTurn();
        }

        public void Dispose()
        {
            if (_player != null)
            {
                _player.MyTurn.OnTurnActive -= StartTurn;
                _player.StaminaHandler.OnStaminaDepleted -= StaminaIsEmpty;
            }
        }
        private bool IsAnimationFinished => IsCharacterPlayingIdleAnimation?.Invoke() ?? true; //_player.VisualCharacter.AnimatorController.IsCurrentlyIdle;
        private bool IsFinishedDetectingCombo => !_comboManager.IsTryingToDetect;
        private bool IsExecutionAquiring => !(IsFinishedVisualAnimationCommands?.Invoke() ?? false);
        public bool IsStaminaIsZero => !_player.StaminaHandler.HasStamina;
    }
}