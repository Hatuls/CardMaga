using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;

namespace Battle.Turns
{

    //public static class TurnHandler
    //{
    //    public static Action<int> OnTurnCountChange;
    //    public static event Action OnFinishTurn;
    //    private static int _turnCount = 0;
    //    private static TurnState _currectState;

    //    private static Dictionary<TurnState, Turn> _turnDict = new Dictionary<TurnState, Turn>()
    //            {
    //             { TurnState.Startbattle, new StartBattle() },
    //             { TurnState.StartEnemyTurn, new StartEnemyTurn() },
    //             { TurnState.EnemyTurn, new EnemyTurn() },
    //             { TurnState.EndEnemyTurn, new EndEnemyTurn() },
    //             { TurnState.StartPlayerTurn, new StartPlayerTurn() },
    //             { TurnState.PlayerTurn, new PlayerTurn() },
    //             { TurnState.EndPlayerTurn, new EndPlayerTurn() },
    //             { TurnState.EndBattle, new EndBattle() },
    //             { TurnState.NotInBattle, null },
    //            };

    //    public static int TurnCount
    //    {
    //        get => _turnCount;
    //        set
    //        {
    //            _turnCount = value; OnTurnCountChange?.Invoke(_turnCount);
    //        }
    //    }
    //    public static TurnState CurrentStateID
    //    {

    //        get => _currectState;
    //        private set
    //        {
    //            if (_currectState != value || BattleManager.isGameEnded)
    //            {
    //                _currectState = value;
    //            }

    //        }
    //    }
    //    public static bool IsTurnFinished { get; set; }
    //    public static bool IsPlayerTurn
    //    {
    //        get
    //        {
    //            var State = TurnHandler.CurrentStateID;
    //            return (State == TurnState.PlayerTurn || State == TurnState.StartPlayerTurn || State == TurnState.EndPlayerTurn);

    //        }
    //    }
    //    public static void FinishTurn()
    //    {
    //        IsTurnFinished = true;
    //        OnFinishTurn?.Invoke();
    //    }
    //    public static void MoveToNextTurn(TurnState turnState)
    //    {
    //        Debug.Log("Current Turn State is " + CurrentStateID);
    //        CurrentStateID = turnState;
    //        Debug.Log("After update Turn State is " + CurrentStateID);
    //    }
    //    public static IEnumerator TurnCycle()
    //    {
    //        TurnState turn = TurnState.NotInBattle;
    //        CurrentStateID = TurnState.Startbattle;
    //        do
    //        {
    //            if (turn != CurrentStateID)
    //            {
    //                turn = CurrentStateID;
    //                yield return _turnDict[turn].PlayTurn();
    //            }
    //        }
    //        while (!BattleManager.isGameEnded);

    //        CurrentStateID = TurnState.EndBattle;
    //        yield return _turnDict[CurrentStateID].PlayTurn();
    //    }


    //    public static void MoveNext()
    //    {

    //    }

    //    internal static void CheckPlayerTurnForAvailableAction()
    //    {
    //        if (CurrentStateID != TurnState.PlayerTurn && BattleManager.isGameEnded == false)
    //            return;

    //        bool noMoreActionAvailable = StaminaHandler.Instance.PlayerStamina.HasStamina == false && CardExecutionManager.CardsQueue.Count == 0;

    //        if (noMoreActionAvailable)
    //        {

    //            EndTurnButton.FinishTurn();
    //        }
    //    }
    //}

    //public enum TurnState
    //{
    //    NotInBattle,
    //    Startbattle,
    //    StartPlayerTurn,
    //    PlayerTurn,
    //    EndPlayerTurn,
    //    StartEnemyTurn,
    //    EnemyTurn,
    //    EndEnemyTurn,
    //    EndBattle
    //};

    //public interface ITurnHandler
    //{
    //    TurnState GetNextTurn { get; }
    //    IEnumerator PlayTurn();


    //}
    //public abstract class Turn : ITurnHandler
    //{
    //    public static WaitForSeconds WaitOneSecond = new WaitForSeconds(1f);

    //    public Turn()
    //    {


    //    }

    //    public abstract TurnState GetNextTurn { get; }
    //    public virtual IEnumerator PlayTurn()
    //    {
    //        if (BattleManager.isGameEnded)
    //            yield break;
    //    }
    //    protected void MoveToNextTurnState()
    //        => TurnHandler.MoveToNextTurn(GetNextTurn);

    //}
    //public class StartBattle : Turn
    //{
    //    public StartBattle() : base()
    //    {
    //    }

    //    public override TurnState GetNextTurn => TurnState.StartPlayerTurn;

    //    public override IEnumerator PlayTurn()
    //    {
    //        TurnHandler.TurnCount = 0;
    //        base.PlayTurn();
    //        yield return null;
    //        MoveToNextTurnState();
    //    }

    //}
    //public class StartEnemyTurn : Turn
    //{
    //    public StartEnemyTurn() : base()
    //    {
    //    }

    //    public override TurnState GetNextTurn => TurnState.EnemyTurn;

    //    public override IEnumerator PlayTurn()
    //    {
    //        bool isPlayerTurn = false;
    //        base.PlayTurn();
    //        //redoo
    //        //  yield return KeywordManager.Instance.OnStartTurnKeywords(isPlayerTurn);

    //        yield return null;
    //        MoveToNextTurnState();
    //    }

    //}
    //public class EnemyTurn : Turn
    //{
    //    public static event Action OnStartTurn;
    //    public EnemyTurn() : base()
    //    {
    //    }

    //    public override TurnState GetNextTurn => TurnState.EndEnemyTurn;

    //    public override IEnumerator PlayTurn()
    //    {
    //        bool isPlayerTurn = false;
    //        Deck.DeckManager.Instance.DrawHand(
    //            isPlayerTurn,
    //            CharacterStatsManager.GetCharacterStatsHandler(isPlayerTurn).GetStats(KeywordTypeEnum.Draw).Amount
    //            );
    //        OnStartTurn?.Invoke();
    //        StaminaHandler.Instance.OnStartTurn(isPlayerTurn);

    //        Debug.Log("Enemy Drawing Cards!");

    //        base.PlayTurn();
    //        // Activate Previous Action if not null
    //        yield return null;

    //        //redo
    //        //if (!KeywordManager.Instance.IsCharcterIsStunned(isPlayerTurn))
    //        //    yield return EnemyManager.Instance.PlayEnemyTurn();

    //        MoveToNextTurnState();
    //    }

    //}
    //public class EndEnemyTurn : Turn
    //{
    //    public static event Action OnEndEnemyTurn;
    //    public EndEnemyTurn() : base()
    //    {
    //    }

    //    public override TurnState GetNextTurn => TurnState.StartPlayerTurn;

    //    public override IEnumerator PlayTurn()
    //    {
    //        /*
    //         * Activate the enemy keywords 
    //         * Remove The Player Defense
    //         */
    //        //GameEventsInvoker.Instance.OnEndTurn?.Invoke();
    //        // CardUIManager.Instance.RemoveHands();
    //        CardExecutionManager.Instance.ResetExecution();
    //        base.PlayTurn();


    //        Deck.DeckManager.Instance.OnEndTurn(false);

    //        base.PlayTurn();
    //        yield return null;
    //        // redo!
    //        // KeywordManager.Instance.OnEndTurnKeywords(false);

    //        CharacterStatsManager.GetCharacterStatsHandler(true).GetStats(KeywordTypeEnum.Shield).Reset();

    //        if (OnEndEnemyTurn != null)
    //        {
    //            OnEndEnemyTurn.Invoke();
    //        }
    //        Managers.PlayerManager.Instance.OnEndTurn();
    //        EnemyManager.Instance.OnEndTurn();
    //        MoveToNextTurnState();
    //    }

    //}

    //public class StartPlayerTurn : Turn
    //{
    //    public StartPlayerTurn() : base()
    //    {
    //    }

    //    public override TurnState GetNextTurn => TurnState.PlayerTurn;

    //    public override IEnumerator PlayTurn()
    //    {
    //        TurnHandler.TurnCount++;
    //        bool isPlayerTurn = true;
    //        base.PlayTurn();
    //        yield return null;
    //        //redoooo
    //        // yield return KeywordManager.Instance.OnStartTurnKeywords(isPlayerTurn);

    //        Debug.Log("Player Drawing Cards!");
    //        MoveToNextTurnState();
    //    }

    //}
    //public class PlayerTurn : Turn
    //{
    //    public static event Action OnStartTurn;
    //    public PlayerTurn() : base()
    //    {
    //    }

    //    public override TurnState GetNextTurn => TurnState.EndPlayerTurn;

    //    public override IEnumerator PlayTurn()
    //    {
    //        base.PlayTurn();
    //        // unlock Player Inputs 
    //        TurnHandler.IsTurnFinished = false;
    //        bool isPlayerTurn = true;
    //        // redo!
    //        // if (!KeywordManager.Instance.IsCharcterIsStunned(isPlayerTurn))
    //        {

    //            StaminaHandler.Instance.OnStartTurn(isPlayerTurn);
    //            Deck.DeckManager.Instance.DrawHand(isPlayerTurn, CharacterStatsManager.GetCharacterStatsHandler(isPlayerTurn).GetStats(KeywordTypeEnum.Draw).Amount);
    //            OnStartTurn?.Invoke();
    //            do
    //            {
    //                yield return null;
    //            } while (!TurnHandler.IsTurnFinished);
    //        }
    //        MoveToNextTurnState();
    //    }

    //}
    //public class EndBattle : Turn
    //{
    //    public EndBattle() : base()
    //    {
    //    }

    //    public override TurnState GetNextTurn => TurnState.NotInBattle;

    //    public override IEnumerator PlayTurn()
    //    {
    //        base.PlayTurn();
    //        SendAnalyticData();
    //        yield return null;
    //    }

    //    private static void SendAnalyticData()
    //    {
    //        string turnCount = string.Concat("Finished Battle At Turn ", TurnHandler.TurnCount);
    //        UnityAnalyticHandler.SendEvent(turnCount);
    //        FireBaseHandler.SendEvent(turnCount);
    //    }
    //}
    //public class EndPlayerTurn : Turn
    //{

    //    public static event Action OnPlayerEndTurn;


    //    public EndPlayerTurn() : base()
    //    {
    //    }

    //    public override TurnState GetNextTurn => TurnState.StartEnemyTurn;

    //    public override IEnumerator PlayTurn()
    //    {
    //        /*
    //         * we first remove the hands cards
    //         * then we pass the CardExecutionManager all the cards we currently have in the placementdeck
    //         * we give the animator the current cards so he can play the animations
    //         * each animation that finished tell the CardExecutionManager to execute the keyword effects of the current card
    //         * and remove the current card from the placementslot
    //        */
    //        //  GameEventsInvoker.Instance.OnEndTurn?.Invoke();
    //        StaminaHandler.Instance.OnEndTurn(true);
    //        Deck.DeckManager.Instance.OnEndTurn(true);
    //        OnPlayerEndTurn?.Invoke();
    //        CardExecutionManager.Instance.ResetExecution();
    //        base.PlayTurn();

    //        //redoooo!
    //        //   yield return KeywordManager.Instance.OnEndTurnKeywords(true);
    //        yield return null;
    //        CharacterStatsManager.GetCharacterStatsHandler(false).GetStats(KeywordTypeEnum.Shield).Reset();


    //        Managers.PlayerManager.Instance.OnEndTurn();
    //        EnemyManager.Instance.OnEndTurn();
    //        MoveToNextTurnState();
    //    }

    //}




    public enum GameTurnType { EnterBattle, ExitBattle, LeftPlayerTurn, RightPlayerTurn }
    public class GameTurnHandler : IDisposable
    {
        public event Action OnGameTurnFinished;
        public event Action OnGameTurnStarted;
        public event Action<int> OnTurnCountChange;
        public event Action<GameTurnHandler> OnTurnHandlerDestroy;

        private Dictionary<GameTurnType, GameTurn> _gameTurnsDictionary;
        private int _turnCount;
        private TokenMachine _turnStarterTurnMachine;
        private bool _canChangeTurn;




        public GameTurnType CurrentTurn { get; private set; } = GameTurnType.EnterBattle;
        public int TurnCount => _turnCount;
        public bool IsLeftCharacterTurn => CurrentTurn == GameTurnType.LeftPlayerTurn;
        public bool IsRightCharacterTurn => CurrentTurn == GameTurnType.RightPlayerTurn;


        public GameTurnHandler() //will need to enter turn logic here 
        {
            _gameTurnsDictionary = new Dictionary<GameTurnType, GameTurn>()
            {
             //   { GameTurnType.EnterBattle,     new EnterBattle(MoveToNextTurn ,new NextTurn(/*Start Turn Logic Enter here*/ GameTurnType.LeftPlayerTurn, 0)) },
                { GameTurnType.EnterBattle,     new GameTurn(new NextTurn(/*Start Turn Logic Enter here*/ GameTurnType.LeftPlayerTurn, 0)) },
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

            CurrentTurn = currentTurn.GetNextTurn();

            var nextTurn = GetTurn(CurrentTurn);

            if (nextTurn == null)
                currentTurn?.Exit(null);
            else
                currentTurn?.Exit(EnterNextState);
        }


        public void Start()
        {
            UnlockTurnChanging();
            GetTurn(CurrentTurn).Enter(_turnStarterTurnMachine);
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


    //public class EnterBattle : GameTurn
    //{
    //    private event Action OnEnterFinished;

    //    public EnterBattle(Action onFinish, NextTurn nextTurn) : base(nextTurn)
    //    {
    //        OnEnterFinished = onFinish;
    //    }
    //    public override void Enter(ITokenReciever tokenMachine)
    //    {
    //        base.Enter(tokenMachine);
    //        OnEnterFinished?.Invoke();
    //    }

    //}


    public class GameTurn : IDisposable
    {
        public static event Action OnTurnStarted;
        public static event Action OnTurnFinished;
        public event Action OnTurnEnter;
        public event Action OnTurnActive;
        public event Action OnTurnExit;



        private SequenceHandler _startTurnOperations ;
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
}