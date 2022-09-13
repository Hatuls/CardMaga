using Battle;
using Battle.Turns;
using DG.Tweening;
using Managers;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
public class TurnCounter : MonoBehaviour, ISequenceOperation<IBattleManager>
{
    private static TokenMachine _timerTokenMachine;
    public static event Action OnCounterDepleted;
    public static ITokenReciever TimerTokenMachine => _timerTokenMachine;

    public int Priority => 0;

    [SerializeField]
    private TextMeshProUGUI _timerText;
    [SerializeField]
    private RectTransform _textScaler;
    [SerializeField, Tooltip("The Turn Time (when depeleted it will end the turn)")]
    private float _timeTillEndTurn;


    [SerializeField, Tooltip("The Color when the timer is under the limit time")]
    private Color _lowTextColor;
    [SerializeField, Tooltip("The Color when the timer is above the limit time")]
    private Color _highTextColor;


    [SerializeField, Min(0), Tooltip("When the counter is lower than this it will trigger the animation and color")]
    private int _textWarningStart = 0;
    [SerializeField]
    private TransitionPackSO _timerTextTransition;



    private GameTurnHandler _gameTurnHandler;
    private int _textTime;
    private float _counter;
    private bool _toStopTimer;
    private float _startScale;
    private Sequence _sequence;
    private Coroutine _coroutineTimer;


    private void Awake()
    {
        _startScale = _timerText.rectTransform.localScale.x;
        BattleManager.Register(this, OrderType.Default);


        GameTurn.OnTurnFinished += FinishCounting;
        _timerTokenMachine = new TokenMachine(StopTimer, ContinueTimer);
        ResetTimer();
        ResetScale();
    }




    public void StartTime()
    {
        ResetScale();
        ResetTimer();
        if (_coroutineTimer != null)
            return;

        _coroutineTimer = StartCoroutine(Count());
        _timerText.gameObject.SetActive(true);
    }
    private void ResetScale()
    {
        if (_sequence == null)
            _sequence.Kill();
        _textScaler.localScale = _startScale * Vector3.one;
    }
    private void ResetTimer()
    {
        _counter = _timeTillEndTurn;
    }
    private IEnumerator Count()
    {
        while (_counter > 0)
        {
            if (!_toStopTimer)
            {
                _counter -= Time.deltaTime;
                AssignTime(_counter);
            }
            yield return null;
        }
        FinishCounting();
        OnCounterDepleted?.Invoke();

    }
    private void AssignTime(float time)
    {
        int roundTime = Mathf.RoundToInt(time);
        if (_textTime == roundTime)
            return;

        _textTime = roundTime;
        //  do somet cool stuff
        ResetScale();
        bool isTimeRunningLow = _textTime <= _textWarningStart;
        Color clr = _highTextColor;
        if (isTimeRunningLow)
        {
            clr = _lowTextColor;
            _sequence = _textScaler.Transition(_timerTextTransition);
        }

        _timerText.text = _textTime.ToString("00").ColorString(clr);
    }
    private void FinishCounting()
    {
        if (_coroutineTimer != null)
            StopCoroutine(_coroutineTimer);

        _coroutineTimer = null;
        _timerText.gameObject.SetActive(false);
    }
    private void ContinueTimer() => _toStopTimer = false;
    private void StopTimer() => _toStopTimer = true;
    private void StartTurn()
    {
        ContinueTimer();
        StartTime();
    }
    public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
    {
        using (tokenMachine.GetToken())
        {
            var turn = data.TurnHandler;
            OnCounterDepleted += turn.MoveToNextTurn;
            var leftTurn = turn.GetTurn(GameTurnType.LeftPlayerTurn);
            leftTurn.OnTurnExit += StopTimer;
            leftTurn.OnTurnActive += StartTurn;
            BattleManager.OnGameEnded += StopTimer;

            var rightTurn = turn.GetTurn(GameTurnType.RightPlayerTurn);
            rightTurn.OnTurnActive += StartTurn;
            rightTurn.OnTurnEnter += StopTimer;
            data.OnBattleManagerDestroyed += Dispose;

            turn.GetTurn(GameTurnType.ExitBattle).OnTurnActive += StopTimer;
        }
    }

    public void Dispose(IBattleManager bm)
    {
        var turn = bm.TurnHandler;
        bm.OnBattleManagerDestroyed -= Dispose;
        OnCounterDepleted -= turn.MoveToNextTurn;
        var leftTurn = turn.GetTurn(GameTurnType.LeftPlayerTurn);
        leftTurn.OnTurnExit -= StopTimer;
        leftTurn.OnTurnActive -= StartTurn;

        var rightTurn = turn.GetTurn(GameTurnType.RightPlayerTurn);
        rightTurn.OnTurnActive -= StartTurn;
        rightTurn.OnTurnEnter -= StopTimer;
        GameTurn.OnTurnFinished -= FinishCounting;
        BattleManager.OnGameEnded -= StopTimer;

        turn.GetTurn(GameTurnType.ExitBattle).OnTurnActive -= StopTimer;

        if (_sequence != null)
            _sequence.Kill();
        _sequence = null;
    }
}
