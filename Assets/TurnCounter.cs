using Battle.Turns;
using ReiTools.TokenMachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnCounter : MonoBehaviour
{
    private static TokenMachine _timerTokenMachine;
    public static ITokenReciever TimerTokenMachine => _timerTokenMachine;

    [SerializeField]
    private TextMeshProUGUI _timerText;
    [SerializeField]
    private float _timeTillEndTurn;

    private float _counter;
    private bool _toStopTimer;
    private Coroutine _coroutineTimer;

    private void Awake()
    {
        PlayerTurn.OnStartTurn += StartTime;
        EnemyTurn.OnStartTurn += StartTime;
        TurnHandler.OnFinishTurn += FinishCounting;
               _timerTokenMachine = new TokenMachine(StopTimer, ContinueTimer);
        ResetTimer();
    }
    private void OnDestroy()
    {
        PlayerTurn.OnStartTurn -= StartTime;
        EnemyTurn.OnStartTurn -= StartTime;
        TurnHandler.OnFinishTurn -= FinishCounting;
    }
    public void StartTime()
    {
        ResetTimer();
        _coroutineTimer = StartCoroutine(Count());
        _timerText.gameObject.SetActive(true);
    }
    private void ResetTimer() => _counter = _timeTillEndTurn;
    private IEnumerator Count()
    {
        while (_counter > 0)
        {
            if (!_toStopTimer)
            {
            _counter-= Time.deltaTime;
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
        //  do somet cool stuff
        _timerText.text = roundTime.ToString();
    }
    private void FinishCounting()
    {
        StopCoroutine(_coroutineTimer);
        _timerText.gameObject.SetActive(false);
    }
    private void ContinueTimer() => _toStopTimer = false;
    private void StopTimer() => _toStopTimer = true;
}
