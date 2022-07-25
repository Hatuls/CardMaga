using Battle.Turns;
using DG.Tweening;
using ReiTools.TokenMachine;
using System.Collections;
using TMPro;
using UnityEngine;
public class TurnCounter : MonoBehaviour
{
    private static TokenMachine _timerTokenMachine;
    public static ITokenReciever TimerTokenMachine => _timerTokenMachine;
    [SerializeField]
    private TextMeshProUGUI _timerText;
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




    private int _textTime;
    private float _counter;
    private bool _toStopTimer;
    private Coroutine _coroutineTimer;
    private float _startScale;
    private Sequence _sequence;


    private void Awake()
    {
        _startScale = _timerText.rectTransform.localScale.x;
        PlayerTurn.OnStartTurn += StartTime;
        EnemyTurn.OnStartTurn += StartTime;
        TurnHandler.OnFinishTurn += FinishCounting;
        _timerTokenMachine = new TokenMachine(StopTimer, ContinueTimer);
        ResetTimer();
        ResetScale();
    }
    private void OnDestroy()
    {
        PlayerTurn.OnStartTurn -= StartTime;
        EnemyTurn.OnStartTurn -= StartTime;
        TurnHandler.OnFinishTurn -= FinishCounting;
        if (_sequence != null)
            _sequence.Kill();
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
        _timerText.rectTransform.localScale = _startScale * Vector3.one;
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
        TurnHandler.FinishTurn();
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
            _sequence = _timerText.rectTransform.Transition(_timerTextTransition);
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


}
